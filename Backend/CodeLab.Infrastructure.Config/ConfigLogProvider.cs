using CodeLab.Application.Interfaces.Logging;
using Microsoft.Extensions.Configuration;

namespace CodeLab.Infrastructure.Config;

public class ConfigLogProvider(IConfiguration configuration) : IConfigLogProvider
{
    public string Ruta => configuration["SerilogSettings:Ruta"];
}