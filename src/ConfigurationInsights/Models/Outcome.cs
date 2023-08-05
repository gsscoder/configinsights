using SharpX;

namespace ConfigurationInsights;

public enum OutcomeType
{
    Failed, // An error occurred in analysis process
    Ok,
    Warning,
    Error,
    Critical
}

public class Outcome
{
    public OutcomeType Kind { get; private set; }
    
    public string Message { get; private set; }

    public string MessageHint { get; set; }

    public Outcome(OutcomeType type, string message)
    {
        Guard.DisallowNull(nameof(message), message);

        Kind = type;
        Message = message;
        MessageHint = string.Empty;
    }
}
