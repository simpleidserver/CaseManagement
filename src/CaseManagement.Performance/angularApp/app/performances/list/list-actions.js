export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["PERFORMANCESLOAD"] = "[Performances] Load";
    ActionTypes["PERFORMANCESLOADED"] = "[Performances] Loaded";
    ActionTypes["ERRORLOADPERFORMANCES"] = "[CaseDefinitions] Error Load";
})(ActionTypes || (ActionTypes = {}));
var LoadPerformancesAction = (function () {
    function LoadPerformancesAction() {
        this.type = ActionTypes.PERFORMANCESLOAD;
    }
    return LoadPerformancesAction;
}());
export { LoadPerformancesAction };
var PerformancesLoadedAction = (function () {
    function PerformancesLoadedAction(result) {
        this.result = result;
        this.type = ActionTypes.PERFORMANCESLOADED;
    }
    return PerformancesLoadedAction;
}());
export { PerformancesLoadedAction };
//# sourceMappingURL=list-actions.js.map