using System;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
