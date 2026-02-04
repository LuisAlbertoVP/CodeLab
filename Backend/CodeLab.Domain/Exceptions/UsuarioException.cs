namespace CodeLab.Domain.Exceptions;

public class UsuarioException(string message) : DomainException(message)
{
}

public class UsuarioNoHabilitadoExcepion() : 
    DomainException("El usuario no se encuentra habilitado.")
{
}

public class CredencialesIncorrectasExcepion() : 
    DomainException("Credenciales incorrectas.")
{
}