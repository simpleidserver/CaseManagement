export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[CaseInstances] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[CaseInstances] COMPLETE_SEARCH";
})(ActionTypes || (ActionTypes = {}));
var StartSearch = (function () {
    function StartSearch(id, startIndex, count, order, direction) {
        this.id = id;
        this.startIndex = startIndex;
        this.count = count;
        this.order = order;
        this.direction = direction;
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
//# sourceMappingURL=caseinstance.js.map