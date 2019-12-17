using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
#if RIDER
using JetBrains.ReSharper.Daemon.CodeInsights;
using JetBrains.ReSharper.Host.Platform.Icons;
using JetBrains.UI.Icons;
using ReSharperPlugin.CognitiveComplexity.Rider;

#endif

namespace ReSharperPlugin.CognitiveComplexity
{
    // Types mentioned in this attribute are used for performance optimizations
    [ElementProblemAnalyzer(
        typeof(ICSharpFunctionDeclaration),
        HighlightingTypes = new[]
        {
            typeof(CognitiveComplexityHighlighting),
#if RIDER
            typeof(CognitiveComplexityInfoHint),
            typeof(CognitiveComplexityWarningHint),
            typeof(CognitiveComplexityErrorHint)
#endif
        })]
    public class CognitiveComplexityProblemAnalyzer : ElementProblemAnalyzer<ICSharpFunctionDeclaration>
    {
#if RIDER
        private readonly CognitiveComplexityCodeInsightsProvider _codeInsightsProvider;
        private readonly IconHost _iconHost;

        public CognitiveComplexityProblemAnalyzer(
            CognitiveComplexityCodeInsightsProvider codeInsightsProvider,
            IconHost iconHost)
        {
            _codeInsightsProvider = codeInsightsProvider;
            _iconHost = iconHost;
        }
#endif

        protected override void Run(
            ICSharpFunctionDeclaration element,
            ElementProblemAnalyzerData data,
            IHighlightingConsumer consumer)
        {
            if (element.Body == null)
                return;

            var elementProcessor = new CognitiveComplexityElementProcessor(element);
            element.Body.ProcessDescendants(elementProcessor);

            var store = data.SettingsStore;
            var baseThreshold = store.GetValue((CognitiveComplexityAnalysisSettings s) => s.CSharpThreshold);
            var complexityPercentage = (int) (elementProcessor.ComplexityScore * 100.0 / baseThreshold);
            if (complexityPercentage > 100)
                consumer.AddHighlighting(new CognitiveComplexityHighlighting(element, complexityPercentage));

#if RIDER
            var lowThreshold = store.GetValue((CognitiveComplexityAnalysisSettings s) => s.LowComplexityThreshold);
            var middleThreshold =
                store.GetValue((CognitiveComplexityAnalysisSettings s) => s.MiddleComplexityThreshold);
            var highThreshold = store.GetValue((CognitiveComplexityAnalysisSettings s) => s.HighComplexityThreshold);

            string codeLensText;
            IconId iconId;

            if (complexityPercentage >= highThreshold)
            {
                iconId = ComplexityExtreme.Id;
                codeLensText = complexityPercentage >= highThreshold * 2
                    ? $"refactor me? ({complexityPercentage}%)"
                    : $"very complex ({complexityPercentage}%)";
            }
            else if (complexityPercentage >= middleThreshold)
            {
                iconId = ComplexityHigh.Id;
                codeLensText = $"mildly complex ({complexityPercentage}%)";
            }
            else if (complexityPercentage >= lowThreshold)
            {
                iconId = ComplexityAverage.Id;
                codeLensText = $"simple enough ({complexityPercentage}%)";
            }
            else
            {
                iconId = ComplexityLow.Id;
                codeLensText = string.Empty;
            }

            var moreText =
                $"Cognitive complexity value of {elementProcessor.ComplexityScore} " +
                $"({complexityPercentage}% of threshold {baseThreshold})";
            consumer.AddHighlighting(
                new CodeInsightsHighlighting(
                    element.GetNameDocumentRange(),
                    codeLensText,
                    moreText,
                    moreText,
                    _codeInsightsProvider,
                    element.DeclaredElement,
                    _iconHost.Transform(iconId))
            );

            if (elementProcessor.Complexities != null)
            {
                var min = int.MaxValue;
                var max = int.MinValue;
                foreach (var (_, _, complexity) in elementProcessor.Complexities)
                {
                    min = Math.Min(min, complexity);
                    max = Math.Max(max, complexity);
                }

                foreach (var (node, offset, complexity) in elementProcessor.Complexities)
                {
                    if (complexityPercentage >= highThreshold && complexity == max)
                        consumer.AddHighlighting(new CognitiveComplexityErrorHint(node, offset, complexity));
                    else if (complexityPercentage >= middleThreshold && complexity >= min + (max - min) / 2)
                        consumer.AddHighlighting(new CognitiveComplexityWarningHint(node, offset, complexity));
                    else
                        consumer.AddHighlighting(new CognitiveComplexityInfoHint(node, offset, complexity));
                }
            }
#endif
        }
    }
}