using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SharpX;

namespace ConfigurationInsights.Analyzers
{
    public abstract class Analyzer : IAnalyzer
    {
        protected ILogger Logger { get; private set; }
        protected bool CanLogOk { get; private set; }
        protected bool CanLogHint { get; private set; }

        public Analyzer(AnalyzerOptions options)
        {
            Guard.DisallowNull(nameof(options), options);
            Guard.DisallowNull(nameof(options.Logger), options.Logger);

            Logger = options.Logger;
            CanLogOk = options.EnableOkLogging;
            CanLogHint = options.EnableHintLogging;
        }

        public abstract string Name { get; }

        public abstract bool CanAnalyze(Setting setting);

        public abstract IEnumerable<Outcome> Analyze(Setting setting);
    }
}
