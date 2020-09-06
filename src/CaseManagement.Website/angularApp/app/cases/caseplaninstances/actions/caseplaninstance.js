export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[CaseInstances] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[CaseInstances] COMPLETE_SEARCH";
    ActionTypes["START_GET"] = "[CaseInstances] START_GET";
    ActionTypes["COMPLETE_GET"] = "[CaseInstances] COMPLETE_GET";
})(ActionTypes || (ActionTypes = {}));
var StartSearch = (function () {
    function StartSearch(startIndex, count, order, direction, casePlanId) {
        this.startIndex = startIndex;
        this.count = count;
        this.order = order;
        this.direction = direction;
        this.casePlanId = casePlanId;
        this.type = ActionTypes.START_SEARCH;
    }
    return StartSearch;
}());
export { StartSearch };
var CompleteSearch = (function () {
    function CompleteSearch(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH;
    }
    return CompleteSearch;
}());
export { CompleteSearch };
var StartGet = (function () {
    function StartGet(id) {
        this.id = id;
        this.type = ActionTypes.START_GET;
    }
    return StartGet;
}());
export { StartGet };
var CompleteGet = (function () {
    function CompleteGet(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_GET;
    }
    return CompleteGet;
}());
export { CompleteGet };
//# sourceMappingURL=caseplaninstance.js.map