using CommandLine;

class Options
{
    [Value(0, HelpText = "Path name of the file with configuration to check")]
    public string FilePath { get; set; }

    [Option('v', "verbose", HelpText = "Shows verbose messaging")]
    public bool Verbose { get; set; }
}
