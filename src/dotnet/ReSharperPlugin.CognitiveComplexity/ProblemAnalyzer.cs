using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CodeInsights;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.UI.Icons;
using ReSharperPlugin.CognitiveComplexity.Options;
using IBinaryExpression = JetBrains.ReSharper.Psi.CSharp.Tree.IBinaryExpression;
using IDoStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IDoStatement;
using IForeachStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IForeachStatement;
using IForStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IForStatement;
using IIfStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IIfStatement;
using ILambdaExpression = JetBrains.ReSharper.Psi.CSharp.Tree.ILambdaExpression;
using IParenthesizedExpression = JetBrains.ReSharper.Psi.CSharp.Tree.IParenthesizedExpression;
using ISwitchStatement = JetBrains.ReSharper.Psi.CSharp.Tree.ISwitchStatement;
using IWhileStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IWhileStatement;

#if RIDER
using JetBrains.ReSharper.Host.Platform.Icons;
#endif

namespace ReSharperPlugin.CognitiveComplexity
{
    // Types mentioned in this attribute are used for performance optimizations
    [ElementProblemAnalyzer(
        typeof(ICSharpFunctionDeclaration),
        HighlightingTypes = new[]
        {
            typeof(CognitiveComplexityHighlighting)
        })]
    public class ProblemAnalyzer : ElementProblemAnalyzer<ICSharpFunctionDeclaration>
    {
#if RIDER
        private readonly OverallCodeInsightsProvider _overallProvider;
        private readonly IconHost _iconHost;

        public ProblemAnalyzer(
            OverallCodeInsightsProvider overallProvider,
            IconHost iconHost)
        {
            _overallProvider = overallProvider;
            _iconHost = iconHost;
        }
#endif

        class ElementProcessor : IRecursiveElementProcessor
        {
            private readonly ICSharpFunctionDeclaration _element;
            private readonly IHighlightingConsumer _consumer;
            
            private int _nesting = 0;

            public ElementProcessor(
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
                if (IsNestingStatement(element))
                {
                    ComplexityScore += _nesting + 1;
                    _nesting++;
                    
                    if (element is ICatchClause clause && clause.Filter != null)
                        ComplexityScore++;
                }
                
                if (element is IGotoStatement ||
                    element is IBreakStatement ||
                    element is IContinueStatement ||
                    element is ICSharpStatement statement && IfStatementNavigator.GetByElse(statement) != null ||
                    element is IConditionalOrExpression && HasParent<IConditionalAndExpression>(element) ||
                    element is IConditionalAndExpression && HasParent<IConditionalOrExpression>(element) ||
                    element is ICSharpExpression expression && expression.HasRecursion(_element) ||
                    element is IConditionalOrExpression && !HasParent<IBinaryExpression>(element) ||
                    element is IConditionalAndExpression && !HasParent<IBinaryExpression>(element))
                {
                    ComplexityScore++;
                }

                if (element is ILambdaExpression ||
                    element is IAnonymousMethodExpression)
                {
                    _nesting++;
                }
            }

            private static bool IsNestingStatement(ITreeNode element)
            {
                return element is IWhileStatement ||
                       element is ISwitchStatement ||
                       element is IDoStatement ||
                       element is IIfStatement ifStatement && IfStatementNavigator.GetByElse(ifStatement) == null ||
                       element is IForStatement ||
                       element is IForeachStatement ||
                       element is ICatchClause;
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

            public void ProcessAfterInterior(ITreeNode element)
            {
                if (IsNestingStatement(element) ||
                    element is ICatchClause ||
                    element is ILambdaExpression ||
                    element is IAnonymousMethodExpression)
                {
                    _nesting--;
                }
            }
        }

        protected override void Run(ICSharpFunctionDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (element.Body == null)
                return;
                
            var elementProcessor = new ElementProcessor(
                element,
                consumer);
            element.Body.ProcessDescendants(elementProcessor);

            var store = data.SettingsStore;
            var baseThreshold = store.GetValue((CognitiveComplexityAnalysisSettings s) => s.CSharpThreshold);
            var lowThreshold = store.GetValue((CognitiveComplexityAnalysisSettings s) => s.LowComplexityThreshold);
            var middleThreshold = store.GetValue((CognitiveComplexityAnalysisSettings s) => s.MiddleComplexityThreshold);
            var highThreshold = store.GetValue((CognitiveComplexityAnalysisSettings s) => s.HighComplexityThreshold);

            var complexityPercentage = (int) (elementProcessor.ComplexityScore * 100.0 / baseThreshold);
            
            string codeLensText;
            IconId iconId;

            if (complexityPercentage >= highThreshold)
            {
                iconId = ComplexityExtreme.Id;
                codeLensText = complexityPercentage >= highThreshold * 2
                    ? $"refactor me? ({complexityPercentage}%)"
                    : $"very complex ({complexityPercentage}%)";
            }
            else if (complexityPercentage >= middleThreshold)
            {
                iconId = ComplexityHigh.Id;
                codeLensText = $"mildly complex ({complexityPercentage}%)";
            }
            else if (complexityPercentage >= lowThreshold)
            {
                iconId = ComplexityAverage.Id;
                codeLensText = $"simple enough ({complexityPercentage}%)";
            }
            else
            {
                iconId = ComplexityLow.Id;
                codeLensText = string.Empty;
            }

            if (complexityPercentage > 100)
                consumer.AddHighlighting(new CognitiveComplexityHighlighting(element, complexityPercentage));
            
#if RIDER
            var moreText =
                $"Cognitive complexity value of {elementProcessor.ComplexityScore} " +
                $"({complexityPercentage}% of threshold {baseThreshold})";
            consumer.AddHighlighting(
                new CodeInsightsHighlighting(
                    element.GetNameDocumentRange(),
                    codeLensText,
                    moreText,
                    moreText,
                    _overallProvider,
                    element.DeclaredElement,
                    _iconHost.Transform(iconId))
                );
#endif
        }
    }
}