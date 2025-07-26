namespace CodeLab.Domain.Exceptions;

public class AuthException(string mensaje) : Exception(mensaje)
{
}