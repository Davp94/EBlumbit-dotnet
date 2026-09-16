using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EBlumbit.Models;

namespace EBlumbit;

[Table("users")]
public class Users
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [MaxLength(250)]
    [Column("password")]
    public string Password { get; set; }

    public ICollection<RoleUser> RoleUsers { get; set; } = [];
}
