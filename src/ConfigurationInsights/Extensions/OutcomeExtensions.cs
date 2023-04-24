
using ConfigurationInsights;
using Microsoft.Extensions.Logging;

public static class OutcomeExtensions
{
    internal static Outcome Log(this Outcome outcome, AnalyzerOptions options)
    {
        if (outcome.Kind == OutcomeType.Ok && !options.EnableOkLogging)
            return outcome;
        switch (outcome.Kind) {
            default:
                options.Logger.LogInformation(outcome.Message);
                break;
            case OutcomeType.Warning:
                options.Logger.LogWarning(outcome.Message);
                break;
            case OutcomeType.Error:
                options.Logger.LogError(outcome.Message);
                break;
            case OutcomeType.Critical:
                options.Logger.LogCritical(outcome.Message);
                break;
        }
        if (options.EnableHintLogging && !string.IsNullOrWhiteSpace(outcome.MessageHint))
            options.Logger.LogInformation($"[HINT] {outcome.MessageHint}");
        return outcome;
    }

    public static bool HasError(this Outcome outcome)
    {
        return !(outcome.Kind == OutcomeType.Ok || outcome.Kind == OutcomeType.Warning);
    }
}
