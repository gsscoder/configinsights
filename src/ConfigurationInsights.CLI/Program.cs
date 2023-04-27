using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using ConfigurationInsights;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharpX;
using SharpX.Extensions;

class ExitCodes 
{
    public const int ExitOk = 0;
    public const int ExitFail = 1;
}

class Program
{
    readonly Options _options;
    ILogger _logger;

    public Program(Options options)
    {
        _options = options;
        _logger = new Logger(options.Verbose);
    }

    int Run()
    {
        var inputResult = ReadInput();
        if (!inputResult.MatchRight(out var lines))
            return Fail(inputResult.FromLeft());
        if (!lines.Any()) {
            _logger.LogInformation("Input file is empty. Nothing left to do");
            return ExitCodes.ExitOk;
        }

        var settings = Enumerable.Empty<Setting>();
        var firstChar = lines.First().TrimStart()[0];
        if (firstChar == '[') {
            var parser = new JsonMapper(_logger);
            if (!parser.MapArray(string.Join(Environment.NewLine, lines)).MatchJust(out settings))
                Fail("Unable to parse JSON input array");
        }
        else if (firstChar == '{')
            Fail("JSON objects are not actually supported");
        else
            Fail("This file type is not recognized or supported");

        if (!settings.Any())
            _logger.LogWarning("No settings to analyze. Nothing left to do");
        if (_options.OutputType != OutputType.Default) {
            _logger = NullLogger.Instance;
        }
        var analyzer = new SettingsAnalyzer(
            new AnalyzerOptions {
                Logger = _logger,
                EnableOkLogging = true,
                EnableHintLogging = true });
        var result = analyzer.Analyze(settings);

        if (_options.OutputType != OutputType.Default) {
            var json = new ResultFormatter().FormatToJson(result, _options.OutputType == OutputType.JsonObfuscated);
            Console.WriteLine(json);
        }

        var hasError = result.SelectMany(x => x.Outcomes).Any(x => x.HasError());

        return hasError
            ? Fail("Analysis completed with errors")
            : ExitCodes.ExitOk;
    }

    static int Fail()
    {
        return ExitCodes.ExitFail;
    }
    
    int Fail(string message)
    {
        _logger.LogError(message);
        return ExitCodes.ExitFail;
    }

    static int Main(string[] args)
    {        
        var parser = new Parser(settings => {
            settings.CaseInsensitiveEnumValues = true;
            settings.HelpWriter = Console.Error;
        });

        return parser.ParseArguments<Options>(args).MapResult(
            options => new Program(options).Run(), _ => Fail());
    }

    Either<string, IEnumerable<string>> ReadInput()
    {
        if (_options.FilePath == null && !Console.IsInputRedirected)
            return Either.Left<string, IEnumerable<string>>("No file path or input redirected specified");
        if (_options.FilePath != null && Console.IsInputRedirected)
            return Either.Left<string, IEnumerable<string>>("Both file path and input redirected specified");
        IEnumerable<string> lines;
        try {
            if (Console.IsInputRedirected) {
                _logger.LogInformation("Reading from standard input");
                lines = IOUtil.ReadStandardInput();
            }
            else {
                _logger.LogInformation($"Reading from file {_options.FilePath}");
                lines = File.ReadAllLines(_options.FilePath);
            }
        }
        catch (IOException e){
            return Either.Left<string, IEnumerable<string>>(
                $"Unable to read input file:{Environment.NewLine}{e.Format()}");
        }
        return Either.Right<string, IEnumerable<string>>(lines);
    }
}
