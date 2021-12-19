using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpX;
using SharpX.Extensions;

namespace ConfigurationInsights
{
    public class JsonMapper
    {
        readonly ILogger _logger;

        public JsonMapper(ILogger logger)
        {
            _logger = logger;
        }

        public Maybe<IEnumerable<Setting>> MapArray(string json)
        {
            var settings = new List<Setting>(capacity: 32);
            JArray array;
            try {
                array = JArray.Parse(json);
                _logger.LogTrace("JSON array parsed");
                foreach (var item in array) {
                    if (item.Type != JTokenType.Object) continue;
                    var obj = (JObject)item;
                    if (!obj.TryGetValue("name", StringComparison.OrdinalIgnoreCase, out var name)) {
                        _logger.LogTrace("'name' property to found");
                        continue;
                    }
                    if (!obj.TryGetValue("value", StringComparison.OrdinalIgnoreCase, out var value)) {
                        _logger.LogTrace("'value' property to found");
                        continue;
                    }
                    var metadata = new Dictionary<string, string>();
                    foreach (var c in obj.Children()) {
                        if (c.Type != JTokenType.Property) continue;
                        var prop = (JProperty)c;
                        if (prop.Name == "name" || prop.Name == "value") continue;
                        metadata.Add(prop.Name, prop.Value.ToString());
                    }
                    settings.Add(new Setting(name.ToString(), value.ToString(), metadata));
                }
            }
            catch (JsonReaderException e) {
                _logger.LogCritical(
                    $"JSON parsing error occurred:{Environment.NewLine}{e.Format()}");
            }
            return settings.AsEnumerable().ToJust();
        }
    }
}
