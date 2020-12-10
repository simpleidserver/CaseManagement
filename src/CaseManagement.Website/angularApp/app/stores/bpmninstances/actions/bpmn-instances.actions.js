export var ActionTypes;
(function (ActionTypes) {
    ActionTypes["CREATE_BPMNINSTANCE"] = "[BpmnInstances] CREATE_BPMNINSTANCE";
    ActionTypes["COMPLETE_CREATE_BPMN_INSTANCE"] = "[BpmnInstances] COMPLETE_CREATE_BPMN_INSTANCE";
    ActionTypes["ERROR_CREATE_BPMNINSTANCE"] = "[BpmnInstances] ERROR_CREATE_BPMNINSTANCE";
    ActionTypes["START_BPMNINSTANCE"] = "[BpmnInstances] START_BPMNINSTANCE";
    ActionTypes["COMPLETE_START_BPMNINSTANCE"] = "[BpmnInstances] COMPLETE_START_BPMNINSTANCE";
    ActionTypes["ERROR_START_BPMNINSTANCE"] = "[BpmnInstances] ERROR_START_BPMNINSTANCE";
    ActionTypes["SEARCH_BPMNINSTANCES"] = "[BpmnInstances] SEARCH_BPMNINSTANCES";
    ActionTypes["COMPLETE_SEARCH_BPMNINSTANCES"] = "[BpmnInstances] COMPLETE_SEARCH_BPMNINSTANCES";
    ActionTypes["ERROR_SEARCH_BPMNINSTANCES"] = "[BpmnInstances] ERROR_SEARCH_BPMNINSTANCES";
    ActionTypes["GET_BPMNINSTANCE"] = "[BpmnInstances] GET_BPMNINSTANCE";
    ActionTypes["COMPLETE_GET_BPMNINSTANCE"] = "[BpmnInstances] COMPLETE_GET_BPMNINSTANCE";
    ActionTypes["ERROR_GET_BPMNINSTANCE"] = "[BpmnInstances] ERROR_GET_BPMNINSTANCE";
})(ActionTypes || (ActionTypes = {}));
var GetBpmnInstance = (function () {
    function GetBpmnInstance(id) {
        this.id = id;
        this.type = ActionTypes.GET_BPMNINSTANCE;
    }
    return GetBpmnInstance;
}());
export { GetBpmnInstance };
var CompleteGetBpmnInstance = (function () {
    function CompleteGetBpmnInstance(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_GET_BPMNINSTANCE;
    }
    return CompleteGetBpmnInstance;
}());
export { CompleteGetBpmnInstance };
var CreateBpmnInstance = (function () {
    function CreateBpmnInstance(processFileId) {
        this.processFileId = processFileId;
        this.type = ActionTypes.CREATE_BPMNINSTANCE;
    }
    return CreateBpmnInstance;
}());
export { CreateBpmnInstance };
var CompleteCreateBpmnInstance = (function () {
    function CompleteCreateBpmnInstance(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_CREATE_BPMN_INSTANCE;
    }
    return CompleteCreateBpmnInstance;
}());
export { CompleteCreateBpmnInstance };
var StartBpmnInstance = (function () {
    function StartBpmnInstance(id) {
        this.id = id;
        this.type = ActionTypes.START_BPMNINSTANCE;
    }
    return StartBpmnInstance;
}());
export { StartBpmnInstance };
var CompleteStartBpmnInstance = (function () {
    function CompleteStartBpmnInstance() {
        this.type = ActionTypes.COMPLETE_START_BPMNINSTANCE;
    }
    return CompleteStartBpmnInstance;
}());
export { CompleteStartBpmnInstance };
var SearchBpmnInstances = (function () {
    function SearchBpmnInstances(order, direction, count, startIndex, processFileId) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.processFileId = processFileId;
        this.type = ActionTypes.SEARCH_BPMNINSTANCES;
    }
    return SearchBpmnInstances;
}());
export { SearchBpmnInstances };
var CompleteSearchBpmnInstances = (function () {
    function CompleteSearchBpmnInstances(content) {
        this.content = content;
        this.type = ActionTypes.COMPLETE_SEARCH_BPMNINSTANCES;
    }
    return CompleteSearchBpmnInstances;
}());
export { CompleteSearchBpmnInstances };
//# sourceMappingURL=bpmn-instances.actions.js.map