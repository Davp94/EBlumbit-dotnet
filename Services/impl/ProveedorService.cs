using EBlumbit.Builders;
using EBlumbit.Dto.Proveedores;
using EBlumbit.exceptions;
using EBlumbit.Repository.spec;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class ProveedorService(IProveedorRepository proveedorRepository) : IProveedorService
{
    private readonly IProveedorRepository _proveedorRepository = proveedorRepository;

    public async Task<IEnumerable<ProveedorResponseDto>> GetAllProveedores()
    {
        var proveedores = await _proveedorRepository.GetAllProveedores();
        return proveedores.Select(ProveedoresBuilder.ToResponseDto);
    }

    public async Task<ProveedorResponseDto?> GetProveedorById(int id)
    {
        var proveedor = await _proveedorRepository.GetProveedorById(id);
        if (proveedor == null)
        {
            throw new ResourceNotFoundException("Proveedor no encontrado");
        }
        return ProveedoresBuilder.ToResponseDto(proveedor);
    }

    public async Task<ProveedorResponseDto> CreateProveedor(ProveedorRequestDto requestDto)
    {
        var proveedor = await _proveedorRepository.CreateProveedor(ProveedoresBuilder.ToEntity(requestDto));
        return ProveedoresBuilder.ToResponseDto(proveedor);
    }

    public async Task<ProveedorResponseDto> UpdateProveedor(int id, ProveedorRequestDto requestDto)
    {
        var proveedor = await _proveedorRepository.GetProveedorById(id);
        if (proveedor == null)
        {
            throw new ResourceNotFoundException("Proveedor no encontrado");
        }

        await _proveedorRepository.UpdateProveedor(ProveedoresBuilder.ToEntityUpdate(requestDto, proveedor));
        var updated = await _proveedorRepository.GetProveedorById(id);
        return ProveedoresBuilder.ToResponseDto(updated!);
    }

    public async Task DeleteProveedor(int id)
    {
        var proveedor = await _proveedorRepository.GetProveedorById(id);
        if (proveedor == null)
        {
            throw new ResourceNotFoundException("Proveedor no encontrado");
        }
        await _proveedorRepository.DeleteProveedor(id);
    }
}