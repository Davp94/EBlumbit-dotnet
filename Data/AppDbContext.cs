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

    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Sucursales> Sucursales => Set<Sucursales>();

     public DbSet<Venta> Ventas => Set<Venta>();

    public DbSet<DetalleVenta> DetalleVentas => Set<DetalleVenta>();

    public DbSet<Inventario> Inventarios => Set<Inventario>();

    public DbSet<Productos> Productos => Set<Productos>();

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<Almacenes> Almacenes => Set<Almacenes>();

    public DbSet<Compra> Compras => Set<Compra>();

    public DbSet<DetalleCompra> DetalleCompras => Set<DetalleCompra>();

    public DbSet<Proveedor> Proveedores => Set<Proveedor>();

    public  DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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

        //Categorias
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(e=>e.Id);
            entity.Property(e=>e.Id).HasColumnName("id");
            entity.Property(e=>e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
            entity.Property(e=>e.Detalle).HasColumnName("detalle");
        });

        //Sucursales
        modelBuilder.Entity<Sucursales>(entity =>
        {
            entity.ToTable("sucursales")
            .HasKey(s => s.Id);
            entity.Property(s => s.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(s => s.Nombre).HasColumnName("nombre")
                .HasColumnType("varchar(100)")
                .IsRequired();
            entity.Property(s => s.Direccion).HasColumnName("direccion")
                .HasColumnType("varchar(255)")
                .IsRequired();
            entity.Property(s => s.Telefono).HasColumnName("telefono")
                .HasColumnType("varchar(20)")
                .IsRequired();
            entity.Property(s => s.Ciudad).HasColumnName("ciudad")
                .HasColumnType("varchar(100)")
                .IsRequired();
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("proveedores");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.RazonSocial).HasColumnName("razon_social").HasMaxLength(255).IsRequired();
            entity.Property(e => e.NroIdentificacion).HasColumnName("nro_identificacion").HasMaxLength(30);
            entity.Property(e => e.Contacto).HasColumnName("contacto").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Telefono).HasColumnName("telefono").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Correo).HasColumnName("correo").HasMaxLength(150);
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");
            entity.Property(e => e.Estado).HasColumnName("estado");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.ToTable("compras");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.Codigo).IsUnique();
            entity.Property(e => e.Fecha).HasColumnName("fecha").IsRequired();
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id").IsRequired();
            entity.Property(e => e.UsuarioId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.DescuentoTotal).HasColumnName("descuento_total").HasPrecision(12, 2);
            entity.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Detalle).HasColumnName("detalle");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");

            entity.HasOne(e => e.Proveedor).WithMany(p => p.Compras).HasForeignKey(e => e.ProveedorId);
            entity.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.UsuarioId);
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.ToTable("detalle_compra");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.CompraId).HasColumnName("compra_id").IsRequired();
            entity.Property(e => e.ProductoId).HasColumnName("producto_id").IsRequired();
            entity.Property(e => e.AlmacenId).HasColumnName("almacen_id").IsRequired();
            entity.Property(e => e.Cantidad).HasColumnName("cantidad").IsRequired();
            entity.Property(e => e.PrecioUnitarioCompra).HasColumnName("precio_unitario_compra").HasPrecision(12, 2).IsRequired();
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");

            entity.HasOne(e => e.Compra).WithMany(c => c.DetalleCompras).HasForeignKey(e => e.CompraId);
            entity.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId);
            entity.HasOne(e => e.Almacen).WithMany().HasForeignKey(e => e.AlmacenId);
        });

        modelBuilder.Entity<Productos>(entity =>
        {
            entity.ToTable("productos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired();
            entity.Property(e => e.CodigoBarra).HasColumnName("codigo_barra");
            entity.Property(e => e.UnidadMedida).HasColumnName("unidad_medida");
            entity.Property(e => e.Marca).HasColumnName("marca");
            entity.Property(e => e.Imagen).HasColumnName("imagen");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.PrecioVentaActual).HasColumnName("precio_venta_actual").HasPrecision(12, 2);
            entity.Property(e => e.StockMinimo).HasColumnName("stock_minimo");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
        });

        modelBuilder.Entity<Almacenes>(entity =>
        {
            entity.ToTable("almacenes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.SucursalId).HasColumnName("sucursal_id").IsRequired();
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.ToTable("inventarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.CantidadActual).HasColumnName("cantidad_actual").IsRequired();
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion").IsRequired();
            entity.Property(e => e.ProductoId).HasColumnName("producto_id").IsRequired();
            entity.Property(e => e.AlmacenId).HasColumnName("almacen_id").IsRequired();
            entity.HasOne(e => e.producto).WithMany().HasForeignKey(e => e.ProductoId);
            entity.HasOne(e => e.almacen).WithMany().HasForeignKey(e => e.AlmacenId);
            entity.HasIndex(e => new { e.ProductoId, e.AlmacenId }).IsUnique();
        });

        modelBuilder.Entity<Users>().Property(e => e.State).HasColumnName("state");

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Token).HasColumnName("token").IsRequired();
            entity.Property(e => e.Created).HasColumnName("created").IsRequired();
            entity.Property(e => e.Expires).HasColumnName("expires").IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("is_active").IsRequired();
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        });

    }
}
