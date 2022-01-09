using ConfigurationInsights;

static class SettingExtensions
{
    public static string LowerName(this Setting setting) => setting.Name.ToLowerInvariant();
}
