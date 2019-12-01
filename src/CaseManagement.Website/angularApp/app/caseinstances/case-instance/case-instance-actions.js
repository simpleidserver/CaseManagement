export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["CASEINSTANCELOAD"] = "[CaseInstance] Load";
    ActionTypes["CASEINSTANCELOADED"] = "[CaseInstance] Loaded";
    ActionTypes["ERRORLOADCASEINSTANCE"] = "[CaseInstance] Error Load";
    ActionTypes["CASEEXECUTIONSSTEPSLOAD"] = "[CaseExecutionSteps] Load";
    ActionTypes["CASEEXECUTIONSTEPSLOADED"] = "[CaseExecutionSteps] Loaded";
    ActionTypes["ERRORLOADCASEEXECUTIONSTEPS"] = "[CaseExecutionSteps] Error Load";
})(ActionTypes || (ActionTypes = {}));
var CaseInstanceLoadedAction = (function () {
    function CaseInstanceLoadedAction(caseInstance, caseDefinition) {
        this.caseInstance = caseInstance;
        this.caseDefinition = caseDefinition;
        this.type = ActionTypes.CASEINSTANCELOAD;
    }
    return CaseInstanceLoadedAction;
}());
export { CaseInstanceLoadedAction };
var CaseExecutionStepsLoadedAction = (function () {
    function CaseExecutionStepsLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.CASEEXECUTIONSTEPSLOADED;
    }
    return CaseExecutionStepsLoadedAction;
}());
export { CaseExecutionStepsLoadedAction };
//# sourceMappingURL=case-instance-actions.js.map