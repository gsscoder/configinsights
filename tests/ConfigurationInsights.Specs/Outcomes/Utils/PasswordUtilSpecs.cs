using Xunit;

namespace Outcomes.Utils;

public class PasswordUtilSpecs
{
    [Theory]
    [InlineData("abcd")]
    [InlineData("abc12")]
    public void Detect_very_weak_password(string value)
    {
        var strength = PasswordUtil.Measure(value);

        Assert.Equal(PasswordStrength.VeryWeak, strength);
    }

    [Theory]
    [InlineData("abc123")]
    [InlineData("Abcd1234")]
    public void Detect_weak_password(string value)
    {
        var strength = PasswordUtil.Measure(value);

        Assert.Equal(PasswordStrength.Weak, strength);
    }

    [Theory]
    [InlineData("AbCdEf123")]
    public void Detect_good_password(string value)
    {
        var strength = PasswordUtil.Measure(value);

        Assert.Equal(PasswordStrength.Good, strength);
    }

    [Theory]
    [InlineData("AbCdEfghi1234567")]
    [InlineData("AbCdEfghi1234567_.")]
    public void Detect_strong_password(string value)
    {
        var strength = PasswordUtil.Measure(value);

        Assert.Equal(PasswordStrength.Strong, strength);
    }

    [Theory]
    [InlineData("AbCdEfghi1234567_.@")]
    public void Detect_very_strong_password(string value)
    {
        var strength = PasswordUtil.Measure(value);

        Assert.Equal(PasswordStrength.VeryStrong, strength);
    }
}
