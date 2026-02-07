using CodeLab.Application.Contracts.Providers.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CodeLab.Infrastructure.Providers.Configurations;

public class ConfigLogProvider(IConfiguration configuration) : IConfigLogProvider
{
    public string Ruta => configuration["SerilogSettings:Ruta"];
}