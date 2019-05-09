using JetBrains.Application.Settings;
using JetBrains.Application.UI.Controls.TreeListView;
using JetBrains.Application.UI.Options;
using JetBrains.DataFlow;
using JetBrains.Lifetimes;

namespace ReSharperPlugin.CognitiveComplexity.Options
{
    public class LanguageSpecificComplexityProperty : ObservableObject
    {
        public LanguageSpecificComplexityProperty(Lifetime lifetime, OptionsSettingsSmartContext settings, SettingsIndexedEntry settingsIndexedEntry, string index, string languagePresentableName, int defaultValue)
        {
            Name = languagePresentableName;

            Threshold = new Property<int>(lifetime, index);
            Threshold.Change.Advise(lifetime, () => OnPropertyChanged(nameof(Threshold)));
            settings.SetBinding(lifetime, settingsIndexedEntry, index, Threshold, defaultValue);
        }

        public string Name { get; private set; }
        public IProperty<int> Threshold { get; set; }
    }
}