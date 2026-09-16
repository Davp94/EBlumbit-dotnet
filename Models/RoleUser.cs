using System;

namespace EBlumbit.Models;

public class RoleUser
{
    public int Id { get; set; }
    
    public int RoleId {get; set;}

    public int UserId {get; set;}

    public Role Role { get; set; }

    public Users User { get; set; }
}
