using Microsoft.Extensions.Configuration;

namespace CodeLab.Infrastructure.SqlServer.Providers;

public sealed class CodeLabConfigurationProvider(string? connectionString) : ConfigurationProvider
{
    public override void Load()
    {
        using var dbContext = new CodeLabContext(connectionString);

        Data = dbContext.Parametros.Any()
            ? dbContext.Parametros.ToDictionary(
                static c => c.Nombre,
                static c => c.Valor, StringComparer.OrdinalIgnoreCase)
            : [];
    }
}