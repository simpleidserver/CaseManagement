export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["START_SEARCH_CASE_PLANINSTANCES"] = "[CasePlanInstance] START_SEARCH_CASE_PLANINSTANCES";
    ActionTypes["COMPLETE_SEARCH_CASE_PLANINSTANCES"] = "[CasePlanInstance] COMPLETE_SEARCH_CASE_PLANINSTANCES";
    ActionTypes["ERROR_SEARCH_CASE_PLANINSTANCES"] = "[CasePlanInstance] ERROR_SEARCH_CASE_PLANINSTANCES";
    ActionTypes["START_GET_CASE_PLANINSTANCE"] = "[CasePlanInstance] START_GET_CASE_PLANINSTANCE";
    ActionTypes["COMPLETE_GET_CASE_PLANINSTANCE"] = "[CasePlanInstance] COMPLETE_GET_CASE_PLANINSTANCE";
    ActionTypes["ERROR_GET_CASE_PLANINSTANCE"] = "[CasePlanInstance] ERROR_GET_CASE_PLANINSTANCE";
    ActionTypes["LAUNCH_CASE_PLANINSTANCE"] = "[CasePlanInstance] LAUNCH_CASE_PLANINSTANCE";
    ActionTypes["ERROR_LAUNCH_CASE_PLANINSTANCE"] = "[CasePlanInstance] ERROR_LAUNCH_CASE_PLANINSTANCE";
    ActionTypes["COMPLETE_LAUNCH_CASE_PLANINSTANCE"] = "[CasePlanInstance] COMPLETE_LAUNCH_CASE_PLANINSTANCE";
    ActionTypes["REACTIVATE_CASE_PLANINSTANCE"] = "[CasePlanInstance] REACTIVATE_CASE_PLANINSTANCE";
    ActionTypes["ERROR_REACTIVATE_CASE_PLANINSTANCE"] = "[CasePlanInstance] ERROR_REACTIVATE_CASE_PLANINSTANCE";
    ActionTypes["COMPLETE_REACTIVATE_CASE_PLANINSTANCE"] = "[CasePlanInstance] COMPLETE_REACTIVATE_CASE_PLANINSTANCE";
    ActionTypes["SUSPEND_CASE_PLANINSTANCE"] = "[CasePlanInstance] SUSPEND_CASE_PLANINSTANCE";
    ActionTypes["ERROR_SUSPEND_CASE_PLANINSTANCE"] = "[CasePlanInstance] ERROR_SUSPEND_CASE_PLANINSTANCE";
    ActionTypes["COMPLETE_SUSPEND_CASE_PLANINSTANCE"] = "[CasePlanInstance] COMPLETE_SUSPEND_CASE_PLANINSTANCE";
    ActionTypes["RESUME_CASE_PLANINSTANCE"] = "[CasePlanInstance] RESUME_CASE_PLANINSTANCE";
    ActionTypes["ERROR_RESUME_CASE_PLANINSTANCE"] = "[CasePlanInstance] ERROR_RESUME_CASE_PLANINSTANCE";
    ActionTypes["COMPLETE_RESUME_CASE_PLANINSTANCE"] = "[CasePlanInstance] COMPLETE_RESUME_CASE_PLANINSTANCE";
    ActionTypes["CLOSE_CASE_PLANINSTANCE"] = "[CasePlanInstance] CLOSE_CASE_PLANINSTANCE";
    ActionTypes["ERROR_CLOSE_CASE_PLANINSTANCE"] = "[CasePlanInstance] ERROR_CLOSE_CASE_PLANINSTANCE";
    ActionTypes["COMPLETE_CLOSE_CASE_PLANINSTANCE"] = "[CasePlanInstance] COMPLETE_CLOSE_CASE_PLANINSTANCE";
    ActionTypes["ENABLE_CASE_PLANINSTANCE_ELT"] = "[CasePlanInstance] ENABLE_CASE_PLANINSTANCE_ELT";
    ActionTypes["ERROR_ENABLE_CASE_PLANINSTANCE_ELT"] = "[CasePlanInstance] ERROR_ENABLE_CASE_PLANINSTANCE_ELT";
    ActionTypes["COMPLETE_ENABLE_CASE_PLANINSTANCE_ELT"] = "[CasePlanInstance] COMPLETE_ENABLE_CASE_PLANINSTANCE_ELT";
})(ActionTypes || (ActionTypes = {}));
var SearchCasePlanInstances = (function () {
    function SearchCasePlanInstances(startIndex, count, order, direction, casePlanId) {
        this.startIndex = startIndex;
        this.count = count;
        this.order = order;
        this.direction = direction;
        this.casePlanId = casePlanId;
        this.type = ActionTypes.START_SEARCH_CASE_PLANINSTANCES;
    }
    return SearchCasePlanInstances;
}());
export { SearchCasePlanInstances };
var SearchCasePlanInstancesComplete = (function () {
    function SearchCasePlanInstancesComplete(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_CASE_PLANINSTANCES;
    }
    return SearchCasePlanInstancesComplete;
}());
export { SearchCasePlanInstancesComplete };
var GetCasePlanInstance = (function () {
    function GetCasePlanInstance(id) {
        this.id = id;
        this.type = ActionTypes.START_GET_CASE_PLANINSTANCE;
    }
    return GetCasePlanInstance;
}());
export { GetCasePlanInstance };
var GetCasePlanInstanceComplete = (function () {
    function GetCasePlanInstanceComplete(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_GET_CASE_PLANINSTANCE;
    }
    return GetCasePlanInstanceComplete;
}());
export { GetCasePlanInstanceComplete };
var LaunchCasePlanInstance = (function () {
    function LaunchCasePlanInstance(casePlanId) {
        this.casePlanId = casePlanId;
        this.type = ActionTypes.LAUNCH_CASE_PLANINSTANCE;
    }
    return LaunchCasePlanInstance;
}());
export { LaunchCasePlanInstance };
var ReactivateCasePlanInstance = (function () {
    function ReactivateCasePlanInstance(casePlanInstanceId) {
        this.casePlanInstanceId = casePlanInstanceId;
        this.type = ActionTypes.REACTIVATE_CASE_PLANINSTANCE;
    }
    return ReactivateCasePlanInstance;
}());
export { ReactivateCasePlanInstance };
var SuspendCasePlanInstance = (function () {
    function SuspendCasePlanInstance(casePlanInstanceId) {
        this.casePlanInstanceId = casePlanInstanceId;
        this.type = ActionTypes.SUSPEND_CASE_PLANINSTANCE;
    }
    return SuspendCasePlanInstance;
}());
export { SuspendCasePlanInstance };
var ResumeCasePlanInstance = (function () {
    function ResumeCasePlanInstance(casePlanInstanceId) {
        this.casePlanInstanceId = casePlanInstanceId;
        this.type = ActionTypes.RESUME_CASE_PLANINSTANCE;
    }
    return ResumeCasePlanInstance;
}());
export { ResumeCasePlanInstance };
var CloseCasePlanInstance = (function () {
    function CloseCasePlanInstance(casePlanInstanceId) {
        this.casePlanInstanceId = casePlanInstanceId;
        this.type = ActionTypes.CLOSE_CASE_PLANINSTANCE;
    }
    return CloseCasePlanInstance;
}());
export { CloseCasePlanInstance };
var EnableCasePlanInstanceElt = (function () {
    function EnableCasePlanInstanceElt(casePlanInstanceId, casePlanInstanceEltId) {
        this.casePlanInstanceId = casePlanInstanceId;
        this.casePlanInstanceEltId = casePlanInstanceEltId;
        this.type = ActionTypes.ENABLE_CASE_PLANINSTANCE_ELT;
    }
    return EnableCasePlanInstanceElt;
}());
export { EnableCasePlanInstanceElt };
//# sourceMappingURL=caseplaninstance.actions.js.map