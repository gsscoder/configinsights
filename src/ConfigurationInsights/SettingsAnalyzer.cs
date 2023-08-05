using System.Collections.Generic;
using System.Linq;
using ConfigurationInsights.Analyzers;
using Microsoft.Extensions.Logging;
using SharpX;
using SharpX.Extensions;

namespace ConfigurationInsights;

public class SettingsAnalyzer
{
    readonly IEnumerable<IAnalyzer> _analyzers;
    readonly ILogger _logger;
    readonly bool _canLogOk;

    public SettingsAnalyzer(AnalyzerOptions options, IEnumerable<IAnalyzer> analyzers)
    {
        Guard.DisallowNull(nameof(options), options);
        Guard.DisallowNull(nameof(options.Logger), options.Logger);

        _logger = options.Logger;
        _canLogOk = options.EnableOkLogging;
        _analyzers = Enumerable.Empty<IAnalyzer>()
            .Concat(new IAnalyzer[] {
                new DefaultAnalyzer(options),
                new DataTypeAnalyzer(options),
                new ConnectionStringAnalyzer(options)
            })
            .Concat(analyzers);

        _logger.LogInformation("Initiating settings analysis");
    }

    public SettingsAnalyzer(AnalyzerOptions options)
        : this(options, Enumerable.Empty<IAnalyzer>())
    {
    }

    public IEnumerable<SettingOutcome> Analyze(IEnumerable<Setting> settings)
    {
        Guard.DisallowNull(nameof(settings), settings);

        var result = new List<SettingOutcome>();

        foreach (var setting in settings.OrderBy(x => x.Name)) {
            var outcomes = new List<Outcome>(capacity:_analyzers.Count() * 3);
            foreach (var analyzer in _analyzers)
                if (analyzer.CanAnalyze(setting))
                    outcomes.AddRange(analyzer.Analyze(setting));
            // No outcomes, setting is assumed to be OK
            if (!outcomes.Any()) {
                var message = $"{setting.Name.Quote()} is OK";
                outcomes.Add(new Outcome(OutcomeType.Ok, message));
                if (_canLogOk) _logger.LogInformation(message);
            }
            result.Add(new SettingOutcome(setting, outcomes));
        }

        return result;
    }
}
