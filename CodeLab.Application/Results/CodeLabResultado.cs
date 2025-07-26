namespace CodeLab.Application.Results;

public class CodeLabResultado<T>
{
    public bool EsExito { get; private set; }

    public T Valor { get; private set; }

    public string MensajeError { get; private set; }

    public static CodeLabResultado<T> Exito(T valor) => new CodeLabResultado<T> { EsExito = true, Valor = valor };

    public static CodeLabResultado<T> Error(string mensajeError) => new CodeLabResultado<T> { EsExito = false, MensajeError = mensajeError };
}