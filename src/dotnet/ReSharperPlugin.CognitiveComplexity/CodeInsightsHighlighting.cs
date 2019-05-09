using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CodeInsights;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Host.Platform.Icons;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Rider.Model;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.UI.Icons;
using Severity = JetBrains.ReSharper.Feature.Services.Daemon.Severity;

//[assembly: RegisterHighlighter(
//    CodeInsightsHighlighting.HighlightAttributeId,
//    EffectType = EffectType.NONE,
//    TransmitUpdates = true,
//    Layer = HighlighterLayer.SYNTAX + 1,
//    GroupId = HighlighterGroupIds.HIDDEN)]

namespace ReSharperPlugin.CognitiveComplexity
{
    [SolutionComponent]
    public class OverallCodeInsightsProvider : ICodeInsightsProvider
    {
        public bool IsAvailableIn(ISolution solution)
        {
            return true;
        }

        public void OnClick(CodeInsightsHighlighting highlighting, ISolution solution)
        {
        }

        public void OnExtraActionClick(CodeInsightsHighlighting highlighting, string actionId, ISolution solution)
        {
        }

        public string ProviderId => nameof(OverallCodeInsightsProvider);
        public string DisplayName => "Cognitive Complexity";
        public CodeLensAnchorKind DefaultAnchor => CodeLensAnchorKind.Top;

        public ICollection<CodeLensRelativeOrdering> RelativeOrderings => new CodeLensRelativeOrdering[]
            {new CodeLensRelativeOrderingFirst()};
    }
    
    // TODO: What is "CSharpInfo"?
//    [StaticSeverityHighlighting(Severity.INFO, "CSharpInfo", AttributeId = HighlightAttributeId)]
//    public class CodeInsightsHighlighting : JetBrains.ReSharper.Daemon.CodeInsights.CodeInsightsHighlighting
//    {
//        public const string HighlightAttributeId = "Cognitive Complexity Code Insight Highlight";
//
//        private static string GetLensText(int score)
//            => score.ToString();
//
//        private static string GetMoreText(int score)
//            => $"Cyclomatic complexity of {score}% of threshold)";
//
//        private static IconId GetIconId(int score)
//            => null;
//
//        public CodeInsightsHighlighting(
//            IFunctionDeclaration declaration,
//            int score,
//            ICodeInsightsProvider provider,
//            IconHost iconHost)
//            : base(
//                declaration.GetNameDocumentRange(),
//                GetLensText(score),
//                GetMoreText(score),
//                provider,
//                declaration.DeclaredElement,
//                icon: null)
////                iconHost.Transform(GetIconId(percentage)))
//        {
//        }
//    }
}