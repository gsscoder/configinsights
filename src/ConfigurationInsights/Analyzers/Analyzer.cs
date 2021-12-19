using System.Collections.Generic;
using SharpX;

namespace ConfigurationInsights.Analyzers
{
    public abstract class Analyzer : IAnalyzer
    {
        protected AnalyzerOptions Options { get; private set; }


        public Analyzer(AnalyzerOptions options)
        {
            Guard.DisallowNull(nameof(options), options);
            Guard.DisallowNull(nameof(options.Logger), options.Logger);

            Options = options;
        }

        public abstract string Name { get; }

        public abstract bool CanAnalyze(Setting setting);

        public abstract IEnumerable<Outcome> Analyze(Setting setting);
    }
}
