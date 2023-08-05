using System.Collections.Generic;
using ConfigurationInsights;

static class Defaults
{
    public static AnalyzerOptions GetAnalyzerOptions() => new AnalyzerOptions {
        Logger = new CaptureLogger(),
        EnableOkLogging = true,
        EnableHintLogging = true,
    };

    public static IDictionary<string, string> GetEmptyMetadata() => new Dictionary<string, string>();
}
