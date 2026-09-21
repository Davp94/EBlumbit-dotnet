using System;

namespace EBlumbit.Models;

public class Role
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string? Descripcion { get; set; }

    public ICollection<RoleUser> RoleUsers { get; set; } = [];

    public ICollection<PermissionRole> PermissionRoles { get; set; } = [];
}
