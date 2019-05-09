using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharperPlugin.CognitiveComplexity;

[assembly: RegisterConfigurableSeverity(
    WarningHighlighting.SeverityId,
    CompoundItemName: null,
    Group: HighlightingGroupIds.CodeSmell,
    Title: WarningHighlighting.Message,
    Description: WarningHighlighting.Description,
    DefaultSeverity: Severity.WARNING)]

namespace ReSharperPlugin.CognitiveComplexity
{
    [ConfigurableSeverityHighlighting(
        SeverityId,
        CSharpLanguage.Name,
        OverlapResolve = OverlapResolveKind.ERROR,
        OverloadResolvePriority = 0,
        ToolTipFormatString = ToolTipFormatString)]
    public class WarningHighlighting : IHighlighting
    {
        public const string SeverityId = nameof(WarningHighlighting);
        public const string Message = "Element exceeds Cognitive Complexity threshold";
        public const string Description = "The cognitive complexity of the code element exceeds the configured threshold. " +
                                          "You can configure the thresholds in the Cognitive Complexity options page.";

        public const string ToolTipFormatString = Message + " ({0}%)";
        
        public WarningHighlighting(ICSharpFunctionDeclaration declaration, int complexityPercentage)
        {
            Declaration = declaration;
            ComplexityPercentage = complexityPercentage;
        }

        public ICSharpFunctionDeclaration Declaration { get; }
        public int ComplexityPercentage { get; }

        public bool IsValid()
        {
            return Declaration.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return Declaration.NameIdentifier?.GetHighlightingRange() ?? DocumentRange.InvalidRange;
        }

        public string ToolTip => string.Format(ToolTipFormatString, ComplexityPercentage);
        
        public string ErrorStripeToolTip
            => Declaration.DeclaredName;
    }
}