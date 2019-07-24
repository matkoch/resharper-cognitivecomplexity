using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

namespace ReSharperPlugin.CognitiveComplexity.Tests
{
  [ZoneDefinition]
  public interface ICognitiveComplexityTestZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>
  {
  }

  [SetUpFixture]
  public class TestEnvironment : ExtensionTestEnvironmentAssembly<ICognitiveComplexityTestZone>
  {
  }
}
