using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

static class IOUtil
{
    public static IEnumerable<string> ReadStandardInput()
    {
        using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
        var lines = reader.ReadToEnd().Split(Environment.NewLine);
        return lines.AsEnumerable();
    }
}
