using EBlumbit.Dto.Proveedores;

namespace EBlumbit.Services.spec;

public interface IProveedorService
{
    Task<IEnumerable<ProveedorResponseDto>> GetAllProveedores();
    Task<ProveedorResponseDto?> GetProveedorById(int id);
    Task<ProveedorResponseDto> CreateProveedor(ProveedorRequestDto requestDto);
    Task<ProveedorResponseDto> UpdateProveedor(int id, ProveedorRequestDto requestDto);
    Task DeleteProveedor(int id);
}