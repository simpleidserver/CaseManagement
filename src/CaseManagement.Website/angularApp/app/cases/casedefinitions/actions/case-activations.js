export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["CASEDEFINITIONSLOAD"] = "[CaseDefinitions] Load";
    ActionTypes["CASEDEFINITIONSLOADED"] = "[CaseDefinitions] Loaded";
    ActionTypes["ERRORLOADCASEDEFINITIONS"] = "[CaseDefinitions] Error Load";
})(ActionTypes || (ActionTypes = {}));
var LoadCaseDefinitionsAction = (function () {
    function LoadCaseDefinitionsAction() {
        this.type = ActionTypes.CASEDEFINITIONSLOAD;
    }
    return LoadCaseDefinitionsAction;
}());
export { LoadCaseDefinitionsAction };
var CaseDefinitionsLoadedAction = (function () {
    function CaseDefinitionsLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.CASEDEFINITIONSLOADED;
    }
    return CaseDefinitionsLoadedAction;
}());
export { CaseDefinitionsLoadedAction };
//# sourceMappingURL=list-actions.js.map