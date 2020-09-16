export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH"] = "[CaseFiles] START_SEARCH";
    ActionTypes["COMPLETE_SEARCH"] = "[CaseFiles] COMPLETE_SEARCH";
    ActionTypes["START_SEARCH_HISTORY"] = "[CasesFiles] START_SEARCH_HISTORY";
    ActionTypes["COMPLETE_SEARCH_HISTORY"] = "[CasesFiles] COMPLETE_SEARCH_HISTORY";
    ActionTypes["START_GET"] = "[CaseFiles] START_GET";
    ActionTypes["COMPLETE_GET"] = "[CaseFiles] COMPLETE_GET";
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
var StartSearchHistory = (function () {
    function StartSearchHistory(caseFileId, order, direction, count, startIndex) {
        this.caseFileId = caseFileId;
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