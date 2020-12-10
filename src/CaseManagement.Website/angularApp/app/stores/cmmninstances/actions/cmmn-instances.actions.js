export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["LAUNCH_CMMN_PLANINSTANCE"] = "[CmmnInstances] LAUNCH_CMMN_PLANINSTANCE";
    ActionTypes["ERROR_LAUNCH_CMMN_PLANINSTANCE"] = "[CmmnInstances] ERROR_LAUNCH_CMMN_PLANINSTANCE";
    ActionTypes["COMPLETE_LAUNCH_CMMN_PLANINSTANCE"] = "[CmmnInstances] COMPLETE_LAUNCH_CMMN_PLANINSTANCE";
    ActionTypes["SEARCH_CMMN_PLANINSTANCE"] = "[CmmnInstances] SEARCH_CMMN_PLANINSTANCE";
    ActionTypes["ERROR_SEARCH_CMMN_PLANINSTANCE"] = "[CmmnInstances] ERROR_SEARCH_CMMN_PLANINSTANCE";
    ActionTypes["COMPLETE_SEARCH_CMMN_PLANINSTANCE"] = "[CmmnInstances] COMPLETE_SEARCH_CMMN_PLANINSTANCE";
})(ActionTypes || (ActionTypes = {}));
var LaunchCmmnPlanInstance = (function () {
    function LaunchCmmnPlanInstance(cmmnPlanId) {
        this.cmmnPlanId = cmmnPlanId;
        this.type = ActionTypes.LAUNCH_CMMN_PLANINSTANCE;
    }
    return LaunchCmmnPlanInstance;
}());
export { LaunchCmmnPlanInstance };
var SearchCmmnPlanInstance = (function () {
    function SearchCmmnPlanInstance(order, direction, count, startIndex, casePlanId) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.casePlanId = casePlanId;
        this.type = ActionTypes.LAUNCH_CMMN_PLANINSTANCE;
    }
    return SearchCmmnPlanInstance;
}());
export { SearchCmmnPlanInstance };
var CompleteSearchCmmnPlanInstances = (function () {
    function CompleteSearchCmmnPlanInstances(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_CMMN_PLANINSTANCE;
    }
    return CompleteSearchCmmnPlanInstances;
}());
export { CompleteSearchCmmnPlanInstances };
//# sourceMappingURL=cmmn-instances.actions.js.map