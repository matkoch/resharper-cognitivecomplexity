using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperPlugin.CognitiveComplexity;

namespace ReSharperPlugin.CognitiveComplexity
{
    [RegisterConfigurableSeverity(
        SeverityId,
        CompoundItemName: null,
        Group: HighlightingGroupIds.CodeSmell,
        Title: Message,
        Description: Description,
        DefaultSeverity: Severity.WARNING)]
    [ConfigurableSeverityHighlighting(
        SeverityId,
        CSharpLanguage.Name,
        OverlapResolve = OverlapResolveKind.ERROR,
        OverloadResolvePriority = 0,
        ToolTipFormatString = ToolTipFormatString)]
    public class CognitiveComplexityErrorHighlighting : IHighlighting
    {
        public const string SeverityId = "CognitiveComplexity";
        public const string Message = "Element exceeds Cognitive Complexity threshold";

        public const string Description =
            "The cognitive complexity of the code element exceeds the configured threshold. " +
            "You can configure the thresholds in the Cognitive Complexity options page.";

        public const string ToolTipFormatString = Message + " by {0} ({1}%)";

        public CognitiveComplexityErrorHighlighting(
            ICSharpFunctionDeclaration declaration,
            int complexityDelta,
            int complexityPercentage)
        {
            Declaration = declaration;
            ComplexityDelta = complexityDelta;
            ComplexityPercentage = complexityPercentage;
        }

        public ICSharpFunctionDeclaration Declaration { get; }
        public int ComplexityDelta { get; }
        public int ComplexityPercentage { get; }

        public bool IsValid()
        {
            return Declaration.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return Declaration.NameIdentifier?.GetHighlightingRange() ?? DocumentRange.InvalidRange;
        }

        public string ToolTip => string.Format(ToolTipFormatString, ComplexityDelta, ComplexityPercentage);

        public string ErrorStripeToolTip => Declaration.DeclaredName;
    }
}