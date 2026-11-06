using EBlumbit.Dto.Roles;
using EBlumbit.Models;
using EBlumbit.Repository;

namespace EBlumbit.Services;

public class RoleService(RoleRepository roleRepository)
{
    private readonly RoleRepository _roleRepository = roleRepository;

    public async Task<IEnumerable<RoleResponseDto>> GetAllAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Select(ToResponseDto);
    }

    public async Task<RoleResponseDto?> GetByIdAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        return role == null ? null : ToResponseDto(role);
    }

    public async Task<RoleResponseDto> CreateAsync(RoleRequestDto request)
    {
        var role = new Role
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion
        };

        var created = await _roleRepository.CreateAsync(role);
        return ToResponseDto(created);
    }

    public async Task<RoleResponseDto> UpdateAsync(int id, RoleRequestDto request)
    {
        var role = await _roleRepository.GetByIdAsync(id)
            ?? throw new exceptions.ResourceNotFoundException($"Rol con id {id} no encontrado");

        role.Nombre = request.Nombre;
        role.Descripcion = request.Descripcion;

        var updated = await _roleRepository.UpdateAsync(role);
        return ToResponseDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id)
            ?? throw new exceptions.ResourceNotFoundException($"Rol con id {id} no encontrado");

        await _roleRepository.DeleteAsync(role);
    }

    private static RoleResponseDto ToResponseDto(Role role)
    {
        return new RoleResponseDto
        {
            Id = role.Id,
            Nombre = role.Nombre,
            Descripcion = role.Descripcion
        };
    }
}
