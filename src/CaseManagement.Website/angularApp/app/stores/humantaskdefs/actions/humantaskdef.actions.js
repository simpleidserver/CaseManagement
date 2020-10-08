export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_GET_HUMANTASKDEF"] = "[HumanTaskDef] START_GET_HUMANTASKDEF";
    ActionTypes["COMPLETE_GET_HUMANTASKDEF"] = "[CasePlanInstance] COMPLETE_GET_HUMANTASKDEF";
    ActionTypes["ERROR_GET_HUMANTASKDEF"] = "[CasePlanInstance] ERROR_GET_HUMANTASKDEF";
    ActionTypes["UPDATE_HUMANASKDEF"] = "[HumanTaskDef] UPDATE_HUMANASKDEF";
    ActionTypes["COMPLETE_UPDATE_HUMANASKDEF"] = "[HumanTaskDef] COMPLETE_UPDATE_HUMANASKDEF";
    ActionTypes["ERROR_UPDATE_HUMANASKDEF"] = "[CasePlanInstance] ERROR_UPDATE_HUMANASKDEF";
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
//# sourceMappingURL=humantaskdef.actions.js.map