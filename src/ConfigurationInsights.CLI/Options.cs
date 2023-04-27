using CommandLine;

enum OutputType : byte
{
    Default,
    Json,
    JsonObfuscated
}

class Options
{
    [Value(0, HelpText = "Path name of the file with configuration to check")]
    public string FilePath { get; set; }

    [Option('v', "verbose", HelpText = "Shows verbose messaging")]
    public bool Verbose { get; set; }

    [Option('o', "output", Default = OutputType.Default, HelpText = "Render analysis result as JSON (Clear, Obfuscated)")]
    public OutputType OutputType { get; set; }
}
