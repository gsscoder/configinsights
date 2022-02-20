using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SharpX;

namespace ConfigurationInsights.Analyzers
{
    public class DataTypeAnalyzer : Analyzer
    {
        enum MatchType { Equals, Contains }
        sealed class Check
        {
            public string Name { get; set; }
            public MatchType NameMatch { get; set;}
            public Type ValueType { get; set;}
        }

        Maybe<Check> _check;
        readonly IEnumerable<Check> _checks = new Check[]
            {
                new Check { Name = "appinsights_instrumentationkey", NameMatch = MatchType.Equals, ValueType = typeof(Guid) },
                new Check { Name = "tenantid", NameMatch = MatchType.Contains, ValueType = typeof(Guid) },
                new Check { Name = "tenant_id", NameMatch = MatchType.Contains, ValueType = typeof(Guid) },
                new Check { Name = "applicationid", NameMatch = MatchType.Contains, ValueType = typeof(Guid) },
                new Check { Name = "application_id", NameMatch = MatchType.Contains, ValueType = typeof(Guid) },
                new Check { Name = "clientid", NameMatch = MatchType.Contains, ValueType = typeof(Guid) },
                new Check { Name = "client_id", NameMatch = MatchType.Contains, ValueType = typeof(Guid) },
                new Check { Name = "appclientid", NameMatch = MatchType.Contains, ValueType = typeof(Guid) },
                new Check { Name = "appclient_id", NameMatch = MatchType.Contains, ValueType = typeof(Guid) },
            };
        
        public override string Name => "Connection string analyzer";

        public DataTypeAnalyzer(AnalyzerOptions options) : base(options)
        {
        }

        public override bool CanAnalyze(Setting setting)
        {
            _check = _checks.SingleOrNothing(
                c => c.NameMatch == MatchType.Equals && setting.LowerName() == c.Name);
            switch (_check.Tag) {
                default:
                    Options.Logger.LogTrace("Known name found");
                    return true;
                case MaybeType.Nothing:
                    _check = _checks.SingleOrNothing(
                        c => c.NameMatch == MatchType.Contains && setting.LowerName().Contains(c.Name));
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
            bool valid = true;
            try {
                _ = Convert.ChangeType(setting.Value, _check.FromJust().ValueType);
            }
            catch (InvalidCastException) {
                valid = false;
            }
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
