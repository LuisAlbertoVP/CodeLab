using CodeLab.Domain.Events;

namespace CodeLab.Domain.Entities;

public class Usuarios : BaseEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Clave { get; set; }
    public bool Estado { get; set; }
    public int IntentosFallidos { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime UltimoAcceso { get; set; }
    public List<UsuarioRol>? UsuarioRol { get; set; }
    public List<RefreshToken>? RefreshTokens { get; set; }

    public void Autenticar(string clave)
    {
        if (!string.IsNullOrWhiteSpace(clave))
            throw new Exception();

        AddDomainEvent(new UsuarioAutenticadoEvent(this));
    }
}