using CodeLab.Application.Contracts.Providers.Interfaces;
using Serilog;
using Serilog.Events;

namespace CodeLab.Infrastructure.Logging.Extensions;

public static class SerilogExtensions
{    
    public static LoggerConfiguration ConfigureLogger(this LoggerConfiguration cfg, IConfigLogProvider configLogProvider)
    {
        return cfg
            .WriteTo.Logger(lc =>
                lc.Filter.ByIncludingOnly(e => e.Properties.ContainsKey("isMyApp"))
                .WriteTo.Async(x => x.File(
                    configLogProvider.Ruta,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true
                ))
            )
            .WriteTo.Logger(lc =>
                lc.Filter.ByExcluding(e => e.Properties.ContainsKey("isMyApp"))
                .WriteTo.Async(x => x.Console())
            )
            .WriteTo.Logger(lc =>
                lc.Filter.ByIncludingOnly(e => 
                    !e.Properties.ContainsKey("isMyApp") && 
                    e.Level == LogEventLevel.Error && 
                    e.Level == LogEventLevel.Fatal)
                .WriteTo.Async(a => a.Map(
                    keySelector: e => "Framework",
                    configure: (key, x) =>
                    {
                        x.File(
                            Path.Combine(configLogProvider.Ruta, key),
                            rollingInterval: RollingInterval.Day,
                            rollOnFileSizeLimit: true
                        );
                    }
                ))
            );
    }
}