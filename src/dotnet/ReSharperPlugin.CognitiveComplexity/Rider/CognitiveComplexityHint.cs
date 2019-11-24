using System;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.InlayHints;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.UI.RichText;
using ReSharperPlugin.CognitiveComplexity.Rider;
using Xunit.Sdk;
using Severity = JetBrains.ReSharper.Feature.Services.Daemon.Severity;

//[assembly: RegisterHighlighter(
//    CodeInsightsHighlighting.HighlightAttributeId,
//    EffectType = EffectType.NONE,
//    TransmitUpdates = true,
//    Layer = HighlighterLayer.SYNTAX + 1,
//    GroupId = HighlighterGroupIds.HIDDEN)]

[assembly: RegisterHighlighterGroup(
    CognitiveComplexityHint.HighlightAttributeId + "Group",
    CognitiveComplexityHint.HighlightAttributeId,
    HighlighterGroupPriority.CODE_SETTINGS)]
[assembly: RegisterHighlighter(
    CognitiveComplexityHint.HighlightAttributeId,
    GroupId = CognitiveComplexityHint.HighlightAttributeId + "Group",
    BackgroundColor = "#0A0FA",
    DarkBackgroundColor = "#0A0FA",
    DarkForegroundColor = "#000000",
    EffectType = EffectType.INTRA_TEXT_ADORNMENT,
    ForegroundColor = "#000000",
    Layer = 3000,
//    RiderReplaceWith = "IDEA_INLINE_PARAMETER_HINT",
    TransmitUpdates = true,
    VSPriority = 40)]


namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    [DaemonIntraTextAdornmentProvider(typeof(CognitiveComplexityAdornmentProvider))]
    [DaemonTooltipProvider(typeof(InlayHintTooltipProvider))]
    [StaticSeverityHighlighting(Severity.INFO, "CSharpInfo", AttributeId = HighlightAttributeId)]
    public class CognitiveComplexityHint : IInlayHintWithDescriptionHighlighting
    {
        public const string HighlightAttributeId = "Cognitive Complexity Hint";

        private readonly DocumentOffset _offset;
        private readonly ITreeNode _node;

        public CognitiveComplexityHint(ITreeNode node, DocumentOffset offset, int value)
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
//            IBinaryExpression MostLeft<T>(T expression)
//                where T : IBinaryExpression
//            {
//                return expression.LeftOperand switch
//                {
//                    T similar => MostLeft<T>(similar),
//                    _ => expression
//                };
//            }
//
//            return new DocumentRange(
//                _node switch
//                {
//                    IForeachStatement foreachStatement => foreachStatement.RPar.GetDocumentRange().EndOffset,
//                    IIfStatement ifStatement => ifStatement.RPar.GetDocumentRange().EndOffset,
//                    IWhileStatement whileStatement => whileStatement.RPar.GetDocumentRange().EndOffset,
//                    IDoStatement doStatement => doStatement.RPar.GetDocumentRange().EndOffset,
//                    IForStatement forStatement => forStatement.RPar.GetDocumentRange().EndOffset,
//                    ISwitchStatement switchStatement => switchStatement.RParenth.GetDocumentRange().EndOffset,
//                    IBreakStatement breakStatement =>breakStatement.Semicolon.GetDocumentRange().EndOffset,
//                    IContinueStatement continueStatement => continueStatement.Semicolon.GetDocumentRange().EndOffset,
//                    IGotoStatement gotoStatement => gotoStatement.Semicolon.GetDocumentRange().EndOffset,
//                    ISpecificCatchClause catchClause => catchClause.RPar.GetDocumentRange().EndOffset,
//                    ICatchClause catchClause => catchClause.CatchKeyword.GetDocumentRange().EndOffset,
//                    IExceptionFilterClause filterClause => filterClause.RPar.GetDocumentRange().EndOffset,
//                    IConditionalAndExpression andExpression => MostLeft(andExpression).OperatorSign.GetDocumentRange().EndOffset,
//                    IConditionalOrExpression orExpression => MostLeft(orExpression).OperatorSign.GetDocumentRange().EndOffset,
//                    IReferenceExpression referenceExpression => referenceExpression.GetDocumentRange().EndOffset,
////                    IExpressionStatement expressionStatement => expressionStatement.Semicolon.GetDocumentRange().EndOffset,
//                    _ => throw new NotSupportedException($"{_node} : {_node.GetText()}")
//                });
        }

        public string ToolTip { get; } = nameof(ToolTip);
        public string ErrorStripeToolTip { get; } = nameof(ErrorStripeToolTip);
        public RichText Description { get; } = nameof(Description);
    }
}