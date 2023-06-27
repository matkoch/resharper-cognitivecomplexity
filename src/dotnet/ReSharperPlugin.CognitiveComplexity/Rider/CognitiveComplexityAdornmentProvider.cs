using JetBrains.ProjectModel;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.TextControl.DocumentMarkup.Adornments;

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    [SolutionComponent]
    public class CognitiveComplexityAdornmentProvider : IHighlighterAdornmentProvider
    {
        public bool IsValid(IHighlighter highlighter)
        {
            return highlighter.UserData is CognitiveComplexityHintBase;
        }

        public IAdornmentDataModel CreateDataModel(IHighlighter highlighter)
        {
            return highlighter.UserData is CognitiveComplexityHintBase hint
                ? new CognitiveComplexityAdornmentDataModel(hint.Value)
                : null;
        }
    }
}
