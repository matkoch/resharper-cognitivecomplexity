using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Tree;
#if RIDER
using ReSharperPlugin.CognitiveComplexity.Rider;
#endif

namespace ReSharperPlugin.CognitiveComplexity
{
    internal class CognitiveComplexityElementProcessor : IRecursiveElementProcessor
    {
        private readonly ICSharpFunctionDeclaration _element;
        private readonly IHighlightingConsumer _consumer;

        private int _nesting;

        public CognitiveComplexityElementProcessor(
            ICSharpFunctionDeclaration element,
            IHighlightingConsumer consumer)
        {
            _element = element;
            _consumer = consumer;
        }

        public int ComplexityScore { get; private set; }

        public bool ProcessingIsFinished => false;

        public bool InteriorShouldBeProcessed(ITreeNode element)
        {
            return true;
        }

        public void ProcessBeforeInterior(ITreeNode element)
        {
            void IncreaseComplexity(ITreeNode token = null, int i = 1)
            {
                ComplexityScore += i;

#if RIDER
                if (CognitiveComplexityCodeInsightsProvider.ShowIndicators)
                {
                    _consumer.AddHighlighting(
                        new CognitiveComplexityHint(
                            element,
                            (token ?? element).GetDocumentRange().EndOffset,
                            i));
                }
#endif
            }

            void IncreaseComplexityAndNesting(ITreeNode token = null)
            {
                IncreaseComplexity(token, 1 + _nesting);
                _nesting++;
            }

            switch (element)
            {
                case IWhileStatement whileStatement:
                    IncreaseComplexityAndNesting(whileStatement.RPar);
                    return;
                case ISwitchStatement switchStatement:
                    IncreaseComplexityAndNesting(switchStatement.RParenth);
                    return;
                case IDoStatement doStatement:
                    IncreaseComplexityAndNesting(doStatement.RPar);
                    return;
                case IIfStatement ifStatement
                    when IfStatementNavigator.GetByElse(ifStatement) == null:
                    IncreaseComplexityAndNesting(ifStatement.RPar);
                    return;
                case IForStatement forStatement:
                    IncreaseComplexityAndNesting(forStatement.RPar);
                    return;
                case IForeachStatement foreachStatement:
                    IncreaseComplexityAndNesting(foreachStatement.RPar);
                    return;
                case ICatchClause catchClause:
                {
                    IncreaseComplexityAndNesting(catchClause.CatchKeyword);
                    if (catchClause.Filter != null)
                        IncreaseComplexity(catchClause.Filter);
                    return;
                }
                case IGotoStatement gotoStatement:
                    IncreaseComplexity(gotoStatement.Semicolon);
                    return;
                case IBreakStatement breakStatement:
                    IncreaseComplexity(breakStatement.Semicolon);
                    return;
                case IContinueStatement continueStatement:
                    IncreaseComplexity(continueStatement.Semicolon);
                    return;
                case ICSharpExpression expression
                    when expression.HasRecursion(_element):
                    IncreaseComplexity();
                    return;
                case IConditionalOrExpression conditionalOrExpression
                    when HasParent<IConditionalAndExpression>(element) || !HasParent<IBinaryExpression>(element):
                    IncreaseComplexity(MostLeft(conditionalOrExpression).OperatorSign);
                    return;
                case IConditionalAndExpression conditionalAndExpression
                    when HasParent<IConditionalOrExpression>(element) || !HasParent<IBinaryExpression>(element):
                    IncreaseComplexity(MostLeft(conditionalAndExpression).OperatorSign);
                    return;
                case ICSharpStatement statement:
                {
                    var elseStatement = IfStatementNavigator.GetByElse(statement);
                    if (statement is IIfStatement ifStatement)
                        IncreaseComplexity(ifStatement.RPar);
                    else if (elseStatement != null)
                        IncreaseComplexity(elseStatement.ElseKeyword);
                    return;
                }
                case ILambdaExpression _:
                case IAnonymousMethodExpression _:
                    _nesting++;
                    return;
            }
        }

        private bool HasParent<T>(ITreeNode expression)
            where T : IBinaryExpression
        {
            switch (expression.Parent)
            {
                case IUnaryOperatorExpression unaryOperatorExpression
                    when unaryOperatorExpression.UnaryOperatorType == UnaryOperatorType.EXCL:
                    return HasParent<T>(unaryOperatorExpression);
                case IParenthesizedExpression parenthesizedExpression:
                    return HasParent<T>(parenthesizedExpression);
                case T _:
                    return true;
                default:
                    return false;
            }
        }

        private IBinaryExpression MostLeft<T>(T binaryExpression)
            where T : IBinaryExpression
        {
            return binaryExpression.LeftOperand switch
            {
                T similar => MostLeft(similar),
                _ => binaryExpression
            };
        }

        public void ProcessAfterInterior(ITreeNode element)
        {
            if (element is IWhileStatement ||
                element is ISwitchStatement ||
                element is IDoStatement ||
                element is IForStatement ||
                element is IForeachStatement ||
                element is ICatchClause ||
                element is ILambdaExpression ||
                element is IAnonymousMethodExpression ||
                element is IIfStatement ifStatement && IfStatementNavigator.GetByElse(ifStatement) == null)
            {
                _nesting--;
            }
        }
    }
}