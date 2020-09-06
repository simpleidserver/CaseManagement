export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[Role] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[Role] COMPLETE_SEARCH";
    ActionTypes["START_GET"] = "[Role] START_GET";
    ActionTypes["COMPLETE_GET"] = "[Role] COMPLETE_GET";
})(ActionTypes || (ActionTypes = {}));
var StartSearch = (function () {
    function StartSearch(startIndex, count, order, direction) {
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
var StartGet = (function () {
    function StartGet(role) {
        this.role = role;
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
//# sourceMappingURL=role.js.map