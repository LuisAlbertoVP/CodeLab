namespace CodeLab.Infrastructure.SqlServer.Contracts.Exceptions;

public class AuthException(string mensaje) : Exception(mensaje)
{
}