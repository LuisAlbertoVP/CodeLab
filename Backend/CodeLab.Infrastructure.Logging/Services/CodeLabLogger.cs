using CodeLab.Application.Contracts.Logging.Interfaces;
using Serilog;

namespace CodeLab.Infrastructure.Logging.Services;

public class CodeLabLogger : ICodeLabLogger
{
    public void LogInformation(string message)
    {
        Log.ForContext("isMyApp", true).Information(message);
    }

    public void LogWarning(string message)
    {
        Log.ForContext("isMyApp", true).Warning(message);
    }

    public void LogError(string message, Exception ex)
    {
        Log.ForContext("isMyApp", true).Error(ex, message);
    }
}