using CodeLab.Application.Contracts.Providers.Interfaces;
using Serilog;

namespace CodeLab.Infrastructure.Logging.Configurations;

public class SerilogConfiguration(IConfigLogProvider configLogProvider)
{    
    public void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Async(x => x.File(
                configLogProvider.Ruta,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true
            ))
            .CreateLogger();
    }
}