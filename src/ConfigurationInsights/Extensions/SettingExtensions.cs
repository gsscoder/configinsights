using System;
using System.Security.Cryptography;
using ConfigurationInsights;

public static class SettingExtensions
{
    public static bool IsEmpty(this Setting setting) => string.IsNullOrWhiteSpace(setting.Value);

    public static Setting Obfuscate(this Setting setting)
    {
        if (setting.IsEmpty()) return setting;

        string obfuscated;
        if (setting.Value.Length <= 4) {
            obfuscated = new String('*', RandomNumberGenerator.GetInt32(1, 6));
        } else {
            obfuscated = setting.Value.Substring(0, 3) +
                         new String('*', RandomNumberGenerator.GetInt32(3, 12));
        }
        var setting_ = new Setting(setting.Name, obfuscated, setting.Metadata);
        return setting_;
    }
}
