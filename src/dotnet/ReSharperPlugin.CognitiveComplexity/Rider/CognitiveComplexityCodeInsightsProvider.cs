using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CodeInsights;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Rider.Model;

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    [SolutionComponent]
    public class CognitiveComplexityCodeInsightsProvider : ICodeInsightsProvider
    {
        public static bool ShowIndicators = true;

        private readonly IPsiServices _services;

        public CognitiveComplexityCodeInsightsProvider(IPsiServices services)
        {
            _services = services;
        }

        public bool IsAvailableIn(ISolution solution)
        {
            return true;
        }

        public void OnClick(CodeInsightsHighlighting highlighting, ISolution solution)
        {
            ShowIndicators = !ShowIndicators;
            using (WriteLockCookie.Create())
            {
                var sourceFile = highlighting.Range.Document.GetPsiSourceFile(solution);
                _services.MarkAsDirty(sourceFile.ToProjectFile());
            }
        }

        public void OnExtraActionClick(CodeInsightsHighlighting highlighting, string actionId, ISolution solution)
        {
        }

        public string ProviderId => nameof(CognitiveComplexityCodeInsightsProvider);
        public string DisplayName => "Cognitive Complexity";
        public CodeLensAnchorKind DefaultAnchor => CodeLensAnchorKind.Top;

        public ICollection<CodeLensRelativeOrdering> RelativeOrderings => new CodeLensRelativeOrdering[]
            {new CodeLensRelativeOrderingFirst()};
    }
}