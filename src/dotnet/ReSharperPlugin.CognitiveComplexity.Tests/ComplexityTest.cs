using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using JetBrains.ReSharper.Psi;
using NUnit.Framework;

namespace ReSharperPlugin.CognitiveComplexity.Tests
{
    public class ComplexityTest : CSharpHighlightingTestBase
    {
        protected override string RelativeTestDataPath => "CSharp";

        protected override bool HighlightingPredicate(
            IHighlighting highlighting,
            IPsiSourceFile sourceFile,
            IContextBoundSettingsStore settingsStore)
        {
            return highlighting is CognitiveComplexityErrorHighlighting ||
                   highlighting is CognitiveComplexityInfoHighlighting;
        }

        [Test] public void TestConditionTest() { DoNamedTest2(); }
        [Test] public void TestNullCheckingTest() { DoNamedTest2(); }
        [Test] public void TestLoopingTest() { DoNamedTest2(); }
        [Test] public void TestLogicalOperatorTest() { DoNamedTest2(); }
        [Test] public void TestSwitchTest() { DoNamedTest2(); }
        [Test] public void TestRecursiveTest() { DoNamedTest2(); }
        [Test] public void TestTryCatchTest() { DoNamedTest2(); }
        [Test] public void TestLambdaTest() { DoNamedTest2(); }
    }
}