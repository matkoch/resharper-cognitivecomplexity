using System.Collections.Generic;
using JetBrains.Application.UI.Controls.BulbMenu.Items;
using JetBrains.Application.UI.Controls.Utils;
using JetBrains.Application.UI.PopupLayout;
using JetBrains.TextControl.DocumentMarkup.IntraTextAdornments;
using JetBrains.Util;

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    public class CognitiveComplexityAdornmentDataModel : IIntraTextAdornmentDataModel
    {

        public CognitiveComplexityAdornmentDataModel(int value)
        {
            Data = new IntraTextAdornmentData()
                .WithText($"+{value}")
                .WithMode(InlayHintsMode.Always);
        }

        public void ExecuteNavigation(PopupWindowContextSource popupWindowContextSource)
        {
        }

        public IntraTextAdornmentData Data { get; }
        public IPresentableItem ContextMenuTitle { get; }
        public IEnumerable<BulbMenuItem> ContextMenuItems { get; }
        public TextRange? SelectionRange { get; }
    }
}
