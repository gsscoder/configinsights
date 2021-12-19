using System.Collections.Generic;
using SharpX;

namespace ConfigurationInsights
{
    public class SettingOutcome
    {
        public Setting Setting { get; private set; }
        public IEnumerable<Outcome> Outcomes { get; private set; }

        public SettingOutcome(Setting setting, IEnumerable<Outcome> outcomes)
        {
            Setting = setting;
            Outcomes = outcomes;
        }
    }
}
