export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["CASEDEFINITIONLOAD"] = "[CaseDefinition] Load";
    ActionTypes["CASEDEFINITIONLOADED"] = "[CaseDefinition] Loaded";
    ActionTypes["ERRORLOADCASEDEFINITION"] = "[CaseDefinition] Error Load";
    ActionTypes["CASEINSTANCESLOAD"] = "[CaseInstances] Load";
    ActionTypes["CASEINSTANCESLOADED"] = "[CaseInstances] Loaded";
    ActionTypes["ERRORLOADCASEINSTANCES"] = "[CaseInstances] Error Load";
    ActionTypes["CASEFORMINSTANCESLOAD"] = "[CaseFormInstances] Load";
    ActionTypes["CASEFORMINSTANCESLOADED"] = "[CaseFormInstances] Loaded";
    ActionTypes["ERRORLOADCASEFORMINSTANCES"] = "[CaseFormInstances] Error Load";
    ActionTypes["CASEACTIVATIONSLOAD"] = "[CaseActivations] Load";
    ActionTypes["CASEACTIVATIONSLOADED"] = "[CaseActivations] Loaded";
    ActionTypes["ERRORLOADCASEACTIVATIONS"] = "[CaseActivations] Error Load";
})(ActionTypes || (ActionTypes = {}));
var LoadCaseDefinitionAction = (function () {
    function LoadCaseDefinitionAction() {
        this.type = ActionTypes.CASEDEFINITIONLOAD;
    }
    return LoadCaseDefinitionAction;
}());
export { LoadCaseDefinitionAction };
var CaseDefinitionLoadedAction = (function () {
    function CaseDefinitionLoadedAction(caseDefinition, caseFile, caseDefinitionHistory) {
        this.caseDefinition = caseDefinition;
        this.caseFile = caseFile;
        this.caseDefinitionHistory = caseDefinitionHistory;
        this.type = ActionTypes.CASEDEFINITIONLOADED;
    }
    return CaseDefinitionLoadedAction;
}());
export { CaseDefinitionLoadedAction };
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
var LoadCaseFormInstancesAction = (function () {
    function LoadCaseFormInstancesAction() {
        this.type = ActionTypes.CASEFORMINSTANCESLOAD;
    }
    return LoadCaseFormInstancesAction;
}());
export { LoadCaseFormInstancesAction };
var LoadCaseFormInstancesLoadedAction = (function () {
    function LoadCaseFormInstancesLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.CASEFORMINSTANCESLOADED;
    }
    return LoadCaseFormInstancesLoadedAction;
}());
export { LoadCaseFormInstancesLoadedAction };
var LoadCaseFormActivationsAction = (function () {
    function LoadCaseFormActivationsAction() {
        this.type = ActionTypes.CASEACTIVATIONSLOAD;
    }
    return LoadCaseFormActivationsAction;
}());
export { LoadCaseFormActivationsAction };
var LoadCaseActivationsLoadedAction = (function () {
    function LoadCaseActivationsLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.CASEFORMINSTANCESLOADED;
    }
    return LoadCaseActivationsLoadedAction;
}());
export { LoadCaseActivationsLoadedAction };
//# sourceMappingURL=view-actions.js.map