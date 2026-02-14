using System.Reflection;
using CodeLab.Api.Web.Middleware;
using CodeLab.Application.Contracts.Providers.Interfaces;
using CodeLab.Application.Shared.Extensions;
using CodeLab.Infrastructure.Logging.Extensions;
using CodeLab.Infrastructure.SqlServer.Extensions;
using CodeLab.Infrastructure.SqlServer.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Microsoft.OpenApi.Models;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddSqlServerConfiguration();
    builder.Services.AddScoped<CodeLabInterceptor>();
    builder.Services.AddDbContext<CodeLabContext>((serviceProvider, options) =>
    {
        var interceptor = serviceProvider.GetRequiredService<CodeLabInterceptor>();
        options.UseSqlServer(builder.Configuration.GetConnectionString("CodeLabDatabase"))
            .AddInterceptors(interceptor);
    });

    var assemblies = DependencyContext.Default.RuntimeLibraries
        .Where(lib => lib.Name.StartsWith("CodeLab"))
        .Select(lib => Assembly.Load(new AssemblyName(lib.Name)))
        .ToArray();
    builder.Services.AddDiscoveryServices(assemblies);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "CodeLab", Version = "v1" });

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

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularApp", policy =>
        {
            policy.WithOrigins("https://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

    builder.Services.AddControllers();

    builder.Host.UseSerilog((context, services, cfg) =>
    {
        using var scope = services.CreateScope();
        var configLogProvider = scope.ServiceProvider.GetRequiredService<IConfigLogProvider>();
        cfg.ConfigureLogger(configLogProvider);
    });

    Console.WriteLine("Configuración de servicios completada.");

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("AllowAngularApp");
    app.MapControllers();
    app.Run(); 
}
catch (Exception ex)
{
    Console.WriteLine($"Error al iniciar la aplicación: {ex.Message}");
    throw;
}