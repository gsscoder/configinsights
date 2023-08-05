using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ConfigurationInsights;

public class AnalyzerOptions
{
    public AnalyzerOptions()
    {
        Logger = NullLogger.Instance;
    }

    public ILogger Logger { get; set; }

    public bool EnableOkLogging { get; set; }

    public bool EnableHintLogging { get; set; }
}
