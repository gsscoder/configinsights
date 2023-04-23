using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SharpX;
using SharpX.Extensions;

namespace ConfigurationInsights.Analyzers
{
    public class DataTypeAnalyzer : Analyzer
    {
        enum MatchType { Equals, Contains }
        sealed class Check
        {
            public string Name { get; set; }
            public MatchType NameMatch { get; set;}
            public Func<string, bool> Converter { get; set;}
        }

        Maybe<Check> _check;
        readonly IEnumerable<Check> _checks = new Check[]
            {
                new Check { Name = "appinsights_instrumentationkey", NameMatch = MatchType.Equals, Converter = x => Guid.TryParse(x, out _) },
                new Check { Name = "tenantid", NameMatch = MatchType.Contains, Converter = x => Guid.TryParse(x, out _) },
                new Check { Name = "tenant_id", NameMatch = MatchType.Contains, Converter = x => Guid.TryParse(x, out _) },
                new Check { Name = "applicationid", NameMatch = MatchType.Contains, Converter = x => Guid.TryParse(x, out _) },
                new Check { Name = "application_id", NameMatch = MatchType.Contains, Converter = x => Guid.TryParse(x, out _) },
                new Check { Name = "clientid", NameMatch = MatchType.Contains, Converter = x => Guid.TryParse(x, out _) },
                new Check { Name = "client_id", NameMatch = MatchType.Contains, Converter = x => Guid.TryParse(x, out _) },
                new Check { Name = "appclientid", NameMatch = MatchType.Contains, Converter = x => Guid.TryParse(x, out _) },
                new Check { Name = "appclient_id", NameMatch = MatchType.Contains, Converter = x => Guid.TryParse(x, out _) },
            };
        
        public override string Name => "Connection string analyzer";

        public DataTypeAnalyzer(AnalyzerOptions options) : base(options)
        {
        }

        public override bool CanAnalyze(Setting setting)
        {
            _check = _checks.SingleOrNothing(
                c => c.NameMatch == MatchType.Equals && setting.Name.ContainsIgnoreCase(c.Name));
            switch (_check.Tag) {
                default:
                    Options.Logger.LogTrace("Known name found");
                    return true;
                case MaybeType.Nothing:
                    _check = _checks.SingleOrNothing(
                        c => c.NameMatch == MatchType.Contains && setting.Name.ContainsIgnoreCase(c.Name));
                    if (_check.IsJust()) {
                        Options.Logger.LogTrace("Possible name with semantic match found");
                        return true;
                    }
                    break;
            };
            return false;
        }

        public override IEnumerable<Outcome> Analyze(Setting setting)
        {
            var valid = _check.FromJust().Converter(setting.Value);
            if (!valid) {
                var outcome = _check.FromJust().NameMatch switch {
                    MatchType.Equals => new Outcome(
                        OutcomeType.Error, message: $"{setting.Name.Quote()} contains an invalid value"),
                    _ => new Outcome(
                        OutcomeType.Warning, message: $"{setting.Name.Quote()} may contain an invalid value")
                };
                return new[] { outcome.Log(Options) };
            }
            return Enumerable.Empty<Outcome>();
        }
    }
}
