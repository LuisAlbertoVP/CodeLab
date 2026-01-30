using CodeLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeLab.Infrastructure.SqlServer.Providers;

public sealed class CodeLabContext : DbContext
{
    private readonly string? _connectionString;

    public CodeLabContext(string? connectionString) => this._connectionString = connectionString;
    public CodeLabContext(DbContextOptions<CodeLabContext> options) : base(options) { }

    public DbSet<Parametros> Parametros { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }
    public DbSet<Roles> Roles { get; set; }
    public DbSet<Usuarios> Usuarios { get; set; }
    public DbSet<UsuarioRol> UsuarioRol { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
            optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parametros>().HasKey(p => p.Nombre);

        modelBuilder.Entity<Usuarios>()
            .HasMany(u => u.RefreshTokens)
            .WithOne(t => t.Usuario)
            .HasForeignKey(u => u.IdUsuario);

        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdRol });

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.UsuarioRol)
                .HasForeignKey(e => e.IdUsuario);

            entity.HasOne(e => e.Rol)
                .WithMany(r => r.UsuarioRol)
                .HasForeignKey(e => e.IdRol);
        });
    }
}