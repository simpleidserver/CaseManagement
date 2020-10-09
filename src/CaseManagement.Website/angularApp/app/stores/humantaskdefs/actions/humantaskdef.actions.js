export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_GET_HUMANTASKDEF"] = "[HumanTaskDef] START_GET_HUMANTASKDEF";
    ActionTypes["COMPLETE_GET_HUMANTASKDEF"] = "[CasePlanInstance] COMPLETE_GET_HUMANTASKDEF";
    ActionTypes["ERROR_GET_HUMANTASKDEF"] = "[CasePlanInstance] ERROR_GET_HUMANTASKDEF";
    ActionTypes["UPDATE_HUMANASKDEF"] = "[HumanTaskDef] UPDATE_HUMANASKDEF";
    ActionTypes["COMPLETE_UPDATE_HUMANASKDEF"] = "[HumanTaskDef] COMPLETE_UPDATE_HUMANASKDEF";
    ActionTypes["ERROR_UPDATE_HUMANASKDEF"] = "[HumanTaskDef] ERROR_UPDATE_HUMANASKDEF";
    ActionTypes["ADD_START_DEADLINE"] = "[HumanTaskDef] ADD_START_DEADLINE";
    ActionTypes["COMPLETE_ADD_START_DEADLINE"] = "[HumanTaskDef] COMPLETE_ADD_STARTDEADLINE";
    ActionTypes["ERROR_ADD_START_DEADLINE"] = "[HumanTaskDef] ERROR_ADD_START_DEADLINE";
    ActionTypes["ADD_COMPLETION_DEADLINE"] = "[HumanTaskDef] ADD_COMPLETION_DEADLINE";
    ActionTypes["COMPLETE_ADD_COMPLETION_DEADLINE"] = "[HumanTaskDef] COMPLETE_ADD_COMPLETION_DEADLINE";
    ActionTypes["ERROR_ADD_COMPLETION_DEADLINE"] = "[HumanTaskDef] ERROR_ADD_COMPLETION_DEADLINE";
    ActionTypes["UPDATE_HUMANTASKDEF_INFO"] = "[HumanTaskDef] UPDATE_HUMANTASKDEF_INFO";
    ActionTypes["COMPLETE_UPDATE_HUMANTASK_INFO"] = "[HumanTaskDef] COMPLETE_UPDATE_HUMANTASK_INFO";
    ActionTypes["ERROR_UPDATE_HUMANTASK_INFO"] = "[HumanTaskDef] ERROR_UPDATE_HUMANTASK_INFO";
})(ActionTypes || (ActionTypes = {}));
var GetHumanTaskDef = (function () {
    function GetHumanTaskDef(id) {
        this.id = id;
        this.type = ActionTypes.START_GET_HUMANTASKDEF;
    }
    return GetHumanTaskDef;
}());
export { GetHumanTaskDef };
var GetHumanTaskDefComplete = (function () {
    function GetHumanTaskDefComplete(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_GET_HUMANTASKDEF;
    }
    return GetHumanTaskDefComplete;
}());
export { GetHumanTaskDefComplete };
var UpdateHumanTaskDef = (function () {
    function UpdateHumanTaskDef(humanTaskDef) {
        this.humanTaskDef = humanTaskDef;
        this.type = ActionTypes.UPDATE_HUMANASKDEF;
    }
    return UpdateHumanTaskDef;
}());
export { UpdateHumanTaskDef };
var UpdateHumanTaskDefComplete = (function () {
    function UpdateHumanTaskDefComplete(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_UPDATE_HUMANASKDEF;
    }
    return UpdateHumanTaskDefComplete;
}());
export { UpdateHumanTaskDefComplete };
var AddStartDeadLine = (function () {
    function AddStartDeadLine(id, deadLine) {
        this.id = id;
        this.deadLine = deadLine;
        this.type = ActionTypes.ADD_START_DEADLINE;
    }
    return AddStartDeadLine;
}());
export { AddStartDeadLine };
var CompleteAddStartDeadLine = (function () {
    function CompleteAddStartDeadLine(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_ADD_START_DEADLINE;
    }
    return CompleteAddStartDeadLine;
}());
export { CompleteAddStartDeadLine };
var AddCompletionDeadLine = (function () {
    function AddCompletionDeadLine(id, deadLine) {
        this.id = id;
        this.deadLine = deadLine;
        this.type = ActionTypes.ADD_COMPLETION_DEADLINE;
    }
    return AddCompletionDeadLine;
}());
export { AddCompletionDeadLine };
var CompleteCompletionDeadLine = (function () {
    function CompleteCompletionDeadLine(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE;
    }
    return CompleteCompletionDeadLine;
}());
export { CompleteCompletionDeadLine };
var UpdateHumanTaskInfo = (function () {
    function UpdateHumanTaskInfo(id, name, priority) {
        this.id = id;
        this.name = name;
        this.priority = priority;
        this.type = ActionTypes.UPDATE_HUMANTASKDEF_INFO;
    }
    return UpdateHumanTaskInfo;
}());
export { UpdateHumanTaskInfo };
var CompleteUpdateHumanTaskInfo = (function () {
    function CompleteUpdateHumanTaskInfo(name, priority) {
        this.name = name;
        this.priority = priority;
        this.type = ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO;
    }
    return CompleteUpdateHumanTaskInfo;
}());
export { CompleteUpdateHumanTaskInfo };
//# sourceMappingURL=humantaskdef.actions.js.map