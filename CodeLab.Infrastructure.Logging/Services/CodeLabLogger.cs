using CodeLab.Infrastructure.Logging.Contracts.Interfaces;
using Serilog;

namespace CodeLab.Infrastructure.Logging.Services;

public class CodeLabLogger : ICodeLabLogger
{
    public void LogInformation(string message)
    {
        Log.Information(message);
    }

    public void LogWarning(string message)
    {
        Log.Warning(message);
    }

    public void LogError(string message, Exception ex)
    {
        Log.Error(ex, message);
    }
}