using System;
using EBlumbit.Builders;
using EBlumbit.Dto.Clientes;
using EBlumbit.exceptions;
using EBlumbit.Repository.spec;
using EBlumbit.Services.spec;

namespace EBlumbit.Services.impl;

public class ClienteService(IClienteRepository clienteRepository) : IClienteService
{
    private readonly IClienteRepository _clienteRepository = clienteRepository;

    public async Task<IEnumerable<ClienteResponseDto>> GetAllClientes()
    {
        var clientes = await _clienteRepository.GetAllClientes();
        return clientes.Select(ClientesBuilder.ToResponseDto);
    }

    public async Task<ClienteResponseDto?> GetClienteById(int id)
    {
        var cliente = await _clienteRepository.GetClienteById(id);
        if (cliente  == null)
        {
            throw new ResourceNotFoundException($"Cliente no encontrado");
        }
        return ClientesBuilder.ToResponseDto(cliente);
    }

    public async Task<ClienteResponseDto> CreateCliente(ClienteRequestDto requestDto)
    {
        var entity = ClientesBuilder.ToEntity(requestDto);
        var cliente = await _clienteRepository.CreateCliente(entity);
         return ClientesBuilder.ToResponseDto(cliente); 
    }

    public async Task<ClienteResponseDto> UpdateCliente(int id, ClienteRequestDto requestDto)
    {
        var clienteToUpdate = await _clienteRepository.GetClienteById(id);
        if (clienteToUpdate  == null)
        {
            throw new ResourceNotFoundException($"Cliente no encontrado");
        }
        var entityToUpdate = ClientesBuilder.ToEntityUpdate(requestDto, clienteToUpdate);
        await _clienteRepository.UpdateCliente(entityToUpdate);
        var updated = await _clienteRepository.GetClienteById(id);
        return ClientesBuilder.ToResponseDto(updated);
    }

    public async Task DeleteCliente(int id)
    {
        var clienteToDelete = await _clienteRepository.GetClienteById(id);
        if (clienteToDelete  == null)
        {
            throw new ResourceNotFoundException($"Cliente no encontrado");
        }
        await _clienteRepository.DeleteCliente(id);
    }
}
