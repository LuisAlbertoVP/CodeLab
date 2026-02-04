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
        modelBuilder.Entity<Parametros>(entity =>
        {
            entity.HasKey(p => p.Nombre);

            entity.HasData([
                new Parametros {
                    Nombre = "JwtSettings:Secret",
                    Valor = "9fK2mA7QpL!eRZx6W@D3#yU8T$hJ0CkN4B^EwV1SgM",
                    FechaCreacion = new DateTime(2026, 1, 1)
                },
                new Parametros {
                    Nombre = "JwtSettings:Issuer",
                    Valor = "localhost",
                    FechaCreacion = new DateTime(2026, 1, 1)
                },
                new Parametros {
                    Nombre = "JwtSettings:Audience",
                    Valor = "localhost",
                    FechaCreacion = new DateTime(2026, 1, 1)
                },
                new Parametros {
                    Nombre = "JwtSettings:ExpiryMinutes",
                    Valor = "15",
                    FechaCreacion = new DateTime(2026, 1, 1)
                },
                new Parametros {
                    Nombre = "SerilogSettings:Ruta",
                    Valor = "/home/Logs/CodeLab",
                    FechaCreacion = new DateTime(2026, 1, 1)
                }
            ]);
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.
                HasMany(u => u.RefreshTokens)
                .WithOne(t => t.Usuario)
                .HasForeignKey(u => u.IdUsuario);

            entity.
                HasData(new Usuarios
                {
                    Id = 1,
                    Email = "luisv-1@hotmail.com",
                    Clave = "12345",
                    Nombre = "Luis Velastegui",
                    Estado = true,
                    FechaCreacion = new DateTime(2026, 1, 1)
                });
        });

        modelBuilder.Entity<Roles>().HasData(
            new Roles
            {
                Id = 1,
                Codigo = "ADMIN",
                Nombre = "Administrador",
                UsuarioCreacion = 1,
                FechaCreacion = new DateTime(2026, 1, 1)
            }
        );


        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdRol });

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.UsuarioRol)
                .HasForeignKey(e => e.IdUsuario);

            entity.HasOne(e => e.Rol)
                .WithMany(r => r.UsuarioRol)
                .HasForeignKey(e => e.IdRol);

            entity
                .HasData(new UsuarioRol
                {
                    IdUsuario = 1,
                    IdRol = 1,
                    FechaAsignacion = new DateTime(2026, 1, 1)
                });
        });
    }
}