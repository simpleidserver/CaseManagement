export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[CaseFiles] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[CaseFiles] COMPLETE_SEARCH";
    ActionTypes["START_GET"] = "[CaseFiles] START_GET";
    ActionTypes["COMPLETE_GET"] = "[CaseFiles] COMPLETE_GET";
})(ActionTypes || (ActionTypes = {}));
var StartFetch = (function () {
    function StartFetch(order, direction, count, startIndex, text, user) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.text = text;
        this.user = user;
        this.type = ActionTypes.START_SEARCH;
    }
    return StartFetch;
}());
export { StartFetch };
var CompleteFetch = (function () {
    function CompleteFetch(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH;
    }
    return CompleteFetch;
}());
export { CompleteFetch };
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
//# sourceMappingURL=case-files.js.map