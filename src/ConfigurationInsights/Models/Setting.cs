using System.Collections.Generic;

namespace ConfigurationInsights
{
    public class Setting
    {
        public string Name { get; private set; }

        public string Value { get; private set; }

        public IDictionary<string, string> Metadata { get; private set; }

        public Setting(string name, string value, IDictionary<string, string> metadata)
        {
            Name = name;
            Value = value;
            Metadata = metadata;
        }
    }
}
