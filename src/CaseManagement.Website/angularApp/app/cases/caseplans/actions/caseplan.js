export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[CasePlan] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[CasePlan] COMPLETE_SEARCH";
    ActionTypes["START_GET"] = "[CasePlan] START_GET";
    ActionTypes["COMPLETE_GET"] = "[CasePlan] COMPLETE_GET";
})(ActionTypes || (ActionTypes = {}));
var StartSearch = (function () {
    function StartSearch(order, direction, count, startIndex, text) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.text = text;
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
//# sourceMappingURL=caseplan.js.map