import { Escalation } from '../../common/escalation.model';
import { Parameter } from '../../common/parameter.model';
import { PeopleAssignment } from '../../common/people-assignment.model';
import { PresentationElement } from '../../common/presentationelement.model';
import { PresentationParameter } from '../../common/presentationparameter.model';
import { ToPart } from '../../common/topart.model';
import * as fromActions from '../actions/humantaskdef.actions';
import { Deadline } from '../models/deadline';
import { HumanTaskDef } from '../models/humantaskdef.model';
import { SearchHumanTaskDefsResult } from '../models/searchhumantaskdef.model';

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
        case fromActions.ActionTypes.COMPLETE_ADD_DEADLINE:
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
        case fromActions.ActionTypes.COMPLETE_ADD_OPERATION_PARAMETER:
            action.parameter.id = action.id;
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
        case fromActions.ActionTypes.COMPLETE_DELETE_OPERATION_PARAMETER:
            {
                const inputParameters = state.content.operationParameters;
                const param1 = inputParameters.filter(function (p: Parameter) {
                    return p.id === action.parameterId;
                })[0];
                const index1 = inputParameters.indexOf(param1);
                inputParameters.splice(index1, 1);
                const result = {
                    ...state,
                    content: {
                        ...state.content,
                        operationParameters: [
                            ...inputParameters
                        ]
                    }
                };
                return result;
            }
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
        case fromActions.ActionTypes.DELETE_DEADLINE:
            var startDeadLines = state.content.deadLines;
            const filteredStartDealine = startDeadLines.filter(function (p: Deadline) {
                return p.id === action.deadLineId;
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
        case fromActions.ActionTypes.COMPLETE_UPDATE_DEADLINE:
            var deadlines = state.content.deadLines;
            const deadline = deadlines.filter(function (p: Deadline) {
                return p.id === action.deadline.id;
            })[0];
            deadline.for = action.deadline.for;
            deadline.until = action.deadline.until;
            deadline.name = action.deadline.name;
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...deadlines
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_ESCALATION_DEADLINE:
            var startDealine3 = state.content.deadLines.filter(function (p: Deadline) {
                return p.id === action.deadlineId;
            })[0];
            const escalation1 = new Escalation();
            escalation1.id = action.escId;
            escalation1.condition = action.condition;
            escalation1.notificationId = action.notificationId;
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
        case fromActions.ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT:
            return {
                ...state,
                content: {
                    ...state.content,
                    peopleAssignments: action.peopleAssignments
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_ESCALATION:
            var dl = state.content.deadLines.filter(function (p: Deadline) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc: Escalation) {
                return esc.id === action.escalationId;
            })[0];
            esc.condition = action.condition;
            esc.notificationId = action.notificationId;
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: [
                        ...state.content.deadLines
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_ESCALATION:
            var dl = state.content.deadLines.filter(function (p: Deadline) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc: Escalation) {
                return esc.id === action.escalationId;
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
        case fromActions.ActionTypes.COMPLETE_ADD_PRESENTATION_PARAMETER:
            return {
                ...state,
                content: {
                    ...state.content,
                    presentationParameters: [
                        ...state.content.presentationParameters,
                        action.presentationParameter
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_PRESENTATION_PARAMETER:
            {
                var pp = state.content.presentationParameters.filter(function (p: PresentationParameter) {
                    return p.name === action.presentationParameter.name;
                })[0];
                const id = state.content.presentationParameters.indexOf(pp);
                state.content.presentationParameters.splice(id, 1);
                return {
                    ...state,
                    content: {
                        ...state.content,
                        presentationParameters: [
                            ...state.content.presentationParameters
                        ]
                    }
                };
            }
        case fromActions.ActionTypes.COMPLETE_ADD_PRESENTATION_ELT:
            return {
                ...state,
                content: {
                    ...state.content,
                    presentationElements: [
                        ...state.content.presentationElements,
                        action.presentationElt
                    ]
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_PRESENTATION_ELT:
            {
                const pe = state.content.presentationElements.filter(function (p: PresentationElement) {
                    return p.usage === action.presentationElt.usage && p.language === action.presentationElt.language;
                })[0];
                const id = state.content.presentationElements.indexOf(pe);
                state.content.presentationElements.splice(id, 1);
                return {
                    ...state,
                    content: {
                        ...state.content,
                        presentationElements: [
                            ...state.content.presentationElements
                        ]
                    }
                };
            }
            break;
        case fromActions.ActionTypes.COMPLETE_ADD_PEOPLE_ASSIGNMENT:
            {
                action.peopleAssignment.id = action.assignmentId;
                return {
                    ...state,
                    content: {
                        ...state.content,
                        peopleAssignments: [
                            ...state.content.peopleAssignments,
                            action.peopleAssignment
                        ]
                    }
                };
            }
            break;
        case fromActions.ActionTypes.COMPLETE_DELETE_PEOPLE_ASSIGNMENT:
            {
                const pe = state.content.peopleAssignments.filter(function (p: PeopleAssignment) {
                    return p.id === action.peopleAssignment.id;
                })[0];
                const id = state.content.peopleAssignments.indexOf(pe);
                state.content.peopleAssignments.splice(id, 1);
                return {
                    ...state,
                    content: {
                        ...state.content,
                        peopleAssignments: [
                            ...state.content.peopleAssignments
                        ]
                    }
                };
            }
            break;
        case fromActions.ActionTypes.COMPLETE_ADD_ESCALATION_TOPART:
            {
                const dl = state.content.deadLines.filter(function (d: Deadline) {
                    return d.id === action.deadlineId;
                })[0];
                const esc = dl.escalations.filter(function (e: Escalation) {
                    return e.id === action.escalationId;
                })[0];
                esc.toParts.push(action.toPart);
                return {
                    ...state,
                    content: {
                        ...state.content,
                        deadLines: [
                            ...state.content.deadLines
                        ]
                    }
                };
            }
            break;
        case fromActions.ActionTypes.COMPLETE_DELETE_ESCALATION_TOPART:
            {
                const dl = state.content.deadLines.filter(function (d: Deadline) {
                    return d.id === action.deadlineId;
                })[0];
                const esc = dl.escalations.filter(function (e: Escalation) {
                    return e.id === action.escalationId;
                })[0];
                const tp = esc.toParts.filter(function (t: ToPart) {
                    return t.name === action.toPartName;
                })[0];
                const id = esc.toParts.indexOf(tp);
                esc.toParts.splice(id, 1);
                return {
                    ...state,
                    content: {
                        ...state.content,
                        deadLines: [
                            ...state.content.deadLines
                        ]
                    }
                };
            }
            break;
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