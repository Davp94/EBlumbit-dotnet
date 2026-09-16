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
}
