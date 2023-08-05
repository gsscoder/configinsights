using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SharpX;

namespace ConfigurationInsights;

public class ResultFormatter
{
    public string FormatToJson(IEnumerable<SettingOutcome> outcomes, bool obfuscated = true)
    {
        Guard.DisallowNull(nameof(outcomes), outcomes);
        
        if (!obfuscated)
            return JsonConvert.SerializeObject(outcomes, Formatting.Indented);

        var outcomes_= outcomes.Select(x =>new SettingOutcome(x.Setting.Obfuscate(), x.Outcomes));

        return JsonConvert.SerializeObject(outcomes_, Formatting.Indented);
    }
}
