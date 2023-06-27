package com.jetbrains.rider.plugins.cognitivecomplexity.options

import com.jetbrains.rider.plugins.cognitivecomplexity.CognitiveComplexityBundle
import com.jetbrains.rider.settings.simple.SimpleOptionsPage

class CognitiveComplexityOptionsPage : SimpleOptionsPage(
    name = CognitiveComplexityBundle.message("configurable.name.cognitivecomplexity.options.title"),
    pageId = "CognitiveComplexityAnalysisOptionPage")
{
    override fun getId(): String {
        return "CognitiveComplexityAnalysisOptionPage"
    }
}
