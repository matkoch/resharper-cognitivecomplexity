using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.Parts;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CodeInsights;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Rider.Model;

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    [ShellComponent(Instantiation.ContainerAsyncPrimaryThread)]
    [HighlightingSource]
    public class CognitiveComplexityCodeInsightsProvider : ICodeInsightsProvider
    {
        public static bool ShowIndicators;

        public bool IsAvailableIn(ISolution solution)
        {
            return true;
        }

        public void OnClick(CodeInsightHighlightInfo highlightInfo, ISolution solution)
        {
            ShowIndicators = !ShowIndicators;
            using (WriteLockCookie.Create())
            {
                var sourceFile = highlightInfo.CodeInsightsHighlighting.Range.Document.GetPsiSourceFile(solution);
                var psiServices = solution.GetPsiServices();
                psiServices.MarkAsDirty(sourceFile.ToProjectFile());
            }
        }

        public void OnExtraActionClick(CodeInsightHighlightInfo highlightInfo, string actionId, ISolution solution)
        {
        }

        public string ProviderId => nameof(CognitiveComplexityCodeInsightsProvider);
        public string DisplayName => "Cognitive Complexity";
        public CodeVisionAnchorKind DefaultAnchor => CodeVisionAnchorKind.Top;

        public ICollection<CodeVisionRelativeOrdering> RelativeOrderings => new CodeVisionRelativeOrdering[]
            { new CodeVisionRelativeOrderingFirst() };
    }
}
