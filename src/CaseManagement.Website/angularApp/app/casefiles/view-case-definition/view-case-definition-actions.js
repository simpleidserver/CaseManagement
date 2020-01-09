export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["CASEDEFSLOAD"] = "[CaseDefs] Load";
    ActionTypes["CASEDEFSLOADED"] = "[CaseDefs] Loaded";
    ActionTypes["ERRORLOADCASEDEFS"] = "[CaseDefs] Error Load";
})(ActionTypes || (ActionTypes = {}));
var LoadCaseDefsAction = (function () {
    function LoadCaseDefsAction() {
        this.type = ActionTypes.CASEDEFSLOAD;
    }
    return LoadCaseDefsAction;
}());
export { LoadCaseDefsAction };
var CaseDefsLoadedAction = (function () {
    function CaseDefsLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.CASEDEFSLOADED;
    }
    return CaseDefsLoadedAction;
}());
export { CaseDefsLoadedAction };
//# sourceMappingURL=list-case-defs-actions.js.map