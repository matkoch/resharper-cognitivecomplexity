using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.InlayHints;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.UI.RichText;
using ReSharperPlugin.CognitiveComplexity.Rider;
using Severity = JetBrains.ReSharper.Feature.Services.Daemon.Severity;

[assembly: RegisterHighlighterGroup(
    CognitiveComplexityHintBase.HighlightAttributeGroupId,
    CognitiveComplexityHintBase.HighlightAttributeIdBase,
    HighlighterGroupPriority.CODE_SETTINGS)]

[assembly: RegisterHighlighter(
    CognitiveComplexityInfoHint.HighlightAttributeId,
    GroupId = CognitiveComplexityHintBase.HighlightAttributeGroupId,
    ForegroundColor = CognitiveComplexityInfoHint.LightForegroundColor,
    BackgroundColor = CognitiveComplexityInfoHint.LightBackgroundColor,
    DarkForegroundColor = CognitiveComplexityInfoHint.DarkForegroundColor,
    DarkBackgroundColor = CognitiveComplexityInfoHint.DarkBackgroundColor,
    EffectType = EffectType.INTRA_TEXT_ADORNMENT,
    Layer = 3000,
    TransmitUpdates = true,
    VSPriority = 40)]

[assembly: RegisterHighlighter(
    CognitiveComplexityWarningHint.HighlightAttributeId,
    GroupId = CognitiveComplexityHintBase.HighlightAttributeGroupId,
    ForegroundColor = CognitiveComplexityWarningHint.LightForegroundColor,
    BackgroundColor = CognitiveComplexityWarningHint.LightBackgroundColor,
    DarkForegroundColor = CognitiveComplexityWarningHint.DarkForegroundColor,
    DarkBackgroundColor = CognitiveComplexityWarningHint.DarkBackgroundColor,
    EffectType = EffectType.INTRA_TEXT_ADORNMENT,
    Layer = 3000,
    TransmitUpdates = true,
    VSPriority = 40)]

[assembly: RegisterHighlighter(
    CognitiveComplexityErrorHint.HighlightAttributeId,
    GroupId = CognitiveComplexityHintBase.HighlightAttributeGroupId,
    ForegroundColor = CognitiveComplexityErrorHint.LightForegroundColor,
    BackgroundColor = CognitiveComplexityErrorHint.LightBackgroundColor,
    DarkForegroundColor = CognitiveComplexityErrorHint.DarkForegroundColor,
    DarkBackgroundColor = CognitiveComplexityErrorHint.DarkBackgroundColor,
    EffectType = EffectType.INTRA_TEXT_ADORNMENT,
    Layer = 3000,
    TransmitUpdates = true,
    VSPriority = 40)]

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    [DaemonIntraTextAdornmentProvider(typeof(CognitiveComplexityAdornmentProvider))]
    [DaemonTooltipProvider(typeof(InlayHintTooltipProvider))]
    [StaticSeverityHighlighting(Severity.INFO, "CSharpInfo", AttributeId = HighlightAttributeId)]
    public class CognitiveComplexityInfoHint : CognitiveComplexityHintBase
    {
        public const string HighlightAttributeId = HighlightAttributeIdBase + " Info Hint";

        internal const string DarkBackgroundColor = "#3B3B3C";
        internal const string DarkForegroundColor = "#787878";

        internal const string LightBackgroundColor = "#EBEBEB";
        internal const string LightForegroundColor = "#707070";

        public CognitiveComplexityInfoHint(ITreeNode node, DocumentOffset offset, int value)
            : base(node, offset, value)
        {
        }
    }

    [DaemonIntraTextAdornmentProvider(typeof(CognitiveComplexityAdornmentProvider))]
    [DaemonTooltipProvider(typeof(InlayHintTooltipProvider))]
    [StaticSeverityHighlighting(Severity.INFO, "CSharpInfo", AttributeId = HighlightAttributeId)]
    public class CognitiveComplexityWarningHint : CognitiveComplexityHintBase
    {
        public const string HighlightAttributeId = HighlightAttributeIdBase + " Warning Hint";

        internal const string DarkBackgroundColor = "#FFCD00";
        internal const string DarkForegroundColor = "#FFFFFF";

        internal const string LightBackgroundColor = "#FFDA00";
        internal const string LightForegroundColor = "#707070";

        public CognitiveComplexityWarningHint(ITreeNode node, DocumentOffset offset, int value)
            : base(node, offset, value)
        {
        }
    }

    [DaemonIntraTextAdornmentProvider(typeof(CognitiveComplexityAdornmentProvider))]
    [DaemonTooltipProvider(typeof(InlayHintTooltipProvider))]
    [StaticSeverityHighlighting(Severity.INFO, "CSharpInfo", AttributeId = HighlightAttributeId)]
    public class CognitiveComplexityErrorHint : CognitiveComplexityHintBase
    {
        public const string HighlightAttributeId = HighlightAttributeIdBase + " Error Hint";

        internal const string DarkBackgroundColor = "#CF0000";
        internal const string DarkForegroundColor = "#FFFFFF";

        internal const string LightBackgroundColor = "#FF0009";
        internal const string LightForegroundColor = "#F4F4F4";

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