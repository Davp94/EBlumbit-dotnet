using System;
using BCrypt.Net;
using EBlumbit.Dto.Users;
using EBlumbit.exceptions;
using EBlumbit.Models;
using EBlumbit.Repository;

namespace EBlumbit.Services;

public class UserService(UserRepository userRepository, PermissionRepository permissionRepository)
{
    private const string DefaultPassword = "Password123*";

    private readonly UserRepository _userRepository = userRepository;

    private readonly PermissionRepository _permissionRepository = permissionRepository;

    public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(ToResponseDto);
    }

    public async Task<UserResponseDto?> GetUserById(int id)
    {
        var user = await _userRepository.GetusuarioById(id);
        return user == null ? null : ToResponseDto(user);
    }

    public async Task<UserResponseDto> CreateUser(UserRequestDto request)
    {
        var username = ResolveUsername(request);
        var email = ResolveEmail(request);

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
        {
            throw new BusinessRuleException("El username y el email son obligatorios");
        }

        if (await _userRepository.ExistsByUsernameOrEmail(username, email))
        {
            throw new ConflictException("El username o el email ya existen");
        }

        if (request.RolIds != null && request.RolIds.Count > 0 && !await _userRepository.RolesExist(request.RolIds))
        {
            throw new BusinessRuleException("Uno o más roles no existen");
        }

        var password = string.IsNullOrWhiteSpace(request.Password) ? DefaultPassword : request.Password;

        var user = new Users
        {
            Name = username.Trim(),
            Email = email.Trim(),
            Nombres = request.Nombres,
            Apellidos = request.Apellidos,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            State = request.State ?? true
        };

        var created = await _userRepository.CreateUsuario(user);

        if (request.RolIds != null && request.RolIds.Count > 0)
        {
            await _userRepository.ReplaceRoles(created.Id, request.RolIds);
        }

        return ToResponseDto((await _userRepository.GetUsuarioForResponse(created.Id))!);
    }

    public async Task<UserResponseDto> UpdateUser(int id, UserRequestDto request)
    {
        var user = await _userRepository.GetusuarioById(id)
            ?? throw new ResourceNotFoundException($"Usuario con id {id} no encontrado");

        var username = ResolveUsername(request);
        var email = ResolveEmail(request);

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
        {
            throw new BusinessRuleException("El username y el email son obligatorios");
        }

        if (await _userRepository.ExistsByUsernameOrEmail(username, email, id))
        {
            throw new ConflictException("El username o el email ya existen");
        }

        if (request.RolIds != null && request.RolIds.Count > 0 && !await _userRepository.RolesExist(request.RolIds))
        {
            throw new BusinessRuleException("Uno o más roles no existen");
        }

        user.Name = username.Trim();
        user.Email = email.Trim();
        if (request.Nombres != null) user.Nombres = request.Nombres;
        if (request.Apellidos != null) user.Apellidos = request.Apellidos;
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }
        if (request.State.HasValue)
        {
            user.State = request.State.Value;
        }

        await _userRepository.UpdateUsuario(user);

        if (request.RolIds != null)
        {
            await _userRepository.ReplaceRoles(id, request.RolIds);
        }

        return ToResponseDto((await _userRepository.GetUsuarioForResponse(id))!);
    }

    public async Task DeleteUsuario(int id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public async Task LogicalDeleteUsuario(int id)
    {
        await _userRepository.LogicalDeleteAsync(id);
    }

    private static string? ResolveUsername(UserRequestDto request)
    {
        return (request.Username ?? request.Name)?.Trim();
    }

    private static string? ResolveEmail(UserRequestDto request)
    {
        return (request.Email ?? request.Correo)?.Trim();
    }

    private static UserResponseDto ToResponseDto(Users user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Name,
            Nombres = user.Nombres,
            Apellidos = user.Apellidos,
            Email = user.Email,
            State = user.State,
            RolIds = user.RoleUsers.Select(ru => ru.RoleId).Distinct().ToList(),
            PermissionsIds = user.RoleUsers
                .SelectMany(ru => ru.Role.PermissionRoles)
                .Select(pr => pr.PermissionId)
                .Distinct()
                .ToList()
        };
    }
}
