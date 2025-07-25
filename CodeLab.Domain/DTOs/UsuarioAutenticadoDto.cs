using System;

namespace CodeLab.Domain.DTOs;

public class UsuarioAutenticadoDto
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Email { get; set; }
}
