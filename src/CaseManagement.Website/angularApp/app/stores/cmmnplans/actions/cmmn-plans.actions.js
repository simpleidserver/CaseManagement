export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["SEARCH_CMMN_PLANS"] = "[CmmnPlans] SEARCH_CMMN_PLANS";
    ActionTypes["COMPLETE_SEARCH_CMMN_PLANS"] = "[CmmnPlans] COMPLETE_SEARCH_CMMN_PLANS";
    ActionTypes["ERROR_SEARCH_CMMN_PLANS"] = "[CmmnPlans] ERROR_SEARCH_CMMN_PLANS";
    ActionTypes["GET_CMMN_PLAN"] = "[CmmnPlans] START_GET";
    ActionTypes["COMPLETE_GET_CMMN_PLAN"] = "[CmmnPlans] COMPLETE_GET_CMMN_PLAN";
    ActionTypes["ERROR_GET_CMMN_PLAN"] = "[CmmnPlans] ERROR_GET_CMMN_PLAN";
})(ActionTypes || (ActionTypes = {}));
var SearchCmmnPlans = (function () {
    function SearchCmmnPlans(order, direction, count, startIndex, caseFileId, takeLatest) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.caseFileId = caseFileId;
        this.takeLatest = takeLatest;
        this.type = ActionTypes.SEARCH_CMMN_PLANS;
    }
    return SearchCmmnPlans;
}());
export { SearchCmmnPlans };
var CompleteSearchCmmnPlans = (function () {
    function CompleteSearchCmmnPlans(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_CMMN_PLANS;
    }
    return CompleteSearchCmmnPlans;
}());
export { CompleteSearchCmmnPlans };
var GetCmmnPlan = (function () {
    function GetCmmnPlan(id) {
        this.id = id;
        this.type = ActionTypes.GET_CMMN_PLAN;
    }
    return GetCmmnPlan;
}());
export { GetCmmnPlan };
var CompleteGet = (function () {
    function CompleteGet(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_GET_CMMN_PLAN;
    }
    return CompleteGet;
}());
export { CompleteGet };
//# sourceMappingURL=cmmn-plans.actions.js.map