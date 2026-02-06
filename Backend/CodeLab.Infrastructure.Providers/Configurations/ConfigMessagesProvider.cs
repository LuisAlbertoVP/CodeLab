using CodeLab.Application.Contracts.Providers.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CodeLab.Infrastructure.Providers.Configurations;

public class ConfigMessagesProvider(IConfiguration configuration) : IConfigMessagesProvider
{
    public string ErrorGenerico => configuration["Messages:ErrorGenerico"];

    public string Timeout => configuration["Messages:Timeout"];
}