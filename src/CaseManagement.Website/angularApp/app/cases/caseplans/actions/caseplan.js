export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[CasePlan] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[CasePlan] COMPLETE_SEARCH";
    ActionTypes["START_GET"] = "[CasePlan] START_GET";
    ActionTypes["COMPLETE_GET"] = "[CasePlan] COMPLETE_GET";
    ActionTypes["START_SEARCH_HISTORY"] = "[CasePlan] START_SEARCH_HISTORY";
    ActionTypes["COMPLETE_SEARCH_HISTORY"] = "[CasePlan] COMPLETE_SEARCH_HISTORY";
})(ActionTypes || (ActionTypes = {}));
var StartSearch = (function () {
    function StartSearch(order, direction, count, startIndex, text, caseFileId) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.text = text;
        this.caseFileId = caseFileId;
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
var StartSearchHistory = (function () {
    function StartSearchHistory(id, order, direction, count, startIndex) {
        this.id = id;
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.type = ActionTypes.START_SEARCH_HISTORY;
    }
    return StartSearchHistory;
}());
export { StartSearchHistory };
var CompleteSearchHistory = (function () {
    function CompleteSearchHistory(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_HISTORY;
    }
    return CompleteSearchHistory;
}());
export { CompleteSearchHistory };
//# sourceMappingURL=caseplan.js.map