using Microsoft.Extensions.Configuration;

namespace CodeLab.Infrastructure.SqlServer.Providers;

public sealed class CodeLabConfigurationSource(string? connectionString) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder) =>
        new CodeLabConfigurationProvider(connectionString);
}