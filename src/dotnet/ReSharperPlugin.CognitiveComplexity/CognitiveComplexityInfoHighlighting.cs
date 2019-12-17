using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl.DocumentMarkup;
using ReSharperPlugin.CognitiveComplexity;

[assembly: RegisterHighlighter(
    CognitiveComplexityInfoHighlighting.HighlightAttributeId)]

namespace ReSharperPlugin.CognitiveComplexity
{
    [StaticSeverityHighlighting(
        Severity.INFO,
        HighlightingGroupIds.CodeSmell,
        AttributeId = HighlightAttributeId)]
    public class CognitiveComplexityInfoHighlighting : IHighlighting
    {
        public const string HighlightAttributeId = nameof(CognitiveComplexityInfoHighlighting);

        public const string ToolTipFormatString = "Element has a cognitive complexity of {0} ({1}%)";

        public CognitiveComplexityInfoHighlighting(
            ICSharpFunctionDeclaration declaration,
            int complexityAbsolute,
            int complexityPercentage)
        {
            Declaration = declaration;
            ComplexityAbsolute = complexityAbsolute;
            ComplexityPercentage = complexityPercentage;
        }

        public ICSharpFunctionDeclaration Declaration { get; }

        public int ComplexityAbsolute { get; }
        public int ComplexityPercentage { get; }

        public bool IsValid()
        {
            return Declaration.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return Declaration.NameIdentifier?.GetHighlightingRange() ?? DocumentRange.InvalidRange;
        }

        public string ToolTip => string.Format(ToolTipFormatString, ComplexityAbsolute, ComplexityPercentage);

        public string ErrorStripeToolTip => Declaration.DeclaredName;
    }
}