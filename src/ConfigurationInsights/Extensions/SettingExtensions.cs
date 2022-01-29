using ConfigurationInsights;

static class SettingExtensions
{
    public static string LowerName(this Setting setting) => setting.Name.ToLowerInvariant();

    public static bool IsEmpty(this Setting setting) => string.IsNullOrWhiteSpace(setting.Value);
}
