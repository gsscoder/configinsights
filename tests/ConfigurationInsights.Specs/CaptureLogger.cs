using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

class CaptureLogger : ILogger
{
    public IDictionary<LogLevel, string> Messages { get; private set; } = new Dictionary<LogLevel, string>();

    public IDisposable BeginScope<TState>(TState state) => default;

    public bool IsEnabled(LogLevel logLevel) => false;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var message = formatter(state, exception);

        Messages.Add(logLevel, message);
    }
}
