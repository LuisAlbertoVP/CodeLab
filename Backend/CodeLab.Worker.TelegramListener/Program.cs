using CodeLab.Application.Contracts.Database.Interfaces;
using CodeLab.Application.Contracts.Jwt.Interfaces;
using CodeLab.Application.Contracts.Logging.Interfaces;
using CodeLab.Application.Contracts.Providers.Interfaces;
using CodeLab.Application.Contracts.Telegram.Interfaces;
using CodeLab.Application.Shared.Extensions;
using CodeLab.Domain.Interfaces;
using CodeLab.Infrastructure.Fallback;
using CodeLab.Infrastructure.Logging.Extensions;
using CodeLab.Infrastructure.Logging.Services;
using CodeLab.Infrastructure.Providers.Configurations;
using CodeLab.Infrastructure.SqlServer.Extensions;
using CodeLab.Infrastructure.SqlServer.Providers;
using CodeLab.Infrastructure.SqlServer.Repositories;
using CodeLab.Infrastructure.Telegram.Services;
using CodeLab.Worker.TelegramListener;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddSqlServerConfiguration();
builder.Services.AddScoped<CodeLabInterceptor>();
builder.Services.AddDbContext<CodeLabContext>((serviceProvider, options) =>
{
    var interceptor = serviceProvider.GetRequiredService<CodeLabInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodeLabDatabase"))
        .AddInterceptors(interceptor);
});

builder.Services.AddSingleton<IConfigMessagesProvider, ConfigMessagesProvider>();

builder.Services.AddSingleton<IConfigLogProvider, ConfigLogProvider>();
builder.Services.AddSingleton<ICodeLabLogger, CodeLabLogger>();

builder.Services.AddSingleton<IConfigTelegramProvider, ConfigTelegramProvider>();
builder.Services.AddSingleton<ITelegramService, TelegramService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<IJwtService, JwtFallbackService>();

builder.Services.AddApplicationServices();

builder.Services.AddSerilog((services, loggerConfiguration) => 
{
    var configLogProvider = services.GetRequiredService<IConfigLogProvider>();
    loggerConfiguration.ConfigureLogger(configLogProvider);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
