using CodeLab.Api.Web.Middleware;
using CodeLab.Application.Interfaces.Database;
using CodeLab.Application.Interfaces.Jwt;
using CodeLab.Application.Interfaces.Logging;
using CodeLab.Application.Shared.Extensions;
using CodeLab.Domain.Interfaces;
using CodeLab.Infrastructure.Jwt.Contracts.Interfaces;
using CodeLab.Infrastructure.Jwt.Contracts.Settings;
using CodeLab.Infrastructure.Jwt.Services;
using CodeLab.Infrastructure.Logging.Configurations;
using CodeLab.Infrastructure.Logging.Contracts.Settings;
using CodeLab.Infrastructure.Logging.Services;
using CodeLab.Infrastructure.RabbitMq.Contracts.Interfaces;
using CodeLab.Infrastructure.RabbitMq.Services;
using CodeLab.Infrastructure.SqlServer.Contracts.Interfaces;
using CodeLab.Infrastructure.SqlServer.Extensions;
using CodeLab.Infrastructure.SqlServer.Providers;
using CodeLab.Infrastructure.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    builder.Services.AddSingleton(jwtSettings);
    builder.Services.AddScoped<IJwtService, JwtService>();

    var serilogSettings = builder.Configuration.GetSection("SerilogSettings").Get<SerilogSettings>();
    SerilogConfiguration.ConfigureLogger(serilogSettings);
    builder.Services.AddSingleton<ICodeLabLogger, CodeLabLogger>();

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();

    builder.Services.AddApplicationServices();

    //builder.Services.AddSingleton<IMailPublisherService, MailPublisherService>();

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

    Console.WriteLine("Configuración de servicios completada.");

    var app = builder.Build();

    //var mailService = app.Services.GetRequiredService<IMailPublisherService>();
    //await mailService.InitializeAsync();

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