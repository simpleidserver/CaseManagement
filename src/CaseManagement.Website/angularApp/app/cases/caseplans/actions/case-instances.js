export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[CaseInstances] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[CaseInstances] COMPLETE_SEARCH";
    ActionTypes["START_GET"] = "[CaseInstances] START_GET";
    ActionTypes["COMPLETE_GET"] = "[CaseInstances] COMPLETE_GET";
    ActionTypes["START_GET_FILE_ITEMS"] = "[CaseInstances] START_GET_FILE_ITEMS";
    ActionTypes["COMPLETE_GET_FILE_ITEMS"] = "[CaseInstances] COMPLETE_GET_FILE_ITEMS";
})(ActionTypes || (ActionTypes = {}));
var StartFetch = (function () {
    function StartFetch(id, startIndex, count, order, direction) {
        this.id = id;
        this.startIndex = startIndex;
        this.count = count;
        this.order = order;
        this.direction = direction;
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
var StartGetFileItems = (function () {
    function StartGetFileItems(id) {
        this.id = id;
        this.type = ActionTypes.START_GET_FILE_ITEMS;
    }
    return StartGetFileItems;
}());
export { StartGetFileItems };
var CompleteGetFileItems = (function () {
    function CompleteGetFileItems(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_GET_FILE_ITEMS;
    }
    return CompleteGetFileItems;
}());
export { CompleteGetFileItems };
//# sourceMappingURL=case-instances.js.map