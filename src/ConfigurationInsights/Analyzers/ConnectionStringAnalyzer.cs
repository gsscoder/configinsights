using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.Extensions.Logging;
using SharpX;
using SharpX.Extensions;

namespace ConfigurationInsights.Analyzers;

public class ConnectionStringAnalyzer : Analyzer
{
    bool _known;
    readonly ILogger _logger;

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

    IEnumerable<string> _knownPwdParamNames = new[] {
        "password",
        "key"
    };

    public override string Name => "Connection string analyzer";

    public ConnectionStringAnalyzer(AnalyzerOptions options) : base(options)
    {
        _logger = options.Logger;
    }

    public override bool CanAnalyze(Setting setting)
    {
        if (_knownPrefixes.Any(n => setting.Name.StartsWithIgnoreCase(n))) {
            _logger.LogTrace("Known prefix found");
            _known = true;
            return true;
        }
        if (_knownNames.Any(n => setting.Name.EqualsIgnoreCase(n))) {
            _logger.LogTrace("Known name found");
            _known = true;
            return true;
        }
        if (setting.Name.ContainsIgnoreCase("connection") &&
            setting.Name.ContainsIgnoreCase("string")) {
            _logger.LogTrace("Possile connection string found");
            return true;
        }
        return false;
    }

    public override IEnumerable<Outcome> Analyze(Setting setting)
    {
        DbConnectionStringBuilder connStr = null;
        try {
            connStr = new DbConnectionStringBuilder {
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
        if (FindParameter().MatchJust(out var paramName)) {
            _logger.LogTrace($"Found password parameter: {paramName}");
            var password = (string)connStr[paramName];
            var strength = PasswordUtil.Measure(password);
            var outcome = strength switch {
                var s when
                    s == PasswordStrength.VeryWeak ||
                    s == PasswordStrength.Weak => new Outcome(
                        OutcomeType.Error, message: $"{setting.Name.Quote()} {password} parameter contains a weak password"),
                _ => new Outcome(
                    OutcomeType.Error, message: $"{setting.Name.Quote()} {password} parameter password strength should be improved")
            };
            return new[] { outcome.Log(Options) };
        }
        return Enumerable.Empty<Outcome>();

        Maybe<string> FindParameter()
        {
            foreach (string key in connStr.Keys) {
                foreach (var paramName in _knownPwdParamNames) {
                    if (key.StartsWithIgnoreCase(paramName))
                        return key.ToJust();
                }
            }
            return Maybe.Nothing<string>();
        }
    }
}
