package com.jetbrains.rider.plugins.cognitivecomplexity.options

import com.jetbrains.rider.settings.simple.SimpleOptionsPage

class CognitiveComplexityOptionsPage : SimpleOptionsPage("Cognitive Complexity", "CognitiveComplexityAnalysisOptionPage") {
    override fun getId(): String {
        return "CognitiveComplexityAnalysisOptionPage"
    }
}