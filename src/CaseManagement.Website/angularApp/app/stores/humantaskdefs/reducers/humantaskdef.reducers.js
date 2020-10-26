var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __spreadArrays = (this && this.__spreadArrays) || function () {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};
import * as fromActions from '../actions/humantaskdef.actions';
import { Escalation } from '../../common/escalation.model';
export var initialHumanTaskDefState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialHumanTaskDefsState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function humanTaskDefReducer(state, action) {
    if (state === void 0) { state = initialHumanTaskDefState; }
    var id = 0;
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_HUMANTASKDEF:
            state.content = action.content;
            return __assign({}, state);
        case fromActions.ActionTypes.COMPLETE_UPDATE_HUMANASKDEF:
            state.content = action.content;
            return __assign({}, state);
        case fromActions.ActionTypes.COMPLETE_ADD_START_DEADLINE:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { startDeadLines: __spreadArrays(state.content.deadLines.startDeadLines, [
                            action.content
                        ]) }) }) });
        case fromActions.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { completionDeadLines: __spreadArrays(state.content.deadLines.completionDeadLines, [
                            action.content
                        ]) }) }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { name: action.name, priority: action.priority }) });
        case fromActions.ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { operation: __assign(__assign({}, state.content.operation), { inputParameters: __spreadArrays(state.content.operation.inputParameters, [
                            action.parameter
                        ]) }) }) });
        case fromActions.ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { operation: __assign(__assign({}, state.content.operation), { outputParameters: __spreadArrays(state.content.operation.outputParameters, [
                            action.parameter
                        ]) }) }) });
        case fromActions.ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER:
            var inputParameters = state.content.operation.inputParameters;
            var param1 = inputParameters.filter(function (p) {
                return p.name === action.name;
            })[0];
            var index1 = inputParameters.indexOf(param1);
            inputParameters.splice(index1, 1);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { operation: __assign(__assign({}, state.content.operation), { inputParameters: __spreadArrays(inputParameters) }) }) });
        case fromActions.ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER:
            var outputParameters = state.content.operation.outputParameters;
            var param2 = outputParameters.filter(function (p) {
                return p.name === action.name;
            })[0];
            var index2 = outputParameters.indexOf(param2);
            outputParameters.splice(index2, 1);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { operation: __assign(__assign({}, state.content.operation), { outputParameters: __spreadArrays(outputParameters) }) }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { rendering: __assign({}, action.rendering) }) });
        case fromActions.ActionTypes.DELETE_START_DEADLINE:
            var startDeadLines = state.content.deadLines.startDeadLines;
            var filteredStartDealine = startDeadLines.filter(function (p) {
                return p.id === action.deadLineId;
            })[0];
            var startDealineIndex = startDeadLines.indexOf(filteredStartDealine);
            startDeadLines.splice(startDealineIndex, 1);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { startDeadLines: __spreadArrays(startDeadLines) }) }) });
        case fromActions.ActionTypes.DELETE_COMPLETION_DEADLINE:
            var completionDeadLines = state.content.deadLines.completionDeadLines;
            var filteredCompletionDeadline = completionDeadLines.filter(function (p) {
                return p.id === action.deadLineId;
            })[0];
            var completionDealineIndex = completionDeadLines.indexOf(filteredCompletionDeadline);
            completionDeadLines.splice(completionDealineIndex, 1);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { completionDeadLines: __spreadArrays(completionDeadLines) }) }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_START_DEADLINE:
            var startDealines2 = state.content.deadLines.startDeadLines;
            var filteredStartedDeadline2 = startDealines2.filter(function (p) {
                return p.id === action.deadline.id;
            })[0];
            var startDealineIndex2 = startDealines2.indexOf(filteredStartedDeadline2);
            startDealines2.splice(startDealineIndex2, 1);
            startDealines2.push(action.deadline);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { startDeadLines: __spreadArrays(startDealines2) }) }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_DEADLINE:
            var completionDealines2 = state.content.deadLines.completionDeadLines;
            var filteredCompletionDeadline2 = completionDealines2.filter(function (p) {
                return p.id === action.deadline.id;
            })[0];
            var completionDealineIndex2 = completionDealines2.indexOf(filteredCompletionDeadline2);
            completionDealines2.splice(completionDealineIndex2, 1);
            completionDealines2.push(action.deadline);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { completionDeadLines: __spreadArrays(completionDealines2) }) }) });
        case fromActions.ActionTypes.COMPLETE_ADD_ESCALATION_STARTDEADLINE:
            var startDealine3 = state.content.deadLines.startDeadLines.filter(function (p) {
                return p.id === action.deadlineId;
            })[0];
            var escalation1 = new Escalation();
            escalation1.id = action.escId;
            escalation1.condition = action.condition;
            startDealine3.escalations.push(escalation1);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { startDeadLines: __spreadArrays(state.content.deadLines.startDeadLines) }) }) });
        case fromActions.ActionTypes.COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE:
            var completionDealine3 = state.content.deadLines.completionDeadLines.filter(function (p) {
                return p.id === action.deadlineId;
            })[0];
            var escalation2 = new Escalation();
            escalation2.id = action.escId;
            escalation2.condition = action.condition;
            completionDealine3.escalations.push(escalation2);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { completionDeadLines: __spreadArrays(state.content.deadLines.completionDeadLines) }) }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { peopleAssignment: action.assignment }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_ESCALATION:
            var dl = state.content.deadLines.completionDeadLines.filter(function (p) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc) {
                return esc.id === action.escalation.id;
            })[0];
            var i = dl.escalations.indexOf(esc);
            dl.escalations.splice(i, 1);
            dl.escalations.push(action.escalation);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { completionDeadLines: __spreadArrays(state.content.deadLines.completionDeadLines) }) }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_START_ESCALATION:
            var dl = state.content.deadLines.startDeadLines.filter(function (p) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc) {
                return esc.id === action.escalation.id;
            })[0];
            id = dl.escalations.indexOf(esc);
            dl.escalations.splice(id, 1);
            dl.escalations.push(action.escalation);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { startDeadLines: __spreadArrays(state.content.deadLines.startDeadLines) }) }) });
        case fromActions.ActionTypes.COMPLETE_DELETE_START_ESCALATION:
            var dl = state.content.deadLines.startDeadLines.filter(function (p) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc) {
                return esc.id === action.escalation.id;
            })[0];
            id = dl.escalations.indexOf(esc);
            dl.escalations.splice(id, 1);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { startDeadLines: __spreadArrays(state.content.deadLines.startDeadLines) }) }) });
        case fromActions.ActionTypes.COMPLETE_DELETE_COMPLETION_ESCALATION:
            var dl = state.content.deadLines.completionDeadLines.filter(function (p) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc) {
                return esc.id === action.escalation.id;
            })[0];
            id = dl.escalations.indexOf(esc);
            dl.escalations.splice(id, 1);
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { completionDeadLines: __spreadArrays(state.content.deadLines.completionDeadLines) }) }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { presentationElementResult: __assign({}, action.presentationElement) }) });
        default:
            return state;
    }
}
;
export function humanTaskDefsReducer(state, action) {
    if (state === void 0) { state = initialHumanTaskDefsState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_HUMANTASKDEFS:
            return __assign(__assign({}, state), { content: action.humanTaskDefsResult });
    }
}
;
//# sourceMappingURL=humantaskdef.reducers.js.map