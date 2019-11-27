export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["CASEDEFLOAD"] = "[CaseDef] Load";
    ActionTypes["CASEDEFLOADED"] = "[CaseDef] Loaded";
    ActionTypes["ERRORLOADCASEDEF"] = "[CaseDef] Error Load";
})(ActionTypes || (ActionTypes = {}));
var LoadCaseDefAction = (function () {
    function LoadCaseDefAction() {
        this.type = ActionTypes.CASEDEFLOAD;
    }
    return LoadCaseDefAction;
}());
export { LoadCaseDefAction };
var CaseDefLoadedAction = (function () {
    function CaseDefLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.CASEDEFLOADED;
    }
    return CaseDefLoadedAction;
}());
export { CaseDefLoadedAction };
//# sourceMappingURL=case-def-actions.js.map