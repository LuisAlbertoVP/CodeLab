using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CodeLab.Infrastructure.SqlServer.Providers;

public class CodeLabContextFactory : IDesignTimeDbContextFactory<CodeLabContext>
{
    public CodeLabContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CodeLabContext>()
            .UseSqlServer(
                "Server=localhost,1433;Database=CodeLab;User Id=SA;Password=P@ssw0rd!;Encrypt=True;TrustServerCertificate=True"
            )
            .Options;

        return new CodeLabContext(options);
    }
}
