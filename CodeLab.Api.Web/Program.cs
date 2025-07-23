using CodeLab.Application.AppServices;
using CodeLab.Application.Interfaces;
using CodeLab.Domain.DTOs;
using CodeLab.Domain.Interfaces;
using CodeLab.Infrastructure.SqlServer.Data;
using CodeLab.Infrastructure.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CodeLabContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodeLabDatabase")));

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/auth", async (UsuarioAuthDto usuario, IAuthService service) =>
{
    return await service.ValidarUsuario(usuario.Email, usuario.Clave)
        ? Results.Ok("Usuario válido")
        : Results.Unauthorized();
})
.WithName("ValidarUsuario")
.WithOpenApi();

app.Run();