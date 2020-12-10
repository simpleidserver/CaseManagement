var BpmnInstance = (function () {
    function BpmnInstance() {
        this.executionPaths = [];
    }
    return BpmnInstance;
}());
export { BpmnInstance };
var BpmnExecutionPath = (function () {
    function BpmnExecutionPath() {
        this.executionPointers = [];
    }
    return BpmnExecutionPath;
}());
export { BpmnExecutionPath };
var BpmnExecutionPointer = (function () {
    function BpmnExecutionPointer() {
        this.incomingTokens = [];
        this.outgoingTokens = [];
    }
    return BpmnExecutionPointer;
}());
export { BpmnExecutionPointer };
var BpmnMessageToken = (function () {
    function BpmnMessageToken() {
    }
    return BpmnMessageToken;
}());
export { BpmnMessageToken };
var BpmnNodeInstance = (function () {
    function BpmnNodeInstance() {
        this.activityStates = [];
    }
    return BpmnNodeInstance;
}());
export { BpmnNodeInstance };
var ActivityStateHistory = (function () {
    function ActivityStateHistory() {
    }
    return ActivityStateHistory;
}());
export { ActivityStateHistory };
//# sourceMappingURL=bpmn-instance.model.js.map