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
                ConsoleEx.WriteLine(ConsoleColor.Green, $"[TRC]: {message}");
                break;
            case LogLevel.Debug:
                ConsoleEx.WriteLine(ConsoleColor.Green, $"[DBG]: {message}");
                break;
            case LogLevel.Information:
                ConsoleEx.WriteLine(ConsoleColor.Blue, message);
                break;
            case LogLevel.Warning:
                ConsoleEx.WriteLine(ConsoleColor.Yellow, $"[WRN]: {message}");
                break;
            case LogLevel.Error:
                ConsoleEx.WriteErrorLine(ConsoleColor.Red, $"[ERR]: {message}");
                break;
            case LogLevel.Critical:
                ConsoleEx.WriteErrorLine(ConsoleColor.White, $"[CRITICAL]: {message}");
                break;
        }
    }
}
