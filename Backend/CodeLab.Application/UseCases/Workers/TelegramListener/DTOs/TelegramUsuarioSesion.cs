using CodeLab.Application.UseCases.Workers.TelegramListener.Enums;

namespace CodeLab.Application.UseCases.Workers.TelegramListener.DTOs;

public class TelegramUsuarioSesion
{
    public TelegramEstadoUsuario Estado { get; set; } = TelegramEstadoUsuario.Ninguno;
    public string Correo { get; set; }
    public int IdUsuario { get; set; }
    public string CodigoEsperado { get; set; }
    public int Intentos { get; set; } = 0;
}