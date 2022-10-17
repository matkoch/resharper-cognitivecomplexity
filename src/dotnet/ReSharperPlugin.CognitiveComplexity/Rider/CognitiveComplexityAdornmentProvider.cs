using JetBrains.ProjectModel;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.TextControl.DocumentMarkup.IntraTextAdornments;

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    [SolutionComponent]
    public class CognitiveComplexityAdornmentProvider : IHighlighterIntraTextAdornmentProvider
    {
        public bool IsValid(IHighlighter highlighter)
        {
            return highlighter.UserData is CognitiveComplexityHintBase;
        }

        public IIntraTextAdornmentDataModel CreateDataModel(IHighlighter highlighter)
        {
            return highlighter.UserData is CognitiveComplexityHintBase hint
                ? new CognitiveComplexityAdornmentDataModel(hint.Value)
                : null;
        }
    }
}
