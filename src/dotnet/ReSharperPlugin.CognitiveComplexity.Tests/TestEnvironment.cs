using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: RequiresSTA]

namespace ReSharperPlugin.CognitiveComplexity.Tests
{
  [ZoneDefinition]
  public interface ICognitiveComplexityTestZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>, IRequire<ICognitiveComplexityZone>
  {
  }

  [SetUpFixture]
  public class TestEnvironment : ExtensionTestEnvironmentAssembly<ICognitiveComplexityTestZone>
  {
  }
}
