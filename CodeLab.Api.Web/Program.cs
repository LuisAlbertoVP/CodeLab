using CodeLab.Application.AppServices;
using CodeLab.Application.Interfaces;
using CodeLab.Domain.Interfaces;
using CodeLab.Infrastructure.Jwt.Contracts.DTOs;
using CodeLab.Infrastructure.Jwt.Contracts.Interfaces;
using CodeLab.Infrastructure.Jwt.Services;
using CodeLab.Infrastructure.SqlServer.Data;
using CodeLab.Infrastructure.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<CodeLabContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("CodeLabDatabase")));

    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettingsDto>()!;
    builder.Services.AddSingleton(jwtSettings);

    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    builder.Services.AddScoped<IJwtService, JwtService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Tu API", Version = "v1" });

        c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "basic",
            In = ParameterLocation.Header,
            Description = "Credenciales en formato Base64: username:password"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddControllers();

    Console.WriteLine("Configuración de servicios completada.");

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run(); 
}
catch (Exception ex)
{
    Console.WriteLine($"Error al iniciar la aplicación: {ex.Message}");
    throw;
}