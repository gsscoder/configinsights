using System.Collections.Generic;

namespace ConfigurationInsights.Analyzers;

public interface IAnalyzer
{
    public string Name { get;  }

    public abstract bool CanAnalyze(Setting setting);

    public IEnumerable<Outcome> Analyze(Setting setting);
}
