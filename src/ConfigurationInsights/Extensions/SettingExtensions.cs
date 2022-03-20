using ConfigurationInsights;

static class SettingExtensions
{
    public static bool IsEmpty(this Setting setting) => string.IsNullOrWhiteSpace(setting.Value);
}
