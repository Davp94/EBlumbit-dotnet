using System;

namespace EBlumbit.Models;

public class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; }

    public DateTime Created { get; set; }

    public DateTime Expires { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public Users User { get; set; }
}
