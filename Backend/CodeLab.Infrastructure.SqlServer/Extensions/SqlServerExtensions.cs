using CodeLab.Infrastructure.SqlServer.Providers;
using Microsoft.Extensions.Configuration;

namespace CodeLab.Infrastructure.SqlServer.Extensions;

public static class SqlServerExtensions
{
    public static ConfigurationManager AddSqlServerConfiguration(this ConfigurationManager manager)
    {
        var connectionString = manager.GetConnectionString("CodeLabDatabase");

        IConfigurationBuilder configBuilder = manager;
        configBuilder.Add(new CodeLabConfigurationSource(connectionString));

        return manager;
    }
}