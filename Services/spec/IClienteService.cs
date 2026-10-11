using EBlumbit.Dto.Clientes;

namespace EBlumbit.Services.spec;

public interface IClienteService
{
    Task<IEnumerable<ClienteResponseDto>> GetAllClientes();
    Task<ClienteResponseDto?> GetClienteById(int id);
    Task<ClienteResponseDto> CreateCliente(ClienteRequestDto requestDto);
    Task<ClienteResponseDto> UpdateCliente(int id, ClienteRequestDto requestDto);
    Task DeleteCliente(int id);
}