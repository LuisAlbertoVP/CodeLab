using CodeLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeLab.Infrastructure.SqlServer.Providers;

public sealed class CodeLabContext : DbContext
{
    private readonly string? _connectionString;

    public CodeLabContext(string? connectionString) => this._connectionString = connectionString;
    public CodeLabContext(DbContextOptions<CodeLabContext> options) : base(options) { }

    public DbSet<Parametros> Parametros { get; set; }
    public DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
            optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parametros>().HasKey(p => p.Nombre);
    }
}