using EBlumbit.Data;
using EBlumbit.Repository;
using EBlumbit.Seeders;
using EBlumbit.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

//Serilog
builder.Host.UseSerilog((context, services, config) => 
    config.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
);

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PermissionRepository>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<PermisosSeeder>();
builder.Services.AddScoped<RolSeeder>();
builder.Services.AddScoped<UsuarioSeeder>();
builder.Services.AddScoped<CategoriaSeeder>();
builder.Services.AddScoped<SucursalSeeder>();
builder.Services.AddScoped<AlmacenSeeder>();
builder.Services.AddScoped<ProductoSeeder>();
builder.Services.AddScoped<InventarioSeeder>();
builder.Services.AddScoped<ClienteSeeder>();
builder.Services.AddScoped<ProveedorSeeder>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
