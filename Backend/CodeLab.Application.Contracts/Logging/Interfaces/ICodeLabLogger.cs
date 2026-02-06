namespace CodeLab.Application.Contracts.Logging.Interfaces;

public interface ICodeLabLogger
{
    void LogInformation(string message);

    void LogWarning(string message);

    void LogError(string message, Exception ex);
}