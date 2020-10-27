export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["SEARCH_TASKS"] = "[Tasks] SEARCH_TASKS";
    ActionTypes["ERROR_SEARCH_TASKS"] = "[Tasks] ERROR_SEARCH_TASKS";
    ActionTypes["COMPLETE_SEARCH_TASKS"] = "[Tasks] COMPLETE_SEARCH_TASKS";
})(ActionTypes || (ActionTypes = {}));
var SearchTasks = (function () {
    function SearchTasks(order, direction, count, startIndex) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.type = ActionTypes.SEARCH_TASKS;
    }
    return SearchTasks;
}());
export { SearchTasks };
var CompleteSearchTasks = (function () {
    function CompleteSearchTasks(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_TASKS;
    }
    return CompleteSearchTasks;
}());
export { CompleteSearchTasks };
//# sourceMappingURL=tasks.actions.js.map