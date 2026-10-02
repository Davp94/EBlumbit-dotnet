using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using EBlumbit.Data;
using EBlumbit.Dto.Auth;
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
        var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if(usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
        {
            throw new InvalidOperationException("Credenciales inválidas");
        }
        if(!usuario.State)
        {
            throw new InvalidOperationException("Usuario inactivo");
        }
        var accessToken = GenerateAccessToken(usuario);
        var refreshToken = GenerateRefreshToken();
        var roles = usuario.RoleUsers.Select(ru => ru.Role.Nombre).ToList();
        var permissions = usuario.RoleUsers.SelectMany(ru=>ru.Role.Permisos.Select(p=>p.Nombre)).ToList();
        var expiration = _configuration.GetValue<int>("Jwt:DurationInMinutes");
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UsuarioId = usuario.Id,
            ExpirationMinutes = expiration,
            Email = usuario.Email,
            Roles = roles,
            Permissions = permissions
        };
    }

    public async Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
    {
        if(tokenValid)
        {
            
        } 

    }

    private string GenerateAccessToken(Users usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim("UsuarioId", usuario.Id.ToString())
        };

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
        return Convert.ToBase64String(randomNumber);
    }
}
