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
        ToolTipFormatString = Message)]
    public class WarningHighlighting : IHighlighting
    {
        public const string SeverityId = nameof(WarningHighlighting);
        public const string Message = "Sample highlighting message";
        public const string Description = "Sample highlighting description";
        
        public WarningHighlighting(ICSharpFunctionDeclaration declaration, int complexity)
        {
            Declaration = declaration;
            Complexity = complexity;
        }

        public ICSharpFunctionDeclaration Declaration { get; }
        public int Complexity { get; }

        public bool IsValid()
        {
            return Declaration.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return Declaration.NameIdentifier?.GetHighlightingRange() ?? DocumentRange.InvalidRange;
        }

        public string ToolTip => Complexity.ToString();
        
        public string ErrorStripeToolTip
            => Declaration.DeclaredName;
    }
}