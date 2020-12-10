export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["CREATE_HUMANTASKINSTANCE"] = "[HumanTaskInstance] CREATE_HUMANTASKINSTANCE";
    ActionTypes["COMPLETE_CREATE_HUMANTASKINSTANCE"] = "[HumanTaskInstance] COMPLETE_CREATE_HUMANTASKINSTANCE";
    ActionTypes["ERROR_CREATE_HUMANTASKINSTANCE"] = "[HumanTaskInstance] ERROR_CREATE_HUMANTASKINSTANCE";
    ActionTypes["CREATE_ME_HUMANTASKINSTANCE"] = "[HumanTaskInstance] CREATE_ME_HUMANTASKINSTANCE";
    ActionTypes["COMPLETE_ME_CREATE_HUMANTASKINSTANCE"] = "[HumanTaskInstance] COMPLETE_ME_CREATE_HUMANTASKINSTANCE";
    ActionTypes["ERROR_ME_CREATE_HUMANTASKINSTANCE"] = "[HumanTaskInstance] ERROR_ME_CREATE_HUMANTASKINSTANCE";
})(ActionTypes || (ActionTypes = {}));
var CreateHumanTaskInstanceOperation = (function () {
    function CreateHumanTaskInstanceOperation(cmd) {
        this.cmd = cmd;
        this.type = ActionTypes.CREATE_HUMANTASKINSTANCE;
    }
    return CreateHumanTaskInstanceOperation;
}());
export { CreateHumanTaskInstanceOperation };
var CreateMeHumanTaskInstanceOperation = (function () {
    function CreateMeHumanTaskInstanceOperation(cmd) {
        this.cmd = cmd;
        this.type = ActionTypes.CREATE_ME_HUMANTASKINSTANCE;
    }
    return CreateMeHumanTaskInstanceOperation;
}());
export { CreateMeHumanTaskInstanceOperation };
//# sourceMappingURL=humantaskinst.actions.js.map