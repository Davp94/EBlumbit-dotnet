using System;

namespace EBlumbit.Models;

public class Cliente
{
    //
    public ICollection<Users> Usuarios;
    public IEnumerable<Role> Roles;
    
    public List<Permission> Permissions;
}
