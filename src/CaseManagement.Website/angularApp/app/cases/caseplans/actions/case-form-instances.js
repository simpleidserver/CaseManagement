export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[CaseFormInstances] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[CaseFormInstances] COMPLETE_SEARCH";
})(ActionTypes || (ActionTypes = {}));
var StartFetch = (function () {
    function StartFetch(id, order, direction, count, startIndex) {
        this.id = id;
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
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
//# sourceMappingURL=case-form-instances.js.map