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
    ActionTypes["ADD_OPERATION_INPUT_PARAMETER"] = "[HumanTaskDef] ADD_OPERATION_INPUT_PARAMETER";
    ActionTypes["COMPLETE_ADD_OPERATION_INPUT_PARAMETER"] = "[HumanTaskDef] COMPLETE_ADD_OPERATION_INPUT_PARAMETER";
    ActionTypes["ERROR_ADD_OPERATION_INPUT_PARAMETER"] = "[HumanTaskDef] ERROR_ADD_OPERATION_INPUT_PARAMETER";
    ActionTypes["ADD_OPERATION_OUTPUT_PARAMETER"] = "[HumanTaskDef] ADD_OPERATION_OUTPUT_PARAMETER";
    ActionTypes["COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER"] = "[HumanTaskDef] COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER";
    ActionTypes["ERROR_ADD_OPERATION_OUTPUT_PARAMETER"] = "[HumanTaskDef] ERROR_ADD_OPERATION_OUTPUT_PARAMETER";
    ActionTypes["DELETE_OPERATION_INPUT_PARAMETER"] = "[HumanTaskDef] DELETE_OPERATION_INPUT_PARAMETER";
    ActionTypes["COMPLETE_DELETE_OPERATION_INPUT_PARAMETER"] = "[HumanTaskDef] COMPLETE_DELETE_OPERATION_INPUT_PARAMETER";
    ActionTypes["ERROR_DELETE_OPERATION_INPUT_PARAMETER"] = "[HumanTaskDef] ERROR_DELETE_OPERATION_INPUT_PARAMETER";
    ActionTypes["DELETE_OPERATION_OUTPUT_PARAMETER"] = "[HumanTaskDef] DELETE_OPERATION_OUTPUT_PARAMETER";
    ActionTypes["COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER"] = "[HumanTaskDef] COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER";
    ActionTypes["ERROR_DELETE_OPERATION_OUTPUT_PARAMETER"] = "[HumanTaskDef] ERROR_DELETE_OPERATION_OUTPUT_PARAMETER";
    ActionTypes["UPDATE_RENDERING_PARAMETER"] = "[HumanTaskDef] UPDATE_RENDERING_PARAMETER";
    ActionTypes["COMPLETE_UPDATE_RENDERING_PARAMETER"] = "[HumanTaskDef] COMPLETE_UPDATE_RENDERING_PARAMETER";
    ActionTypes["ERROR_UPDATE_RENDERING_PARAMETER"] = "[HumanTaskDef] ERROR_UPDATE_RENDERING_PARAMETER";
    ActionTypes["DELETE_START_DEADLINE"] = "[HumanTaskDef] DELETE_START_DEADLINE";
    ActionTypes["COMPLETE_DELETE_START_DEALINE"] = "[HumanTaskDef] COMPLETE_DELETE_START_DEALINE";
    ActionTypes["ERROR_DELETE_START_DEALINE"] = "[HumanTaskDef] ERROR_DELETE_START_DEALINE";
    ActionTypes["DELETE_COMPLETION_DEADLINE"] = "[HumanTaskDef] DELETE_COMPLETION_DEADLINE";
    ActionTypes["COMPLETE_DELETE_COMPLETION_DEADLINE"] = "[HumanTaskDef] COMPLETE_DELETE_COMPLETION_DEADLINE";
    ActionTypes["ERROR_DELETE_COMPLETION_DEADLINE"] = "[HumanTaskDef] ERROR_DELETE_COMPLETION_DEADLINE";
    ActionTypes["UPDATE_START_DEADLINE"] = "[HumanTaskDef] UPDATE_START_DEADLINE";
    ActionTypes["COMPLETE_UPDATE_START_DEADLINE"] = "[HumanTaskDef] COMPLETE_UPDATE_START_DEADLINE";
    ActionTypes["ERROR_UPDATE_START_DEADLINE"] = "[HumanTaskDef] ERROR_UPDATE_START_DEADLINE";
    ActionTypes["UPDATE_COMPLETION_DEADLINE"] = "[HumanTaskDef] UPDATE_COMPLETION_DEADLINE";
    ActionTypes["COMPLETE_UPDATE_COMPLETION_DEADLINE"] = "[HumanTaskDef] COMPLETE_UPDATE_COMPLETION_DEADLINE";
    ActionTypes["ERROR_UPDATE_COMPLETION_DEADLINE"] = "[HumanTaskDef] ERROR_UPDATE_COMPLETION_DEADLINE";
    ActionTypes["ADD_ESCALATION_STARTDEADLINE"] = "[HumanTaskDef] ADD_ESCALATION_STARTDEADLINE";
    ActionTypes["COMPLETE_ADD_ESCALATION_STARTDEADLINE"] = "[HumanTaskDef] COMPLETE_ADD_ESCALATION_STARTDEADLINE";
    ActionTypes["ERROR_ADD_ESCALATION_STARTDEADLINE"] = "[HumanTaskDef] ERROR_ADD_ESCALATION_STARTDEADLINE";
    ActionTypes["ADD_ESCALATION_COMPLETIONDEADLINE"] = "[HumanTaskDef] ADD_ESCALATION_COMPLETIONDEADLINE";
    ActionTypes["COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE"] = "[HumanTaskDef] COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE";
    ActionTypes["ERROR_ADD_ESCALATION_COMPLETIONDEADLINE"] = "[HumanTaskDef] ERROR_ADD_ESCALATION_COMPLETIONDEADLINE";
    ActionTypes["UPDATE_PEOPLE_ASSIGNMENT"] = "[HumanTaskDef] UPDATE_PEOPLE_ASSIGNMENT";
    ActionTypes["COMPLETE_UPDATE_PEOPLE_ASSIGNMENT"] = "[HumanTaskDef] COMPLETE_UPDATE_PEOPLE_ASSIGNMENT";
    ActionTypes["ERROR_UPDATE_PEOPLE_ASSIGNMENT"] = "[HumanTaskDef] ERROR_UPDATE_PEOPLE_ASSIGNMENT";
    ActionTypes["UPDATE_COMPLETION_ESCALATION"] = "[HumanTaskDef] UPDATE_COMPLETION_ESCALATION";
    ActionTypes["COMPLETE_UPDATE_COMPLETION_ESCALATION"] = "[HumanTaskDef] COMPLETE_UPDATE_COMPLETION_ESCALATION";
    ActionTypes["ERROR_UPDATE_COMPLETION_ESCALATION"] = "[HumanTaskDef] ERROR_UPDATE_COMPLETION_ESCALATION";
    ActionTypes["UPDATE_START_ESCALATION"] = "[HumanTaskDef] UPDATE_START_ESCALATION";
    ActionTypes["ERROR_UPDATE_START_ESCALATION"] = "[HumanTaskDef] ERROR_UPDATE_START_ESCALATION";
    ActionTypes["COMPLETE_UPDATE_START_ESCALATION"] = "[HumanTaskDef] COMPLETE_UPDATE_START_ESCALATION";
    ActionTypes["DELETE_START_ESCALATION"] = "[HumanTaskDef] DELETE_START_ESCALATION";
    ActionTypes["ERROR_DELETE_START_ESCALATION"] = "[HumanTaskDef] ERROR_DELETE_START_ESCALATION";
    ActionTypes["COMPLETE_DELETE_START_ESCALATION"] = "[HumanTaskDef] COMPLETE_DELETE_START_ESCALATION";
    ActionTypes["DELETE_COMPLETION_ESCALATION"] = "[HumanTaskDef] DELETE_COMPLETION_ESCALATION";
    ActionTypes["ERROR_DELETE_COMPLETION_ESCALATION"] = "[HumanTaskDef] ERROR_DELETE_COMPLETION_ESCALATION";
    ActionTypes["COMPLETE_DELETE_COMPLETION_ESCALATION"] = "[HumanTaskDef] COMPLETE_DELETE_COMPLETION_ESCALATION";
    ActionTypes["ADD_HUMANTASKEF"] = "[HumanTaskDef] ADD_HUMANTASKDEF";
    ActionTypes["COMPLETE_ADD_HUMANTASKDEF"] = "[HumanTaskDef] COMPLETE_ADD_HUMANTASKDEF";
    ActionTypes["ERROR_ADD_HUMANTASKDEF"] = "[HumanTaskDef] ERROR_ADD_HUMANTASKDEF";
    ActionTypes["SEARCH_HUMANTASKDEFS"] = "[HumanTaskDef] SEARCH_HUMANTASKDEFS";
    ActionTypes["COMPLETE_SEARCH_HUMANTASKDEFS"] = "[HumanTaskDef] COMPLETE_SEARCH_HUMANTASKDEFS";
    ActionTypes["ERROR_SEARCH_HUMANTASKDEFS"] = "[HumanTaskDef] ERROR_SEARCH_HUMANTASKDEFS";
    ActionTypes["GET_HUMANTASKDEF"] = "[HumanTaskDef] GET_HUMANTASKDEF";
    ActionTypes["UPDATE_PRESENTATIONELEMENT"] = "[HumanTaskDef] UPDATE_PRESENTATIONELEMENT";
    ActionTypes["COMPLETE_UPDATE_PRESENTATIONELEMENT"] = "[HumanTaskDef] COMPLETE_UPDATE_PRESENTATIONELEMENT";
    ActionTypes["ERROR_UPDATE_PRESENTATIONELEMENT"] = "[HumanTaskDef] ERROR_UPDATE_PRESENTATIONELEMENT";
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
var AddInputParameterOperation = (function () {
    function AddInputParameterOperation(id, parameter) {
        this.id = id;
        this.parameter = parameter;
        this.type = ActionTypes.ADD_OPERATION_INPUT_PARAMETER;
    }
    return AddInputParameterOperation;
}());
export { AddInputParameterOperation };
var CompleteAddInputParameterOperation = (function () {
    function CompleteAddInputParameterOperation(parameter) {
        this.parameter = parameter;
        this.type = ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER;
    }
    return CompleteAddInputParameterOperation;
}());
export { CompleteAddInputParameterOperation };
var AddOutputParameterOperation = (function () {
    function AddOutputParameterOperation(id, parameter) {
        this.id = id;
        this.parameter = parameter;
        this.type = ActionTypes.ADD_OPERATION_OUTPUT_PARAMETER;
    }
    return AddOutputParameterOperation;
}());
export { AddOutputParameterOperation };
var CompleteAddOutputParameterOperation = (function () {
    function CompleteAddOutputParameterOperation(id, parameter) {
        this.id = id;
        this.parameter = parameter;
        this.type = ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER;
    }
    return CompleteAddOutputParameterOperation;
}());
export { CompleteAddOutputParameterOperation };
var DeleteInputParameterOperation = (function () {
    function DeleteInputParameterOperation(id, name) {
        this.id = id;
        this.name = name;
        this.type = ActionTypes.DELETE_OPERATION_INPUT_PARAMETER;
    }
    return DeleteInputParameterOperation;
}());
export { DeleteInputParameterOperation };
var CompleteDeleteInputParameterOperation = (function () {
    function CompleteDeleteInputParameterOperation(name) {
        this.name = name;
        this.type = ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER;
    }
    return CompleteDeleteInputParameterOperation;
}());
export { CompleteDeleteInputParameterOperation };
var DeleteOutputParameterOperation = (function () {
    function DeleteOutputParameterOperation(id, name) {
        this.id = id;
        this.name = name;
        this.type = ActionTypes.DELETE_OPERATION_OUTPUT_PARAMETER;
    }
    return DeleteOutputParameterOperation;
}());
export { DeleteOutputParameterOperation };
var CompleteDeleteOutputParameterOperation = (function () {
    function CompleteDeleteOutputParameterOperation(name) {
        this.name = name;
        this.type = ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER;
    }
    return CompleteDeleteOutputParameterOperation;
}());
export { CompleteDeleteOutputParameterOperation };
var UpdateRenderingOperation = (function () {
    function UpdateRenderingOperation(id, rendering) {
        this.id = id;
        this.rendering = rendering;
        this.type = ActionTypes.UPDATE_RENDERING_PARAMETER;
    }
    return UpdateRenderingOperation;
}());
export { UpdateRenderingOperation };
var CompleteUpdateRenderingOperation = (function () {
    function CompleteUpdateRenderingOperation(rendering) {
        this.rendering = rendering;
        this.type = ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER;
    }
    return CompleteUpdateRenderingOperation;
}());
export { CompleteUpdateRenderingOperation };
var DeleteStartDeadlineOperation = (function () {
    function DeleteStartDeadlineOperation(id, deadLineId) {
        this.id = id;
        this.deadLineId = deadLineId;
        this.type = ActionTypes.DELETE_START_DEADLINE;
    }
    return DeleteStartDeadlineOperation;
}());
export { DeleteStartDeadlineOperation };
var CompleteDeleteStartDeadlineOperation = (function () {
    function CompleteDeleteStartDeadlineOperation(deadLineId) {
        this.deadLineId = deadLineId;
        this.type = ActionTypes.COMPLETE_DELETE_START_DEALINE;
    }
    return CompleteDeleteStartDeadlineOperation;
}());
export { CompleteDeleteStartDeadlineOperation };
var DeleteCompletionDeadlineOperation = (function () {
    function DeleteCompletionDeadlineOperation(id, deadLineId) {
        this.id = id;
        this.deadLineId = deadLineId;
        this.type = ActionTypes.DELETE_COMPLETION_DEADLINE;
    }
    return DeleteCompletionDeadlineOperation;
}());
export { DeleteCompletionDeadlineOperation };
var CompleteDeleteCompletionDeadlineOperation = (function () {
    function CompleteDeleteCompletionDeadlineOperation(deadLineId) {
        this.deadLineId = deadLineId;
        this.type = ActionTypes.COMPLETE_DELETE_COMPLETION_DEADLINE;
    }
    return CompleteDeleteCompletionDeadlineOperation;
}());
export { CompleteDeleteCompletionDeadlineOperation };
var UpdateStartDeadlineOperation = (function () {
    function UpdateStartDeadlineOperation(id, deadline) {
        this.id = id;
        this.deadline = deadline;
        this.type = ActionTypes.UPDATE_START_DEADLINE;
    }
    return UpdateStartDeadlineOperation;
}());
export { UpdateStartDeadlineOperation };
var CompleteUpdateStartDeadlineOperation = (function () {
    function CompleteUpdateStartDeadlineOperation(deadline) {
        this.deadline = deadline;
        this.type = ActionTypes.COMPLETE_UPDATE_START_DEADLINE;
    }
    return CompleteUpdateStartDeadlineOperation;
}());
export { CompleteUpdateStartDeadlineOperation };
var UpdateCompletionDeadlineOperation = (function () {
    function UpdateCompletionDeadlineOperation(id, deadline) {
        this.id = id;
        this.deadline = deadline;
        this.type = ActionTypes.UPDATE_COMPLETION_DEADLINE;
    }
    return UpdateCompletionDeadlineOperation;
}());
export { UpdateCompletionDeadlineOperation };
var CompleteCompletionStartDeadlineOperation = (function () {
    function CompleteCompletionStartDeadlineOperation(deadline) {
        this.deadline = deadline;
        this.type = ActionTypes.COMPLETE_UPDATE_COMPLETION_DEADLINE;
    }
    return CompleteCompletionStartDeadlineOperation;
}());
export { CompleteCompletionStartDeadlineOperation };
var AddEscalationStartDeadlineOperation = (function () {
    function AddEscalationStartDeadlineOperation(id, deadlineId, condition) {
        this.id = id;
        this.deadlineId = deadlineId;
        this.condition = condition;
        this.type = ActionTypes.ADD_ESCALATION_STARTDEADLINE;
    }
    return AddEscalationStartDeadlineOperation;
}());
export { AddEscalationStartDeadlineOperation };
var CompleteAddEscalationStartDeadlineOperation = (function () {
    function CompleteAddEscalationStartDeadlineOperation(deadlineId, condition, escId) {
        this.deadlineId = deadlineId;
        this.condition = condition;
        this.escId = escId;
        this.type = ActionTypes.COMPLETE_ADD_ESCALATION_STARTDEADLINE;
    }
    return CompleteAddEscalationStartDeadlineOperation;
}());
export { CompleteAddEscalationStartDeadlineOperation };
var AddEscalationCompletionDeadlineOperation = (function () {
    function AddEscalationCompletionDeadlineOperation(id, deadlineId, condition) {
        this.id = id;
        this.deadlineId = deadlineId;
        this.condition = condition;
        this.type = ActionTypes.ADD_ESCALATION_COMPLETIONDEADLINE;
    }
    return AddEscalationCompletionDeadlineOperation;
}());
export { AddEscalationCompletionDeadlineOperation };
var CompleteAddEscalationCompletionDeadlineOperation = (function () {
    function CompleteAddEscalationCompletionDeadlineOperation(deadlineId, condition, escId) {
        this.deadlineId = deadlineId;
        this.condition = condition;
        this.escId = escId;
        this.type = ActionTypes.COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE;
    }
    return CompleteAddEscalationCompletionDeadlineOperation;
}());
export { CompleteAddEscalationCompletionDeadlineOperation };
var UpdatePeopleAssignmentOperation = (function () {
    function UpdatePeopleAssignmentOperation(id, assignment) {
        this.id = id;
        this.assignment = assignment;
        this.type = ActionTypes.UPDATE_PEOPLE_ASSIGNMENT;
    }
    return UpdatePeopleAssignmentOperation;
}());
export { UpdatePeopleAssignmentOperation };
var CompletePeopleAssignmentOperation = (function () {
    function CompletePeopleAssignmentOperation(assignment) {
        this.assignment = assignment;
        this.type = ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT;
    }
    return CompletePeopleAssignmentOperation;
}());
export { CompletePeopleAssignmentOperation };
var UpdateStartEscalationOperation = (function () {
    function UpdateStartEscalationOperation(id, deadLineId, escalation) {
        this.id = id;
        this.deadLineId = deadLineId;
        this.escalation = escalation;
        this.type = ActionTypes.UPDATE_START_ESCALATION;
    }
    return UpdateStartEscalationOperation;
}());
export { UpdateStartEscalationOperation };
var CompleteUpdateStartEscalationOperation = (function () {
    function CompleteUpdateStartEscalationOperation(deadLineId, escalation) {
        this.deadLineId = deadLineId;
        this.escalation = escalation;
        this.type = ActionTypes.COMPLETE_UPDATE_START_ESCALATION;
    }
    return CompleteUpdateStartEscalationOperation;
}());
export { CompleteUpdateStartEscalationOperation };
var UpdateCompletionEscalationOperation = (function () {
    function UpdateCompletionEscalationOperation(id, deadLineId, escalation) {
        this.id = id;
        this.deadLineId = deadLineId;
        this.escalation = escalation;
        this.type = ActionTypes.UPDATE_COMPLETION_ESCALATION;
    }
    return UpdateCompletionEscalationOperation;
}());
export { UpdateCompletionEscalationOperation };
var CompleteUpdateCompletionEscalationOperation = (function () {
    function CompleteUpdateCompletionEscalationOperation(deadLineId, escalation) {
        this.deadLineId = deadLineId;
        this.escalation = escalation;
        this.type = ActionTypes.COMPLETE_UPDATE_COMPLETION_ESCALATION;
    }
    return CompleteUpdateCompletionEscalationOperation;
}());
export { CompleteUpdateCompletionEscalationOperation };
var DeleteStartEscalationOperation = (function () {
    function DeleteStartEscalationOperation(id, deadLineId, escalation) {
        this.id = id;
        this.deadLineId = deadLineId;
        this.escalation = escalation;
        this.type = ActionTypes.DELETE_START_ESCALATION;
    }
    return DeleteStartEscalationOperation;
}());
export { DeleteStartEscalationOperation };
var CompleteDeleteStartEscalationOperation = (function () {
    function CompleteDeleteStartEscalationOperation(deadLineId, escalation) {
        this.deadLineId = deadLineId;
        this.escalation = escalation;
        this.type = ActionTypes.COMPLETE_DELETE_START_ESCALATION;
    }
    return CompleteDeleteStartEscalationOperation;
}());
export { CompleteDeleteStartEscalationOperation };
var DeleteCompletionEscalationOperation = (function () {
    function DeleteCompletionEscalationOperation(id, deadLineId, escalation) {
        this.id = id;
        this.deadLineId = deadLineId;
        this.escalation = escalation;
        this.type = ActionTypes.DELETE_COMPLETION_ESCALATION;
    }
    return DeleteCompletionEscalationOperation;
}());
export { DeleteCompletionEscalationOperation };
var CompleteDeleteCompletionEscalationOperation = (function () {
    function CompleteDeleteCompletionEscalationOperation(deadLineId, escalation) {
        this.deadLineId = deadLineId;
        this.escalation = escalation;
        this.type = ActionTypes.COMPLETE_DELETE_COMPLETION_ESCALATION;
    }
    return CompleteDeleteCompletionEscalationOperation;
}());
export { CompleteDeleteCompletionEscalationOperation };
var AddHumanTaskDefOperation = (function () {
    function AddHumanTaskDefOperation(name) {
        this.name = name;
        this.type = ActionTypes.ADD_HUMANTASKEF;
    }
    return AddHumanTaskDefOperation;
}());
export { AddHumanTaskDefOperation };
var CompleteAddHumanTaskDefOperation = (function () {
    function CompleteAddHumanTaskDefOperation(humanTaskDef) {
        this.humanTaskDef = humanTaskDef;
        this.type = ActionTypes.COMPLETE_ADD_HUMANTASKDEF;
    }
    return CompleteAddHumanTaskDefOperation;
}());
export { CompleteAddHumanTaskDefOperation };
var SearchHumanTaskDefOperation = (function () {
    function SearchHumanTaskDefOperation(order, direction, count, startIndex) {
        this.order = order;
        this.direction = direction;
        this.count = count;
        this.startIndex = startIndex;
        this.type = ActionTypes.SEARCH_HUMANTASKDEFS;
    }
    return SearchHumanTaskDefOperation;
}());
export { SearchHumanTaskDefOperation };
var CompleteSearchHumanTaskDefOperation = (function () {
    function CompleteSearchHumanTaskDefOperation(humanTaskDefsResult) {
        this.humanTaskDefsResult = humanTaskDefsResult;
        this.type = ActionTypes.COMPLETE_SEARCH_HUMANTASKDEFS;
    }
    return CompleteSearchHumanTaskDefOperation;
}());
export { CompleteSearchHumanTaskDefOperation };
var UpdatePresentationElementOperation = (function () {
    function UpdatePresentationElementOperation(id, presentationElement) {
        this.id = id;
        this.presentationElement = presentationElement;
        this.type = ActionTypes.UPDATE_PRESENTATIONELEMENT;
    }
    return UpdatePresentationElementOperation;
}());
export { UpdatePresentationElementOperation };
var CompleteUpdatePresentationElementOperation = (function () {
    function CompleteUpdatePresentationElementOperation(presentationElement) {
        this.presentationElement = presentationElement;
        this.type = ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT;
    }
    return CompleteUpdatePresentationElementOperation;
}());
export { CompleteUpdatePresentationElementOperation };
//# sourceMappingURL=humantaskdef.actions.js.map