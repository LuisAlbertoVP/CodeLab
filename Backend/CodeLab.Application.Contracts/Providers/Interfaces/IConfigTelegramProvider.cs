namespace CodeLab.Application.Contracts.Providers.Interfaces;

public interface IConfigTelegramProvider
{
    string Token { get; }
    
    int LastOffset { get; }
}