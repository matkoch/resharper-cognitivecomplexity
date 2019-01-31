using JetBrains.Application.BuildScript.Application.Zones;

namespace ReSharperPlugin.CognitiveComplexity
{
    [ZoneMarker]
    public class ZoneMarker : IRequire<ICognitiveComplexityZone>
    {
    }
}