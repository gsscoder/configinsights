using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ConfigurationInsights.Analyzers
{
    class DefaultAnalyzer : Analyzer
    {
        public override string Name { get => "Default"; }

        public DefaultAnalyzer(AnalyzerOptions options) : base(options)
        {
        }

        public override bool CanAnalyze(Setting setting) => true;

        public override IEnumerable<Outcome> Analyze(Setting setting)
        {
            var outcomes = new List<Outcome>();

            var quotedName = setting.Name.Quote();
            if (string.IsNullOrWhiteSpace(setting.Value)) {
                var message = $"{quotedName} is empty";
                outcomes.Add(new Outcome(OutcomeType.Warning, message));
                Logger.LogWarning(message);
            }

            return outcomes;
        }
    }
}
