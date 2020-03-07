using System.Collections.Generic;
using JetBrains.Application.UI.Controls.BulbMenu.Items;
using JetBrains.Application.UI.Controls.Utils;
using JetBrains.Application.UI.PopupLayout;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace ReSharperPlugin.CognitiveComplexity.Rider
{
    public class CognitiveComplexityAdornmentDataModel : IIntraTextAdornmentDataModel
    {
        private readonly int _value;

        public CognitiveComplexityAdornmentDataModel(int value)
        {
            _value = value;
        }

        public void ExecuteNavigation(PopupWindowContextSource popupWindowContextSource)
        {
        }

        public RichText Text => $"+{_value}";
        public bool HasContextMenu { get; }
        public IPresentableItem ContextMenuTitle { get; }
        public IEnumerable<BulbMenuItem> ContextMenuItems { get; }
        public bool IsNavigable { get; }
        public TextRange? SelectionRange { get; }
        public IconId IconId { get; }
        public bool IsPreceding { get; }
    }
}
