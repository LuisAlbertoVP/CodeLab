using CodeLab.Application.Contracts.Providers.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CodeLab.Infrastructure.Providers.Configurations;

public class ConfigTelegramProvider(IConfiguration configuration) : IConfigTelegramProvider
{
    public string Token => configuration["Telegram:Token"];

    public int LastOffset => int.Parse(configuration["Telegram:LastOffset"]);
}