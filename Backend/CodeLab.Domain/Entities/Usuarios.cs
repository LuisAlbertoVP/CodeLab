using System;

namespace CodeLab.Domain.Entities;

public class Usuarios
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Clave { get; set; }
    public DateTime FechaCreacion { get; set; }
}