namespace CodeLab.Application.Contracts.Providers.Interfaces;

public interface IConfigMessagesProvider
{
    string ErrorGenerico { get; }
    
    string Timeout { get; }
}