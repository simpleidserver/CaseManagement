export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["SEARCH_TASKS"] = "[Tasks] SEARCH_TASKS";
    ActionTypes["ERROR_SEARCH_TASKS"] = "[Tasks] ERROR_SEARCH_TASKS";
    ActionTypes["COMPLETE_SEARCH_TASKS"] = "[Tasks] COMPLETE_SEARCH_TASKS";
    ActionTypes["START_TASK"] = "[Tasks] START_TASK";
    ActionTypes["ERROR_START_TASK"] = "[Tasks] ERROR_START_TASK";
    ActionTypes["COMPLETE_START_TASK"] = "[Tasks] COMPLETE_START_TASK";
    ActionTypes["NOMINATE_TASK"] = "[Tasks] NOMINATE_TASK";
    ActionTypes["ERROR_NOMINATE_TASK"] = "[Tasks] ERROR_NOMINATE_TASK";
    ActionTypes["COMPLETE_NOMINATE_TASK"] = "[Tasks] COMPLETE_NOMINATE_TASK";
    ActionTypes["CLAIM_TASK"] = "[Tasks] CLAIM_TASK";
    ActionTypes["ERROR_CLAIM_TASK"] = "[Tasks] ERROR_CLAIM_TASK";
    ActionTypes["COMPLETE_CLAIM_TASK"] = "[Tasks] COMPLETE_CLAIM_TASK";
    ActionTypes["GET_TASK"] = "[Tasks] GET_TASK";
    ActionTypes["ERROR_GET_TASK"] = "[Tasks] ERROR_GET_TASK";
    ActionTypes["COMPLETE_GET_TASK"] = "[Tasks] COMPLETE_GET_TASK";
    ActionTypes["SUBMIT_TASK"] = "[Tasks] SUBMIT_TASK";
    ActionTypes["ERROR_SUBMIT_TASK"] = "[Tasks] ERROR_SUBMIT_TASK";
    ActionTypes["COMPLETE_SUBMIT_TASK"] = "[Tasks] COMPLETE_SUBMIT_TASK";
})(ActionTypes || (ActionTypes = {}));
var SearchTasks = (function () {
    function SearchTasks(order, direction, count, startIndex, owner, status) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.owner = owner;
        this.status = status;
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
var StartTask = (function () {
    function StartTask(humanTaskInstanceId) {
        this.humanTaskInstanceId = humanTaskInstanceId;
        this.type = ActionTypes.START_TASK;
    }
    return StartTask;
}());
export { StartTask };
var NominateTask = (function () {
    function NominateTask(humanTaskInstanceId, parameter) {
        this.humanTaskInstanceId = humanTaskInstanceId;
        this.parameter = parameter;
        this.type = ActionTypes.NOMINATE_TASK;
    }
    return NominateTask;
}());
export { NominateTask };
var ClaimTask = (function () {
    function ClaimTask(humanTaskInstanceId) {
        this.humanTaskInstanceId = humanTaskInstanceId;
        this.type = ActionTypes.CLAIM_TASK;
    }
    return ClaimTask;
}());
export { ClaimTask };
var RenderingTask = (function () {
    function RenderingTask(humanTaskInstanceId, order, direction) {
        this.humanTaskInstanceId = humanTaskInstanceId;
        this.order = order;
        this.direction = direction;
        this.type = ActionTypes.GET_TASK;
    }
    return RenderingTask;
}());
export { RenderingTask };
var CompleteRenderingTask = (function () {
    function CompleteRenderingTask(rendering, task, description, searchTaskHistory) {
        this.rendering = rendering;
        this.task = task;
        this.description = description;
        this.searchTaskHistory = searchTaskHistory;
        this.type = ActionTypes.COMPLETE_GET_TASK;
    }
    return CompleteRenderingTask;
}());
export { CompleteRenderingTask };
var SubmitTask = (function () {
    function SubmitTask(humanTaskInstanceId, operationParameters) {
        this.humanTaskInstanceId = humanTaskInstanceId;
        this.operationParameters = operationParameters;
        this.type = ActionTypes.SUBMIT_TASK;
    }
    return SubmitTask;
}());
export { SubmitTask };
//# sourceMappingURL=tasks.actions.js.map