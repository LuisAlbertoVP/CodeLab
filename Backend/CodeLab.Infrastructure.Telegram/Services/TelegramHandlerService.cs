using CodeLab.Application.Contracts.Database.Interfaces;
using CodeLab.Application.Contracts.Telegram.DTOs;
using CodeLab.Application.Contracts.Telegram.Enums;
using CodeLab.Application.Contracts.Telegram.Interfaces;
using CodeLab.Domain.Entities;
using CodeLab.Domain.Enums;
using CodeLab.Domain.Interfaces;

namespace CodeLab.Infrastructure.Telegram.Services;

public class TelegramHandlerService(
    ITelegramUsuarioSesionService telegramUsuarioSesionService,
    IRepository<Usuarios> usuarioRepository,
    IRepository<UsuarioCanal> usuarioCanalRepository,
    IUnitOfWork unitOfWork
) : ITelegramHandlerService
{
    public async Task<TelegramUsuarioSesionResponse> HandleMessageAsync(long chatId, string message)
    {
        var session = telegramUsuarioSesionService.GetOrCreate(chatId);
        switch (session.Estado)
        {
            case TelegramEstadoUsuario.Ninguno:
                session.Estado = TelegramEstadoUsuario.EsperandoCorreo;
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
                    telegramUsuarioSesionService.Reset(chatId);
                    return new("Este correo ya está registrado con Telegram.");
                }
                session.Estado = TelegramEstadoUsuario.EsperandoCodigo;
                session.Correo = message;
                session.IdUsuario = usuario.Id;
                session.CodigoEsperado = "test";
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

                    await unitOfWork.SaveChangesAsync();
                    telegramUsuarioSesionService.Reset(chatId);

                    return new("Código correcto, ¡bienvenido a CodeLab!", true);
                }

                session.Intentos++;
                if (session.Intentos >= 3)
                {
                    telegramUsuarioSesionService.Reset(chatId);
                    return new("Has excedido el número de intentos.", true);
                }

                return new($"Código incorrecto ({session.Intentos}/3):");
        }

        return new("Ocurrió un error inesperado. Por favor, intenta de nuevo.");
    }
}