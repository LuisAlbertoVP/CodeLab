using CodeLab.Infrastructure.Logging.Contracts.Settings;
using Serilog;

namespace CodeLab.Infrastructure.Logging.Configurations;

public static class SerilogConfiguration
{
    public static void ConfigureLogger(SerilogSettings settings)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(settings.Ruta,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();
        }
}