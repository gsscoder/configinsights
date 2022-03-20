using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.Extensions.Logging;
using SharpX.Extensions;

namespace ConfigurationInsights.Analyzers
{
    public class ConnectionStringAnalyzer : Analyzer
    {
        bool _known;

        IEnumerable<string> _knownPrefixes = new[] {
            "SQLCONNSTR_",
            "MYSQLCONNSTR_",
            "SQLAZURECONNSTR_",
            "CUSTOMCONNSTR_",
            "POSTGRESQLCONNSTR_"
        };

        IEnumerable<string> _knownNames = new[] {
            "APPLICATIONINSIGHTS_CONNECTION_STRING",
            "AzureWebJobsDashboard",
            "AzureWebJobsStorage",
            "WEBSITE_CONTENTAZUREFILECONNECTIONSTRING"
        };

        public override string Name => "Connection string analyzer";

        public ConnectionStringAnalyzer(AnalyzerOptions options) : base(options)
        {
        }

        public override bool CanAnalyze(Setting setting)
        {
            if (_knownPrefixes.Any(n => setting.Name.StartsWithIgnoreCase(n))) {
                Options.Logger.LogTrace("Known prefix found");
                _known = true;
                return true;
            }
            if (_knownNames.Any(n => setting.Name.EqualsIgnoreCase(n))) {
                Options.Logger.LogTrace("Known name found");
                _known = true;
                return true;
            }
            if (setting.Name.ContainsIgnoreCase("connection") &&
                setting.Name.ContainsIgnoreCase("string")) {
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
