using JetBrains.ProjectModel;
using JetBrains.TextControl.DocumentMarkup;

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    [SolutionComponent]
    public class CognitiveComplexityAdornmentProvider : IHighlighterIntraTextAdornmentProvider
    {
        public IIntraTextAdornmentDataModel CreateDataModel(IHighlighter highlighter)
        {
            return highlighter.UserData is CognitiveComplexityHint hint
                ? new CognitiveComplexityAdornmentDataModel(hint.Value)
                : null;
        }
    }
}