using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.DataFlow;
using JetBrains.IDE.UI.Extensions;
using JetBrains.IDE.UI.Extensions.Properties;
using JetBrains.IDE.UI.Options;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.Daemon.OptionPages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ControlFlow;
using JetBrains.ReSharper.Psi.JavaScript.WinRT.LanguageImpl;
using JetBrains.Rider.Model.UIAutomation;

namespace ReSharperPlugin.CognitiveComplexity.Options
{
    [OptionsPage(PageId, "Cognitive Complexity", typeof(ComplexityExtreme), ParentId = CodeInspectionPage.PID)]
    public class CognitiveComplexityAnalysisOptionPage : BeSimpleOptionsPage
    {
        private const string PageId = nameof(CognitiveComplexityAnalysisOptionPage);

        public CognitiveComplexityAnalysisOptionPage(
            Lifetime lifetime,
            OptionsPageContext optionsPageContext,
            OptionsSettingsSmartContext optionsSettingsSmartContext,
            ILanguages languages,
            ILanguageManager languageManager)
            : base(lifetime, optionsPageContext, optionsSettingsSmartContext)
        {
            (Property<int> Property, BeSpinner Spinner) CreateComplexity(
                string id,
                Expression<Func<CognitiveComplexityAnalysisSettings, int>> setting)
            {
                var property = new Property<int>(lifetime, id + "Complexity");
                optionsSettingsSmartContext.SetBinding(lifetime, setting, property);
                return (property, property.GetBeSpinner(lifetime));
            }

            AddText("Language specific thresholds:");

            var csharpComplexity = CreateComplexity("csharp", s => s.CSharpThreshold);

            using (Indent())
            {
                AddControl(csharpComplexity.Spinner.WithDescription("CSharp:", lifetime));
            }

//            var thresholds =
//                OptionsSettingsSmartContext.Schema.GetIndexedEntry((CognitiveComplexityAnalysisSettings s) =>
//                    s.CSharpThreshold);
//
//            var list = new List<LanguageSpecificComplexityProperty>();
//            foreach (var languageType in languages.All.Where(languageManager.HasService<IControlFlowBuilder>)
//                .OrderBy(GetPresentableName))
//            {
//                var presentableName = GetPresentableName(languageType);
//                var thing = new LanguageSpecificComplexityProperty(lifetime, optionsSettingsSmartContext, thresholds,
//                    languageType.Name, presentableName, CognitiveComplexityAnalysisSettings.DefaultThreshold);
//                list.Add(thing);
//            }
//
//            var treeGrid = list.GetBeList(lifetime,
//                (l, e, p) => new List<BeControl>
//                {
//                    e.Name.GetBeLabel(),
//                    e.Threshold.GetBeSpinner(lifetime, min: 1)
//                },
//                new TreeConfiguration(new[] {"Language,*", "Threshold,auto"}));
//
//            AddControl(treeGrid, isStar: true);

#if RIDER
            AddText("CodeVision thresholds (in %):");

            var lowComplexity = CreateComplexity("low", s => s.LowComplexityThreshold);
            var middleComplexity = CreateComplexity("middle", s => s.MiddleComplexityThreshold);
            var highComplexity = CreateComplexity("high", s => s.HighComplexityThreshold);

            using (Indent())
            {
                AddControl(lowComplexity.Spinner.WithDescription("Simple enough:", lifetime));
                AddControl(middleComplexity.Spinner.WithDescription("Mildly complex:", lifetime));
                AddControl(highComplexity.Spinner.WithDescription("Very complex:", lifetime));
            }
#endif
        }
    }
}