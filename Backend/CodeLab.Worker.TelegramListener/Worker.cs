using System.Collections.Concurrent;
using CodeLab.Application.Contracts.Providers.Interfaces;
using Telegram.Bot;

namespace CodeLab.Worker.TelegramListener;

public class Worker(IConfigTelegramProvider configTelegramProvider) : BackgroundService
{
    private enum EstadoUsuario
    {
        Ninguno,
        EsperandoCorreo,
        EsperandoCodigo
    }

    private class UsuarioSession
    {
        public EstadoUsuario Estado { get; set; } = EstadoUsuario.Ninguno;
        public string Correo { get; set; }
        public string CodigoEsperado { get; set; }
    }

    private readonly ConcurrentDictionary<long, UsuarioSession> _sessions = new();
    
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var bot = new TelegramBotClient(configTelegramProvider.Token, cancellationToken: ct);
        int? offset = null;
        
        while (!ct.IsCancellationRequested)
        {
            var updates = await bot.GetUpdates(offset, timeout: 2, cancellationToken: ct);
            foreach (var update in updates)
            {
                offset = update.Id + 1;
                
                var chatId = update.Message.Chat.Id;
                var texto = update.Message.Text ?? string.Empty;

                Console.WriteLine(chatId);
                Console.WriteLine(texto);

                var session = _sessions.GetOrAdd(chatId, new UsuarioSession());

                try
                {
                    switch (session.Estado)
                    {
                        case EstadoUsuario.Ninguno:
                            await bot.SendMessage(chatId, "Por favor ingresa tu correo electrónico:", cancellationToken: ct);
                            session.Estado = EstadoUsuario.EsperandoCorreo;
                            break;

                        case EstadoUsuario.EsperandoCorreo:
                            session.Correo = texto;
                            session.CodigoEsperado = "test";
                            await bot.SendMessage(chatId, "Se ha enviado a tu correo electrónico un código, por favor escríbelo:", cancellationToken: ct);
                            session.Estado = EstadoUsuario.EsperandoCodigo;
                            break;

                        case EstadoUsuario.EsperandoCodigo:
                            if (texto == session.CodigoEsperado)
                            {
                                await bot.SendMessage(chatId, "Código correcto, ¡bienvenido a CodeLab!", cancellationToken: ct);
                                _sessions.TryRemove(chatId, out _);
                            }
                            else
                            {
                                await bot.SendMessage(chatId, "Código incorrecto, intenta de nuevo:", cancellationToken: ct);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                }
                if (ct.IsCancellationRequested) break;
            }
        }
    }
}