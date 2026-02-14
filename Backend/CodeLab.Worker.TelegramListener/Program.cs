using System.Reflection;
using CodeLab.Application.Contracts.Providers.Interfaces;
using CodeLab.Application.Shared.Extensions;
using CodeLab.Infrastructure.Logging.Extensions;
using CodeLab.Infrastructure.SqlServer.Extensions;
using CodeLab.Infrastructure.SqlServer.Providers;
using CodeLab.Worker.TelegramListener;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Serilog;
using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddSqlServerConfiguration();
builder.Services.AddScoped<CodeLabInterceptor>();
builder.Services.AddDbContext<CodeLabContext>((serviceProvider, options) =>
{
    var interceptor = serviceProvider.GetRequiredService<CodeLabInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodeLabDatabase"))
        .AddInterceptors(interceptor);
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = "localhost:6379";
    return ConnectionMultiplexer.Connect(configuration);
});

var assemblies = DependencyContext.Default.RuntimeLibraries
    .Where(lib => lib.Name.StartsWith("CodeLab"))
    .Select(lib => Assembly.Load(new AssemblyName(lib.Name)))
    .ToArray();
builder.Services.AddDiscoveryServices(assemblies);

builder.Services.AddSerilog((services, loggerConfiguration) => 
{
    using var scope = services.CreateScope();
    var configLogProvider = scope.ServiceProvider.GetRequiredService<IConfigLogProvider>();
    loggerConfiguration.ConfigureLogger(configLogProvider);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
