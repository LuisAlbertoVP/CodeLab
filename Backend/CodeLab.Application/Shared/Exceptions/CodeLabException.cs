using System.Net;

namespace CodeLab.Application.Shared.Exceptions;

public class CodeLabException : Exception
{
    public CodeLabException(HttpStatusCode httpStatusCode, string message) : base(message)
    {
        this.HttpStatusCode = httpStatusCode;
    }

    public HttpStatusCode HttpStatusCode { get; set; }
}