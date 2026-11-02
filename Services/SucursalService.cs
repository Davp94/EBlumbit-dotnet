using EBlumbit.Dto.Sucursales;
using EBlumbit.exceptions;
using EBlumbit.Models;
using EBlumbit.Repository;

namespace EBlumbit.Services;

public class SucursalService(SucursalRepository sucursalRepository)
{
    private readonly SucursalRepository _sucursalRepository = sucursalRepository;

    public async Task<IEnumerable<SucursalResponseDto>> GetAll()
    {
        var sucursales = await _sucursalRepository.GetAllAsync();
        return sucursales.Select(ToResponseDto);
    }

    public async Task<SucursalResponseDto> GetById(int id)
    {
        var sucursal = await _sucursalRepository.GetByIdAsync(id);
        if (sucursal == null)
        {
            throw new ResourceNotFoundException($"Sucursal con id {id} no encontrado");
        }
        return ToResponseDto(sucursal);
    }

    public async Task<SucursalResponseDto> Create(SucursalRequestDto request)
    {
        var created = await _sucursalRepository.CreateAsync(ToEntity(request));
        return ToResponseDto(created);
    }

    public async Task<SucursalResponseDto> Update(int id, SucursalRequestDto request)
    {
        var sucursal = await _sucursalRepository.GetByIdAsync(id);
        if (sucursal == null)
        {
            throw new ResourceNotFoundException($"Sucursal con id {id} no encontrado");
        }

        sucursal.Nombre = request.Nombre;
        sucursal.Direccion = request.Direccion;
        sucursal.Telefono = request.Telefono;
        sucursal.Ciudad = request.Ciudad;

        var updated = await _sucursalRepository.UpdateAsync(sucursal);
        return ToResponseDto(updated);
    }

    public async Task Delete(int id)
    {
        var sucursal = await _sucursalRepository.GetByIdAsync(id);
        if (sucursal == null)
        {
            throw new ResourceNotFoundException($"Sucursal con id {id} no encontrado");
        }
        await _sucursalRepository.DeleteAsync(id);
    }

    private static Sucursales ToEntity(SucursalRequestDto request)
    {
        return new Sucursales
        {
            Nombre = request.Nombre,
            Direccion = request.Direccion,
            Telefono = request.Telefono,
            Ciudad = request.Ciudad
        };
    }

    private static SucursalResponseDto ToResponseDto(Sucursales sucursal)
    {
        return new SucursalResponseDto
        {
            Id = sucursal.Id,
            Nombre = sucursal.Nombre,
            Direccion = sucursal.Direccion,
            Telefono = sucursal.Telefono,
            Ciudad = sucursal.Ciudad
        };
    }
}
