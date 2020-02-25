export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[CaseInstances] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[CaseInstances] COMPLETE_SEARCH";
    ActionTypes["START_SEARCH_ME"] = "[CaseInstances] START_SEARCH_ME";
    ActionTypes["COMPLETE_SEARCH_ME"] = "[CaseInstances] COMPLETE_SEARCH_ME";
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
var StartSearchMe = (function () {
    function StartSearchMe(startIndex, count, order, direction) {
        this.startIndex = startIndex;
        this.count = count;
        this.order = order;
        this.direction = direction;
        this.type = ActionTypes.START_SEARCH_ME;
    }
    return StartSearchMe;
}());
export { StartSearchMe };
var CompleteSearchMe = (function () {
    function CompleteSearchMe(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_ME;
    }
    return CompleteSearchMe;
}());
export { CompleteSearchMe };
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