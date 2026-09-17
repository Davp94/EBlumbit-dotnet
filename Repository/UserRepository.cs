using System;
using EBlumbit.Data;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Repository;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    //LINQ
    public async Task<IEnumerable<Users>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync(); //select * from users
    }

    public async Task<Users?> GetusuarioById(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u=>u.Id == id); 
    }

    public async Task<Users> CreateUsuario(Users usuario)
    {
        _context.Users.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario; 
    }

    public async Task<Users> UpdateUsuario(Users usuario)
    {
        _context.Users.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await _context.Users.FindAsync(id);
        if(usuario != null)
        {
            _context.Users.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }

    public async Task LogicalDeleteAsync(int id)
    {
        var usuario = await _context.Users.FindAsync(id);
        if(usuario != null)
        {
            usuario.State = false;
            _context.Users.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
