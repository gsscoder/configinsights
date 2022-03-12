using FluentAssertions;
using Xunit;

namespace Outcomes
{
    public class PasswordUtilSpecs
    {
        [Theory]
        [InlineData("abcd")]
        [InlineData("abc12")]
        public void Detect_very_weak_password(string value)
        {
            var strength = PasswordUtil.Measure(value);

            strength.Should().Be(PasswordStrength.VeryWeak);
        }

        [Theory]
        [InlineData("abc123")]
        [InlineData("Abcd1234")]
        public void Detect_weak_password(string value)
        {
            var strength = PasswordUtil.Measure(value);

            strength.Should().Be(PasswordStrength.Weak);
        }

        [Theory]
        [InlineData("AbCdEf123")]
        public void Detect_good_password(string value)
        {
            var strength = PasswordUtil.Measure(value);

            strength.Should().Be(PasswordStrength.Good);
        }

        [Theory]
        [InlineData("AbCdEfghi1234567")]
        [InlineData("AbCdEfghi1234567_.")]
        public void Detect_strong_password(string value)
        {
            var strength = PasswordUtil.Measure(value);

            strength.Should().Be(PasswordStrength.Strong);
        }

        [Theory]
        [InlineData("AbCdEfghi1234567_.@")]
        public void Detect_very_strong_password(string value)
        {
            var strength = PasswordUtil.Measure(value);

            strength.Should().Be(PasswordStrength.VeryStrong);
        }
    }
}
