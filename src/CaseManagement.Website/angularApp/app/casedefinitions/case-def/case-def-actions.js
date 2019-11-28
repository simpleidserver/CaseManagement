export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["CASEDEFLOAD"] = "[CaseDef] Load";
    ActionTypes["CASEDEFLOADED"] = "[CaseDef] Loaded";
    ActionTypes["ERRORLOADCASEDEF"] = "[CaseDef] Error Load";
    ActionTypes["CASEINSTANCESLOAD"] = "[CaseInstances] Load";
    ActionTypes["CASEINSTANCESLOADED"] = "[CaseInstances] Loaded";
    ActionTypes["ERRORLOADCASEINSTANCES"] = "[CaseInstances] Error Load";
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
var LoadCaseInstancesAction = (function () {
    function LoadCaseInstancesAction() {
        this.type = ActionTypes.CASEINSTANCESLOAD;
    }
    return LoadCaseInstancesAction;
}());
export { LoadCaseInstancesAction };
var CaseInstancesLoadedAction = (function () {
    function CaseInstancesLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.CASEINSTANCESLOADED;
    }
    return CaseInstancesLoadedAction;
}());
export { CaseInstancesLoadedAction };
//# sourceMappingURL=case-def-actions.js.map