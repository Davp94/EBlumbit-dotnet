using System;
using EBlumbit.Models;
using Microsoft.EntityFrameworkCore;

namespace EBlumbit.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    //FLUENT API
    public DbSet<Users> Users => Set<Users>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<PermissionRole> PermissionRoles => Set<PermissionRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Users>()
        .HasIndex(u=> u.Email)
        .IsUnique();

        modelBuilder.Entity<Users>()
        .HasIndex(u=> u.Name)
        .IsUnique();

        //ROLES
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e=>e.Id);
            entity.Property(e=>e.Id).HasColumnName("id");
            entity.Property(e=>e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
            entity.Property(e=>e.Descripcion).HasColumnName("descripcion");
        });

        //RoleUser
        modelBuilder.Entity<RoleUser>(entity =>
        {
            entity.ToTable("role_user");
            entity.HasKey(e=>e.Id);
            entity.Property(e=>e.RoleId).HasColumnName("role_id");
            entity.Property(e=>e.UserId).HasColumnName("user_id");

            entity.HasOne(e=>e.Role).WithMany(r=>r.RoleUsers).HasForeignKey(e=>e.RoleId);
            entity.HasOne(e=>e.User).WithMany(u=>u.RoleUsers).HasForeignKey(e=>e.UserId);
        });

        //PERMISSIONS
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permissions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Detalle).HasColumnName("detalle");
            entity.Property(e => e.Subject).HasColumnName("subject").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Action).HasColumnName("action").IsRequired().HasMaxLength(100);
        });

        //PermissionRole
        modelBuilder.Entity<PermissionRole>(entity =>
        {
            entity.ToTable("permission_role");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");

            entity.HasOne(e => e.Role).WithMany(r => r.PermissionRoles).HasForeignKey(e => e.RoleId);
            entity.HasOne(e => e.Permission).WithMany(p => p.PermissionRoles).HasForeignKey(e => e.PermissionId);
        });
    }
}
