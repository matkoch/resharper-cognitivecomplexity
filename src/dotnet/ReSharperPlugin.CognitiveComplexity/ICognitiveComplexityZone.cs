using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
#if RIDER
using JetBrains.Rider.Backend.Env;
#endif

namespace ReSharperPlugin.CognitiveComplexity
{
    [ZoneDefinition]
    public interface ICognitiveComplexityZone : IPsiLanguageZone,
        IRequire<ILanguageCSharpZone>,
        #if RIDER
        IRequire<IRiderPlatformZone>,
        #endif
        IRequire<DaemonZone>
    {
    }
}