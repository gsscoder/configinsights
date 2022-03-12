// Based on: https://stackoverflow.com/questions/12899876/checking-strings-for-a-strong-enough-password
using System.Linq;
using SharpX;

enum PasswordStrength
{
    VeryWeak = 1,
    Weak,
    Good,
    Strong,
    VeryStrong
}

static class PasswordUtil
{
    public static PasswordStrength Measure(string password)
    {
        if (password.Length <= 4)
            return PasswordStrength.VeryWeak;
        
        var rating = 0;

        if (password.Length >= 16)
            rating++;
        if (password.Count(char.IsDigit) > 2)
            rating++;
        if (password.Count(char.IsLower) > 2)
            rating++;
        if (password.Count(char.IsUpper) > 2)
            rating++;
        if (password.Count(Strings.IsSpecialChar) > 2)
            rating++;

        if (rating > 5)
            rating = 5;
        
        return (PasswordStrength)rating;
    }
}
