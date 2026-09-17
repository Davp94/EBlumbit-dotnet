using System;
using EBlumbit.Repository;

namespace EBlumbit.Services;

public class UserService(UserRepository userRepository)
{
    private readonly UserRepository _userRepository = userRepository;

    public async Task<IEnumerable<Users>> GetAllUsers()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<Users?> GetUserById(int id)
    {
        return await _userRepository.GetusuarioById(id);
    }

    public async Task<Users> CreateUser(Users user)
    {
        return await _userRepository.CreateUsuario(user);
    }

    public async Task<Users> UpdateUser(Users user)
    {
        return await _userRepository.UpdateUsuario(user);
    }

    public async Task DeleteUsuario(int id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public async Task LogicalDeleteUsuario(int id)
    {
        await _userRepository.LogicalDeleteAsync(id);
    }


}
