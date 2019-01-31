using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;

namespace ReSharperPlugin.CognitiveComplexity
{
    [ZoneDefinition]
    [ZoneDefinitionConfigurableFeature("Title", "Description", IsInProductSection: false)]
    public interface ICognitiveComplexityZone : IPsiLanguageZone,
        IRequire<ILanguageCSharpZone>,
        IRequire<DaemonZone>
    {
    }
}