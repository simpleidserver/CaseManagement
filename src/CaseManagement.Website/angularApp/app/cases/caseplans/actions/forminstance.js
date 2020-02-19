export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[FormInstance] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[FormInstance] COMPLETE_SEARCH";
})(ActionTypes || (ActionTypes = {}));
var StartSearch = (function () {
    function StartSearch(id, order, direction, count, startIndex) {
        this.id = id;
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
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
//# sourceMappingURL=forminstance.js.map