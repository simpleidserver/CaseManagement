import { Escalation } from '../../common/escalation.model';
import { Parameter } from '../../common/parameter.model';
import * as fromActions from '../actions/humantaskdef.actions';
import { HumanTaskDef } from '../models/humantaskdef.model';
import { SearchHumanTaskDefsResult } from '../models/searchhumantaskdef.model';
import { Deadline } from '../models/deadline';

export interface HumanTaskDefState{
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: HumanTaskDef;
}

export interface SearchHumanTaskDefState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchHumanTaskDefsResult;
}

export const initialHumanTaskDefState: HumanTaskDefState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialHumanTaskDefsState: SearchHumanTaskDefState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function humanTaskDefReducer(state = initialHumanTaskDefState, action: fromActions.ActionsUnion) {
    let id: number = 0;
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_HUMANTASKDEF:
            state.content = action.content;
            return { ...state };
        case fromActions.ActionTypes.COMPLETE_UPDATE_HUMANASKDEF:
            state.content = action.content;
            return { ...state };
        case fromActions.ActionTypes.COMPLETE_ADD_START_DEADLINE:
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...state.content.deadLines,
                        action.content
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE:
            return { 
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...state.content.deadLines,
                        action.content
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO:
            return {
                ...state,
                content: {
                    ...state.content,
                    name: action.name,
                    priority: action.priority
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER:
            return {
                ...state,
                content: {
                    ...state.content,
                    operationParameters: [
                        ...state.content.operationParameters,
                        action.parameter
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER:
            return {
                ...state,
                content: {
                    ...state.content,
                    operationParameters: [
                        ...state.content.operationParameters,
                        action.parameter
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER:
            var inputParameters = state.content.operationParameters;
            const param1 = inputParameters.filter(function (p: Parameter) {
                return p.name === action.name && p.usage === 'INPUT';
            })[0];
            const index1 = inputParameters.indexOf(param1);
            inputParameters.splice(index1, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    operationParameters: [
                        ...inputParameters
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER:
            var outputParameters = state.content.operationParameters;
            const param2 = outputParameters.filter(function (p: Parameter) {
                return p.name === action.name && p.usage === 'OUTPUT';
            })[0];
            const index2 = outputParameters.indexOf(param2);
            outputParameters.splice(index2, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    operationParameters: [
                        ...outputParameters
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER:
            return {
                ...state,
                content: {
                    ...state.content,
                    renderingElements: [
                        ...action.renderingElements
                    ]
                }
            };
        case fromActions.ActionTypes.DELETE_START_DEADLINE:
            var startDeadLines = state.content.deadLines;
            const filteredStartDealine = startDeadLines.filter(function (p: Deadline) {
                return p.id === action.deadLineId && p.usage === 'START';
            })[0];
            const startDealineIndex = startDeadLines.indexOf(filteredStartDealine);
            startDeadLines.splice(startDealineIndex, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...startDeadLines
                    ]
                }
            };
        case fromActions.ActionTypes.DELETE_COMPLETION_DEADLINE:
            var completionDeadLines = state.content.deadLines;
            const filteredCompletionDeadline = completionDeadLines.filter(function (p: Deadline) {
                return p.id === action.deadLineId && p.usage === 'COMPLETION';
            })[0];
            const completionDealineIndex = completionDeadLines.indexOf(filteredCompletionDeadline);
            completionDeadLines.splice(completionDealineIndex, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...completionDeadLines
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_START_DEADLINE:
            var startDealines2 = state.content.deadLines;
            const filteredStartedDeadline2 = startDealines2.filter(function (p: Deadline) {
                return p.id === action.deadline.id && p.usage === 'START';
            })[0];
            const startDealineIndex2 = startDealines2.indexOf(filteredStartedDeadline2);
            startDealines2.splice(startDealineIndex2, 1);
            startDealines2.push(action.deadline);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...startDealines2
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_DEADLINE:
            var completionDealines2 = state.content.deadLines;
            const filteredCompletionDeadline2 = completionDealines2.filter(function (p: Deadline) {
                return p.id === action.deadline.id && p.usage === 'COMPLETION';
            })[0];
            const completionDealineIndex2 = completionDealines2.indexOf(filteredCompletionDeadline2);
            completionDealines2.splice(completionDealineIndex2, 1);
            completionDealines2.push(action.deadline);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...completionDealines2
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_ESCALATION_STARTDEADLINE:
            var startDealine3 = state.content.deadLines.filter(function (p: Deadline) {
                return p.id === action.deadlineId;
            })[0];
            const escalation1 = new Escalation();
            escalation1.id = action.escId;
            escalation1.condition = action.condition;
            startDealine3.escalations.push(escalation1);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...state.content.deadLines
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE:
            var completionDealine3 = state.content.deadLines.filter(function (p: Deadline) {
                return p.id === action.deadlineId;
            })[0];
            const escalation2 = new Escalation();
            escalation2.id = action.escId;
            escalation2.condition = action.condition;
            completionDealine3.escalations.push(escalation2);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...state.content.deadLines
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT:
            return {
                ...state,
                content: {
                    ...state.content,
                    peopleAssignments: action.peopleAssignments
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_ESCALATION:
            var dl = state.content.deadLines.filter(function (p: Deadline) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc: Escalation) {
                return esc.id === action.escalation.id;
            })[0];
            const i = dl.escalations.indexOf(esc);
            dl.escalations.splice(i, 1);
            dl.escalations.push(action.escalation);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...state.content.deadLines
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_START_ESCALATION:
            var dl = state.content.deadLines.filter(function (p: Deadline) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc: Escalation) {
                return esc.id === action.escalation.id;
            })[0];
            id = dl.escalations.indexOf(esc);
            dl.escalations.splice(id, 1);
            dl.escalations.push(action.escalation);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...state.content.deadLines
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_START_ESCALATION:
            var dl = state.content.deadLines.filter(function (p: Deadline) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc: Escalation) {
                return esc.id === action.escalation.id;
            })[0];
            id = dl.escalations.indexOf(esc);
            dl.escalations.splice(id, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...state.content.deadLines
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_COMPLETION_ESCALATION:
            var dl = state.content.deadLines.filter(function (p: Deadline) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc: Escalation) {
                return esc.id === action.escalation.id;
            })[0];
            id = dl.escalations.indexOf(esc);
            dl.escalations.splice(id, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...state.content.deadLines
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT:
            return {
                ...state,
                content: {
                    ...state.content,
                    presentationElements: [...action.presentationElements],
                    presentationParameters: [ ...action.presentationParameters]
                }
            };
        default:
            return state;
    }
};

export function humanTaskDefsReducer(state = initialHumanTaskDefsState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_HUMANTASKDEFS:
            return {
                ...state,
                content: action.humanTaskDefsResult
            };
    }
};