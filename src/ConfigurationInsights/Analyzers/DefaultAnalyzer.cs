using System.Collections.Generic;
using SharpX;

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

            if (Strings.ContainsSpecialChar(setting.Name, excluded: new[] { '_', '-' }) ||
                Strings.ContainsWhitespace(setting.Name)) {
                outcomes.Add(new Outcome(OutcomeType.Warning,
                    message: $"{quotedName} contains not allowed special characters or whitespace")
                {
                    MessageHint = "Aavoid special characters and whitespace in names except '_' and '-'"
                }
                    .Log(Options));
            }

            if (string.IsNullOrWhiteSpace(setting.Value)) {
                outcomes.Add(
                    new Outcome(OutcomeType.Warning, message: $"{quotedName} is empty")
                    .Log(Options));
            }
            
            return outcomes;
        }
    }
}
