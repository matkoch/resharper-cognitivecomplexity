using System.Collections.Generic;
using JetBrains.Application.UI.Controls.BulbMenu.Items;
using JetBrains.Application.UI.Controls.Utils;
using JetBrains.Application.UI.PopupLayout;
using JetBrains.TextControl.DocumentMarkup.Adornments;
using JetBrains.Util;

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    public class CognitiveComplexityAdornmentDataModel : IAdornmentDataModel
    {

        public CognitiveComplexityAdornmentDataModel(int value)
        {
            Data = new AdornmentData()
                .WithText($"+{value}")
                .WithMode(PushToHintMode.Always);
        }

        public void ExecuteNavigation(PopupWindowContextSource popupWindowContextSource)
        {
        }

        public AdornmentData Data { get; }
        public IPresentableItem ContextMenuTitle { get; }
        public IEnumerable<BulbMenuItem> ContextMenuItems { get; }
        public TextRange? SelectionRange { get; }
    }
}
