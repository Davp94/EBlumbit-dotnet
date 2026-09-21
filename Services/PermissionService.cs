using System;
using EBlumbit.Models;
using EBlumbit.Repository;

namespace EBlumbit.Services;

public class PermissionService(PermissionRepository permissionRepository)
{
    private readonly PermissionRepository _permissionRepository = permissionRepository;

    public async Task<IEnumerable<Permission>> GetAllPermissions()
    {
        return await _permissionRepository.GetAllPermissionsAsync();
    }

    public async Task<Permission?> GetPermissionById(int id)
    {
        return await _permissionRepository.GetPermissionByIdAsync(id);
    }

    public async Task<Permission> CreatePermission(Permission permission)
    {
        return await _permissionRepository.CreatePermissionAsync(permission);
    }

    public async Task<Permission> UpdatePermission(Permission permission)
    {
        return await _permissionRepository.UpdatePermissionAsync(permission);
    }

    public async Task DeletePermission(int id)
    {
        await _permissionRepository.DeleteAsync(id);
    }
}
