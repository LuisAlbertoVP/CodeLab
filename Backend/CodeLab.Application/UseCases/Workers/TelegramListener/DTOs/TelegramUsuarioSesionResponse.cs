using System;

namespace CodeLab.Application.UseCases.Workers.TelegramListener.DTOs;

public class TelegramUsuarioSesionResponse
{
    public string Mensaje { get; set; }
    public bool Reset { get; set; }

    public TelegramUsuarioSesionResponse(string mensaje, bool reset = false)
    {
        Mensaje = mensaje;
        Reset = reset;
    }
}