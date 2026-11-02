using EBlumbit.Dto.Almacenes;
using EBlumbit.exceptions;
using EBlumbit.Models;
using EBlumbit.Repository;

namespace EBlumbit.Services;

public class AlmacenService(AlmacenRepository almacenRepository)
{
    private readonly AlmacenRepository _almacenRepository = almacenRepository;

    public async Task<IEnumerable<AlmacenResponseDto>> GetAll()
    {
        var almacenes = await _almacenRepository.GetAllAsync();
        return almacenes.Select(ToResponseDto);
    }

    public async Task<AlmacenResponseDto> GetById(int id)
    {
        var almacen = await _almacenRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException($"Almacen con id {id} no encontrado");
        return ToResponseDto(almacen);
    }

    public async Task<AlmacenResponseDto> Create(AlmacenRequestDto request)
    {
        var created = await _almacenRepository.CreateAsync(new Almacenes
        {
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            SucursalId = request.SucursalId
        });
        return ToResponseDto(created);
    }

    public async Task<AlmacenResponseDto> Update(int id, AlmacenRequestDto request)
    {
        var almacen = await _almacenRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException($"Almacen con id {id} no encontrado");

        almacen.Codigo = request.Codigo;
        almacen.Nombre = request.Nombre;
        almacen.Descripcion = request.Descripcion;
        almacen.SucursalId = request.SucursalId;

        var updated = await _almacenRepository.UpdateAsync(almacen);
        return ToResponseDto(updated);
    }

    public async Task Delete(int id)
    {
        var almacen = await _almacenRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException($"Almacen con id {id} no encontrado");
        await _almacenRepository.DeleteAsync(almacen.Id);
    }

    private static AlmacenResponseDto ToResponseDto(Almacenes almacen)
    {
        return new AlmacenResponseDto
        {
            Id = almacen.Id,
            Codigo = almacen.Codigo,
            Nombre = almacen.Nombre,
            Descripcion = almacen.Descripcion,
            SucursalId = almacen.SucursalId,
            SucursalNombre = almacen.Sucursal?.Nombre ?? string.Empty
        };
    }
}
