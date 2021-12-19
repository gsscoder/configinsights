using System;
using Microsoft.Extensions.Logging;

class Logger : ILogger
{
    readonly bool _verbose;

    public Logger (bool verbose)
    {
        _verbose = true;
    }

    public IDisposable BeginScope<TState>(TState state) => default;

    public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel.Trace ? _verbose : true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);
        switch (logLevel) {
            default:
                Console.WriteLine(message);
                break;
            case LogLevel.Debug:
                Console.WriteLine(message);
                break;
            case LogLevel.Information:
                Console.WriteLine(message);
                break;
            case LogLevel.Warning:
                ConsoleEx.WriteErrorLine(ConsoleColor.Yellow, $"[WRN]: {message}");
                break;
            case LogLevel.Error:
                ConsoleEx.WriteErrorLine(ConsoleColor.Red, $"[ERR]: {message}");
                break;
            case LogLevel.Critical:
                ConsoleEx.WriteErrorLine(ConsoleColor.Red, $"[CRITICAL]: {message}");
                break;
        }
    }
}
