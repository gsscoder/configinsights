using System.Linq;
using System.Threading.Tasks;
using ConfigurationInsights;
using ConfigurationInsights.Analyzers;
using Microsoft.Extensions.Logging;
using SharpX;
using Xunit;
using Outcome = ConfigurationInsights.Outcome;
using OutcomeType = ConfigurationInsights.OutcomeType;

namespace Outcomes.Analyzers;

public class DefaultAnalyzerSpecs
{
    [Fact]
    public void Detect_special_characters_in_name()
    {
        var options = Defaults.GetAnalyzerOptions();
        var sut = new DefaultAnalyzer(options);
        var settingName = Strings.Generate(20, new GenerateOptions { AllowSpecialChars = true });

        var outcomes = sut.Analyze(new Setting(settingName, "value", Defaults.GetEmptyMetadata()));

        Assert.Collection(outcomes, outcome =>
        {
            Assert.Equal(
                new Outcome(OutcomeType.Warning, $"{settingName.Quote()} contains special characters or whitespace")
                {
                    MessageHint = "Avoid special characters and whitespace in names except '_' and '-'"
                },
                outcome);
        });

        var logger = (CaptureLogger)options.Logger;

        Assert.Equal(1, logger.Messages.Keys.Count(k => k == LogLevel.Information));
        Assert.Equal(1, logger.Messages.Keys.Count(k => k == LogLevel.Warning));
    }
}
