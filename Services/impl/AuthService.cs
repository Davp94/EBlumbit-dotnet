using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using EBlumbit.Data;
using EBlumbit.Dto.Auth;
using EBlumbit.exceptions;
using EBlumbit.Models;
using EBlumbit.Services.spec;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EBlumbit.Services.impl;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
    private readonly AppDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Password)
            || (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Username)))
        {
            throw new AuthenticationException("Credenciales inválidas");
        }

        var email = request.Email?.Trim();
        var username = request.Username?.Trim();

        var query = _context.Users
            .Include(r => r.RoleUsers).ThenInclude(ro => ro.Role)
            .AsQueryable();

        var usuario = !string.IsNullOrWhiteSpace(email)
            ? await query.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower())
            : await query.FirstOrDefaultAsync(u => u.Name == username);

        if(usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
        {
            throw new AuthenticationException("Credenciales inválidas");
        }
        if(!usuario.State)
        {
            throw new AuthenticationException("Usuario inactivo");
        }
       
        var roles = usuario.RoleUsers.Select(ro => ro.Role.Nombre).Distinct().ToList();
        var rolesIds = usuario.RoleUsers.Select(ro => ro.Role.Id).ToList();
        var permissionsList = await _context.PermissionRoles.Include(r=>r.Permission).Where(pr=>rolesIds.Contains(pr.Role.Id)).ToListAsync();
        var permissions = permissionsList.Select(p=>p.Permission.Nombre).Distinct().ToList();

        var accessToken = GenerateAccessToken(usuario, permissions);
        var refreshToken = GenerateRefreshToken();
        usuario.RefreshTokens.Add(BuildRefreshToken(usuario.Id, refreshToken));
        await _context.SaveChangesAsync();
        var expiration = _configuration.GetValue<int>("Jwt:DurationInMinutes");
        return new AuthResponse
        (
            accessToken,
            refreshToken,
            usuario.Id,
            expiration,
            usuario.Email,
            roles,
            permissions,
            usuario.Id,
            expiration * 60_000L
        );
    }

    public async Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new AuthenticationException("Token de actualización inválido");
        }

        var usuario = await _context.Users
            .Include(u => u.RefreshTokens)
            .Include(u => u.RoleUsers).ThenInclude(ro => ro.Role)
            .FirstOrDefaultAsync(u=>u.RefreshTokens.Any(t=>t.Token == request.RefreshToken));

        if(usuario == null)
        {
            throw new AuthenticationException("Token de actualización inválido");
        }

        var refreshToken = usuario.RefreshTokens.Single(t=>t.Token == request.RefreshToken);
        if(!refreshToken.IsActive || refreshToken.Expires < DateTime.UtcNow)
        {
            throw new AuthenticationException("Token de actualización inválido o expirado");
        }

        if(!usuario.State)
        {
            throw new AuthenticationException("Usuario inactivo");
        }

        var roles = usuario.RoleUsers.Select(ro => ro.Role.Nombre).Distinct().ToList();
        var rolesIds = usuario.RoleUsers.Select(ro => ro.Role.Id).ToList();
        var permissionsList = await _context.PermissionRoles.Include(r=>r.Permission).Where(pr=>rolesIds.Contains(pr.Role.Id)).ToListAsync();
        var permissions = permissionsList.Select(p=>p.Permission.Nombre).Distinct().ToList();
        var expiration = _configuration.GetValue<int>("Jwt:DurationInMinutes");

        var newAccesToken = GenerateAccessToken(usuario, permissions);
        var newRefreshToken = GenerateRefreshToken();

        refreshToken.IsActive = false;
        usuario.RefreshTokens.Add(BuildRefreshToken(usuario.Id, newRefreshToken));
        await _context.SaveChangesAsync();

        return new AuthResponse
        (
            newAccesToken,
            newRefreshToken,
            usuario.Id,
            expiration,
            usuario.Email,
            roles,
            permissions,
            usuario.Id,
            expiration * 60_000L
        );


    }

    private RefreshToken BuildRefreshToken(int userId, string token)
    {
        return new RefreshToken
        {
            Token = token,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenDurationInDays")),
            IsActive = true,
            UserId = userId
        };
    }

    private string GenerateAccessToken(Users usuario, List<string> permissions)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim("UsuarioId", usuario.Id.ToString()),
        };
        foreach(var permission in permissions)
        {
            claims.Add(new Claim("Permission", permission));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:DurationInMinutes"));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: cred
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        RandomNumberGenerator.Fill(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
