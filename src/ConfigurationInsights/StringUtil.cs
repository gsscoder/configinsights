static class StringUtil
{
    public static string Quote(this string str)
    {
        if (str.Contains(' '))
            return str.Contains("'") || str.Contains('"') ? $"[{str}]" : $"'{str}'";
        return str;
    }
}
