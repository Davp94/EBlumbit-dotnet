using EBlumbit.Dto.Permissions;
using EBlumbit.Models;
using EBlumbit.Repository;

namespace EBlumbit.Services;

public class PermissionService(PermissionRepository permissionRepository)
{
    private readonly PermissionRepository _permissionRepository = permissionRepository;

    public async Task<IEnumerable<PermissionResponseDto>> GetAllPermissions()
    {
        var permissions = await _permissionRepository.GetAllPermissionsAsync();
        return permissions.Select(ToResponseDto);
    }

    public async Task<PermissionResponseDto?> GetPermissionById(int id)
    {
        var permission = await _permissionRepository.GetPermissionByIdAsync(id);
        return permission == null ? null : ToResponseDto(permission);
    }

    public async Task<PermissionResponseDto> CreatePermission(PermissionRequestDto request)
    {
        var created = await _permissionRepository.CreatePermissionAsync(ToEntity(request));
        return ToResponseDto(created);
    }

    public async Task<PermissionResponseDto> UpdatePermission(int id, PermissionRequestDto request)
    {
        var permission = await _permissionRepository.GetPermissionByIdAsync(id)
            ?? throw new exceptions.ResourceNotFoundException($"Permiso con id {id} no encontrado");

        permission.Nombre = request.Nombre;
        permission.Detalle = request.ResolveDetalle();
        permission.Subject = request.Subject;
        permission.Action = request.Action;

        var updated = await _permissionRepository.UpdatePermissionAsync(permission);
        return ToResponseDto(updated);
    }

    public async Task DeletePermission(int id)
    {
        await _permissionRepository.DeleteAsync(id);
    }

    private static Permission ToEntity(PermissionRequestDto request)
    {
        return new Permission
        {
            Nombre = request.Nombre,
            Detalle = request.ResolveDetalle(),
            Subject = request.Subject,
            Action = request.Action
        };
    }

    private static PermissionResponseDto ToResponseDto(Permission permission)
    {
        return new PermissionResponseDto
        {
            Id = permission.Id,
            Nombre = permission.Nombre,
            Detalle = permission.Detalle,
            Subject = permission.Subject,
            Action = permission.Action
        };
    }
}
