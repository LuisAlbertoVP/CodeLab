using CodeLab.Domain.Events;
using CodeLab.Domain.Exceptions;

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
    public DateTime? UltimoAcceso { get; set; }
    public List<UsuarioRol> UsuarioRol { get; set; } = [];
    public List<RefreshToken> RefreshTokens { get; set; } = [];

    public void Autenticar(string clave)
    {        
        if (!PuedeAutenticarse())
            throw new UsuarioNoHabilitadoExcepion();

        if (clave != Clave)
        {
            RegistrarIntentoFallido();
            throw new CredencialesIncorrectasExcepion();
        }

        IntentosFallidos = 0;
        UltimoAcceso = DateTime.UtcNow;
        AddDomainEvent(new UsuarioAutenticadoEvent(this));
    }

    private bool PuedeAutenticarse() =>
        Estado && IntentosFallidos < 5;


    private void RegistrarIntentoFallido()
    {
        IntentosFallidos++;
        if (IntentosFallidos >= 5)
            Estado = false;
    }

    public void AgregarRefreshToken(RefreshToken refreshToken)
    {
        RefreshTokens.Add(refreshToken);
    }
}