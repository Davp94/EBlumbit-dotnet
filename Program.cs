using System.Text;
using EBlumbit.Authorization;
using EBlumbit.Data;
using EBlumbit.Repository;
using EBlumbit.Seeders;
using EBlumbit.Services;
using EBlumbit.Services.impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

//repositories
builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<PermissionRepository>();

//Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<AuthService>();
//Seeders
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

//Auth JWT
builder.Services.AddAuthentication(options =>
{
   options.DefaultAuthenticateScheme =  JwtBearerDefaults.AuthenticationScheme;
   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("leer:users", policy => policy.Requirements.Add(new PermissionRequirement("leer:users")));
    options.AddPolicy("leer:compras", policy => policy.Requirements.Add(new PermissionRequirement("leer:compras")));
});

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
