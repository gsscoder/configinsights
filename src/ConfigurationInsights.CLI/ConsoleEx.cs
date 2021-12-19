using System;

static class ConsoleEx
{
    enum StreamKind { Out, Error }

    public static void Write(ConsoleColor color, string format, params object[] arg) =>
        Write(StreamKind.Out, color, format, arg);

    public static void WriteError(ConsoleColor color, string format, params object[] arg) =>
        Write(StreamKind.Error, color, format, arg);

    public static void WriteLine(ConsoleColor color, string format, params object[] arg) =>
        Write(color, $"{format}{Environment.NewLine}", arg);

    public static void WriteErrorLine(ConsoleColor color, string format, params object[] arg) =>
        WriteError(color, $"{format}{Environment.NewLine}", arg);

    public static void WriteBold(string format, params object[] arg) =>
        Console.Write($"\x1b[1m{format}\x1b[0m", arg);

    public static void WriteBoldLine(string format, params object[] arg) =>
       WriteBold($"{format}{Environment.NewLine}", arg);

    static void Write(StreamKind kind, ConsoleColor color, string format, params object[] arg)
    {
        var current = Console.ForegroundColor;
        Console.ForegroundColor = color;
        var writer = kind == StreamKind.Out ? Console.Out : Console.Error;
        writer.Write(format, arg);
        Console.ForegroundColor = current;
    }
}
