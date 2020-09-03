using System.Threading;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: Apartment(ApartmentState.STA)]

namespace ReSharperPlugin.CognitiveComplexity.Tests
{

    [ZoneDefinition]
    public class CognitiveComplexityTestEnvironmentZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>, IRequire<ICognitiveComplexityZone> { }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>, IRequire<CognitiveComplexityTestEnvironmentZone> { }
    
    [SetUpFixture]
    public class CognitiveComplexityTestsAssembly : ExtensionTestEnvironmentAssembly<CognitiveComplexityTestEnvironmentZone> { }
}