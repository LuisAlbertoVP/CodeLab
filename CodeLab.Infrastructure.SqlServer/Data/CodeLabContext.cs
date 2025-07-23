using System;
using CodeLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeLab.Infrastructure.SqlServer.Data;

public class CodeLabContext(DbContextOptions<CodeLabContext> options) : DbContext(options)
{
    public DbSet<Usuarios> Usuarios { get; set; }
}