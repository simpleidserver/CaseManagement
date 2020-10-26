export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["CREATE_HUMANTASKINSTANCE"] = "[HumanTaskInstance] CREATE_HUMANTASKINSTANCE";
    ActionTypes["COMPLETE_CREATE_HUMANTASKINSTANCE"] = "[HumanTaskInstance] COMPLETE_CREATE_HUMANTASKINSTANCE";
    ActionTypes["ERROR_CREATE_HUMANTASKINSTANCE"] = "[HumanTaskInstance] ERROR_CREATE_HUMANTASKINSTANCE";
})(ActionTypes || (ActionTypes = {}));
var CreateHumanTaskInstanceOperation = (function () {
    function CreateHumanTaskInstanceOperation(cmd) {
        this.cmd = cmd;
        this.type = ActionTypes.CREATE_HUMANTASKINSTANCE;
    }
    return CreateHumanTaskInstanceOperation;
}());
export { CreateHumanTaskInstanceOperation };
//# sourceMappingURL=humantaskinst.actions.js.map