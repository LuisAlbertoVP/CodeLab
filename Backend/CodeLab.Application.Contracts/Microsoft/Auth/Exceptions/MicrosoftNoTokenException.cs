namespace CodeLab.Application.Contracts.Microsoft.Auth.Exceptions;

public class MicrosoftNoTokenException(string message) : Exception(message)
{
}