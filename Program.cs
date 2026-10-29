using System.Text;
using EBlumbit.Authorization;
using EBlumbit.Data;
using EBlumbit.exceptions;
using EBlumbit.Repository;
using EBlumbit.Repository.impl;
using EBlumbit.Repository.spec;
using EBlumbit.Seeders;
using EBlumbit.Services;
using EBlumbit.Services.impl;
using EBlumbit.Services.spec;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteDapperRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProveedorRepository, ProveedorDapperRepository>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();

//Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IComprasService, ComprasService>();
builder.Services.AddScoped<IFIleService, FileService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IVentasService, VentasService>();
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

    // Respuestas 401/403 con el mismo envelope ProblemDetails que el resto de errores.
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails
            {
                Status = 401,
                Title = "AUTHENTICATION_EXCEPTION",
                Detail = "No autorizado",
                Instance = context.Request.Path
            };
            problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
            await context.Response.WriteAsJsonAsync(problem);
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails
            {
                Status = 403,
                Title = "FORBIDDEN",
                Detail = "Acceso denegado",
                Instance = context.Request.Path
            };
            problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
            await context.Response.WriteAsJsonAsync(problem);
        }
    };
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("leer:users", policy => policy.Requirements.Add(new PermissionRequirement("leer:users")));
    options.AddPolicy("crear:users", policy => policy.Requirements.Add(new PermissionRequirement("crear:users")));
    options.AddPolicy("actualizar:users", policy => policy.Requirements.Add(new PermissionRequirement("actualizar:users")));
    options.AddPolicy("eliminar:users", policy => policy.Requirements.Add(new PermissionRequirement("eliminar:users")));
    options.AddPolicy("admin:ventas", policy => policy.Requirements.Add(new PermissionRequirement("admin:ventas")));
    options.AddPolicy("leer:compras", policy => policy.Requirements.Add(new PermissionRequirement("leer:compras")));
});

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        if (allowedOrigins.Length == 0)
        {
            policy.AllowAnyOrigin();
        }
        else
        {
            policy.WithOrigins(allowedOrigins);
        }
        policy.AllowAnyHeader().AllowAnyMethod();
    });
});
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

app.UseExceptionHandler();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
