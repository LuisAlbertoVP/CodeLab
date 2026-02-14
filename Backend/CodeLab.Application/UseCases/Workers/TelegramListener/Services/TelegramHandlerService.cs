using CodeLab.Application.Contracts.Caching.Interfaces;
using CodeLab.Application.UseCases.Workers.TelegramListener.DTOs;
using CodeLab.Application.UseCases.Workers.TelegramListener.Enums;
using CodeLab.Application.UseCases.Workers.TelegramListener.Interfaces;
using CodeLab.Domain.Entities;
using CodeLab.Domain.Enums;
using CodeLab.Domain.Interfaces;

namespace CodeLab.Application.UseCases.Workers.TelegramListener.Services;

public class TelegramHandlerService(
    ICacheService cacheService,
    IRepository<Usuarios> usuarioRepository,
    IRepository<UsuarioCanal> usuarioCanalRepository
) : ITelegramHandlerService
{
    public async Task<TelegramUsuarioSesionResponse> HandleMessageAsync(long chatId, string message)
    {
        if (!cacheService.TryGetValue(chatId.ToString(), out TelegramUsuarioSesion session))
        {
            session = new TelegramUsuarioSesion();
            cacheService.Set(chatId.ToString(), session, TimeSpan.FromMinutes(15));
        }

        switch (session.Estado)
        {
            case TelegramEstadoUsuario.Ninguno:
                session.Estado = TelegramEstadoUsuario.EsperandoCorreo;
                cacheService.Set(chatId.ToString(), session, TimeSpan.FromMinutes(15));
                return new("Por favor ingresa tu correo electrónico:");

            case TelegramEstadoUsuario.EsperandoCorreo:
            {
                var usuario = await usuarioRepository.FirstOrDefaultAsync(u => u.Email == message);
                if (usuario == null)
                {
                    return new("Correo no registrado. Por favor, intenta de nuevo.");
                }
                if (await usuarioCanalRepository.AnyAsync(uc => uc.IdUsuario == usuario.Id && uc.Canal == OutboundChannel.Telegram))
                {
                    cacheService.Remove(chatId.ToString());
                    return new("Este correo ya está registrado con Telegram.");
                }
                session.Estado = TelegramEstadoUsuario.EsperandoCodigo;
                session.Correo = message;
                session.IdUsuario = usuario.Id;
                session.CodigoEsperado = "test";
                cacheService.Set(chatId.ToString(), session, TimeSpan.FromMinutes(15));
                return new("Por favor ingresa el código de verificación enviado a tu correo:");
            }

            case TelegramEstadoUsuario.EsperandoCodigo:
                if (message == session.CodigoEsperado)
                {
                    await usuarioCanalRepository.AddAsync(new UsuarioCanal
                    {
                        IdUsuario = session.IdUsuario,
                        Canal = OutboundChannel.Telegram,
                        Destino = chatId.ToString(),
                        FechaCreacion = DateTime.UtcNow,
                        FechaModificacion = DateTime.UtcNow
                    });

                    cacheService.Remove(chatId.ToString());

                    return new("Código correcto, ¡bienvenido a CodeLab!", true);
                }

                session.Intentos++;
                if (session.Intentos >= 3)
                {
                    cacheService.Remove(chatId.ToString());
                    return new("Has excedido el número de intentos.", true);
                }

                return new($"Código incorrecto ({session.Intentos}/3):");
        }

        return new("Ocurrió un error inesperado. Por favor, intenta de nuevo.");
    }
}