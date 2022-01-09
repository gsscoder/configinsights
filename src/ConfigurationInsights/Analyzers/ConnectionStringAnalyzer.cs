using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ConfigurationInsights.Analyzers
{
    public class ConnectionStringAnalyzer : Analyzer
    {
        bool _known;

        string[] _knownNames = new[] {
            "applicationinsights_connection_string",
            "azurewebjobsdashboard",
            "azurewebjobsstorage",
            "website_contentazurefileconnectionstring"
        };

        public override string Name => "Connection string analyzer";

        public ConnectionStringAnalyzer(AnalyzerOptions options) : base(options)
        {
        }

        public override bool CanAnalyze(Setting setting)
        {
            if (_knownNames.Contains(setting.LowerName())) {
                Options.Logger.LogTrace("Known name found");
                _known = true;
                return true;
            }
            if (setting.LowerName().Contains("connection") &&
                setting.LowerName().Contains("string")) {
                Options.Logger.LogTrace("Possile connection string found");
                return true;
            }
            return false;
        }

        public override IEnumerable<Outcome> Analyze(Setting setting)
        {
            try {
                var connStr = new DbConnectionStringBuilder {
                    ConnectionString = setting.Value
                };
            }
            catch {
                if (_known) {
                    var outcome = new Outcome(
                        OutcomeType.Error, message: $"{setting.Name.Quote()} contains an invalid connection string");
                    return new[] { outcome.Log(Options) };
                }
            }
            return Enumerable.Empty<Outcome>();
        }
    }
}
