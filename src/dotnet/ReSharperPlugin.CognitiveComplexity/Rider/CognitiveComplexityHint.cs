using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.InlayHints;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.UI.RichText;
using ReSharperPlugin.CognitiveComplexity.Rider;
using Severity = JetBrains.ReSharper.Feature.Services.Daemon.Severity;

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    // TODO: extract RegisterHighlighterGroup and RegisterHighlighter to separate class
    [RegisterHighlighterGroup(
        CognitiveComplexityHintBase.HighlightAttributeGroupId,
        CognitiveComplexityHintBase.HighlightAttributeIdBase,
        HighlighterGroupPriority.CODE_SETTINGS)]
    [RegisterHighlighter(
        HighlightAttributeId,
        GroupId = HighlightAttributeGroupId,
        ForegroundColor = "#707070",
        BackgroundColor = "#EBEBEB",
        DarkForegroundColor = "#787878",
        DarkBackgroundColor = "#3B3B3C",
        EffectType = EffectType.INTRA_TEXT_ADORNMENT,
        Layer = 3000,
        TransmitUpdates = true,
        VSPriority = 40)]
    [DaemonIntraTextAdornmentProvider(typeof(CognitiveComplexityAdornmentProvider))]
    [DaemonTooltipProvider(typeof(InlayHintTooltipProvider))]
    [StaticSeverityHighlighting(Severity.INFO, typeof(HighlightingGroupIds.CodeInsights), AttributeId = HighlightAttributeId)]
    public class CognitiveComplexityInfoHint : CognitiveComplexityHintBase
    {
        public const string HighlightAttributeId = HighlightAttributeIdBase + " Info Hint";

        public CognitiveComplexityInfoHint(ITreeNode node, DocumentOffset offset, int value)
            : base(node, offset, value)
        {
        }
    }

    [RegisterHighlighter(
        HighlightAttributeId,
        GroupId = HighlightAttributeGroupId,
        ForegroundColor = "#707070",
        BackgroundColor = "#FFDA00",
        DarkForegroundColor = "#FFFFFF",
        DarkBackgroundColor = "#FFCD00",
        EffectType = EffectType.INTRA_TEXT_ADORNMENT,
        Layer = 3000,
        TransmitUpdates = true,
        VSPriority = 40)]
    [DaemonIntraTextAdornmentProvider(typeof(CognitiveComplexityAdornmentProvider))]
    [DaemonTooltipProvider(typeof(InlayHintTooltipProvider))]
    [StaticSeverityHighlighting(Severity.INFO, typeof(HighlightingGroupIds.CodeInsights), AttributeId = HighlightAttributeId)]
    public class CognitiveComplexityWarningHint : CognitiveComplexityHintBase
    {
        public const string HighlightAttributeId = HighlightAttributeIdBase + " Warning Hint";

        public CognitiveComplexityWarningHint(ITreeNode node, DocumentOffset offset, int value)
            : base(node, offset, value)
        {
        }
    }

    [RegisterHighlighter(
        HighlightAttributeId,
        GroupId = HighlightAttributeGroupId,
        ForegroundColor = "#F4F4F4",
        BackgroundColor = "#FF0009",
        DarkForegroundColor = "#FFFFFF",
        DarkBackgroundColor = "#CF0000",
        EffectType = EffectType.INTRA_TEXT_ADORNMENT,
        Layer = 3000,
        TransmitUpdates = true,
        VSPriority = 40)]
    [DaemonIntraTextAdornmentProvider(typeof(CognitiveComplexityAdornmentProvider))]
    [DaemonTooltipProvider(typeof(InlayHintTooltipProvider))]
    [StaticSeverityHighlighting(Severity.INFO, typeof(HighlightingGroupIds.CodeInsights), AttributeId = HighlightAttributeId)]
    public class CognitiveComplexityErrorHint : CognitiveComplexityHintBase
    {
        public const string HighlightAttributeId = HighlightAttributeIdBase + " Error Hint";

        public CognitiveComplexityErrorHint(ITreeNode node, DocumentOffset offset, int value)
            : base(node, offset, value)
        {
        }
    }

    public abstract class CognitiveComplexityHintBase : IInlayHintWithDescriptionHighlighting
    {
        public const string HighlightAttributeIdBase = "Cognitive Complexity";
        public const string HighlightAttributeGroupId = HighlightAttributeIdBase + " Group";

        private readonly DocumentOffset _offset;
        private readonly ITreeNode _node;

        protected CognitiveComplexityHintBase(ITreeNode node, DocumentOffset offset, int value)
        {
            _node = node;
            _offset = offset;
            Value = value;
        }

        public int Value { get; }

        public bool IsValid()
        {
            return _node.IsValid();
        }

        public DocumentRange CalculateRange()
        {
            return new DocumentRange(_offset);
        }

        public string ToolTip { get; } = nameof(ToolTip);
        public string ErrorStripeToolTip { get; } = nameof(ErrorStripeToolTip);
        public RichText Description { get; } = nameof(Description);
    }
}
