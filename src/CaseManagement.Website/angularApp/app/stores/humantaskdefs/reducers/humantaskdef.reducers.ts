import { Parameter } from '../../common/operation.model';
import * as fromActions from '../actions/humantaskdef.actions';
import { HumanTaskDef } from '../models/humantaskdef.model';
import { HumanTaskDefinitionDeadLine } from '../models/humantaskdef-deadlines';
import { Escalation } from '../../common/escalation.model';

export interface HumanTaskDefState{
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: HumanTaskDef;
}

export const initialHumanTaskDefState: HumanTaskDefState = {
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
                    deadLines: {
                        ...state.content.deadLines,
                        startDeadLines: [
                            ...state.content.deadLines.startDeadLines,
                            action.content
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE:
            return { 
                ...state,
                content: {
                    ...state.content,
                    deadLines: {
                        ...state.content.deadLines,
                        completionDeadLines: [
                            ...state.content.deadLines.completionDeadLines,
                            action.content
                        ]
                    }
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
                    operation: {
                        ...state.content.operation,
                        inputParameters: [
                            ...state.content.operation.inputParameters,
                            action.parameter
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER:
            return {
                ...state,
                content: {
                    ...state.content,
                    operation: {
                        ...state.content.operation,
                        outputParameters: [
                            ...state.content.operation.outputParameters,
                            action.parameter
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER:
            var inputParameters = state.content.operation.inputParameters;
            const param1 = inputParameters.filter(function (p: Parameter) {
                return p.name === action.name;
            })[0];
            const index1 = inputParameters.indexOf(param1);
            inputParameters.splice(index1, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    operation: {
                        ...state.content.operation,
                        inputParameters: [
                            ...inputParameters
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER:
            var outputParameters = state.content.operation.outputParameters;
            const param2 = outputParameters.filter(function (p: Parameter) {
                return p.name === action.name;
            })[0];
            const index2 = outputParameters.indexOf(param2);
            outputParameters.splice(index2, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    operation: {
                        ...state.content.operation,
                        outputParameters: [
                            ...outputParameters
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER:
            return {
                ...state,
                content: {
                    ...state.content,
                    rendering: {
                        ...action.rendering
                    }
                }
            };
        case fromActions.ActionTypes.DELETE_START_DEADLINE:
            var startDeadLines = state.content.deadLines.startDeadLines;
            const filteredStartDealine = startDeadLines.filter(function (p: HumanTaskDefinitionDeadLine) {
                return p.name === action.name;
            })[0];
            const startDealineIndex = startDeadLines.indexOf(filteredStartDealine);
            startDeadLines.splice(startDealineIndex, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: {
                        ...state.content.deadLines,
                        startDeadLines: [
                            ...startDeadLines
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.DELETE_COMPLETION_DEADLINE:
            var completionDeadLines = state.content.deadLines.completionDeadLines;
            const filteredCompletionDeadline = completionDeadLines.filter(function (p: HumanTaskDefinitionDeadLine) {
                return p.name === action.name;
            })[0];
            const completionDealineIndex = completionDeadLines.indexOf(filteredCompletionDeadline);
            completionDeadLines.splice(completionDealineIndex, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: {
                        ...state.content.deadLines,
                        completionDeadLines: [
                            ...completionDeadLines
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_START_DEADLINE:
            var startDealines2 = state.content.deadLines.startDeadLines;
            const filteredStartedDeadline2 = startDealines2.filter(function (p: HumanTaskDefinitionDeadLine) {
                return p.id === action.deadline.id;
            })[0];
            const startDealineIndex2 = startDealines2.indexOf(filteredStartedDeadline2);
            startDealines2.splice(startDealineIndex2, 1);
            startDealines2.push(action.deadline);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: {
                        ...state.content.deadLines,
                        startDeadLines: [
                            ...startDealines2
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_DEADLINE:
            var completionDealines2 = state.content.deadLines.completionDeadLines;
            const filteredCompletionDeadline2 = completionDealines2.filter(function (p: HumanTaskDefinitionDeadLine) {
                return p.id === action.deadline.id;
            })[0];
            const completionDealineIndex2 = completionDealines2.indexOf(filteredCompletionDeadline2);
            completionDealines2.splice(completionDealineIndex2, 1);
            completionDealines2.push(action.deadline);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: {
                        ...state.content.deadLines,
                        completionDeadLines: [
                            ...completionDealines2
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_ESCALATION_STARTDEADLINE:
            var startDealine3 = state.content.deadLines.startDeadLines.filter(function (p: HumanTaskDefinitionDeadLine) {
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
                    deadLines: {
                        ...state.content.deadLines,
                        startDeadLines: [
                            ...state.content.deadLines.startDeadLines
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE:
            var completionDealine3 = state.content.deadLines.completionDeadLines.filter(function (p: HumanTaskDefinitionDeadLine) {
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
                    deadLines: {
                        ...state.content.deadLines,
                        completionDeadLines: [
                            ...state.content.deadLines.completionDeadLines
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT:
            return {
                ...state,
                content: {
                    ...state.content,
                    peopleAssignment: action.assignment
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_ESCALATION:
            var dl = state.content.deadLines.completionDeadLines.filter(function (p: HumanTaskDefinitionDeadLine) {
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
                    deadLines: {
                        ...state.content.deadLines,
                        completionDeadLines: [
                            ...state.content.deadLines.completionDeadLines
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_START_ESCALATION:
            var dl = state.content.deadLines.startDeadLines.filter(function (p: HumanTaskDefinitionDeadLine) {
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
                    deadLines: {
                        ...state.content.deadLines,
                        startDeadLines: [
                            ...state.content.deadLines.startDeadLines
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_START_ESCALATION:
            var dl = state.content.deadLines.startDeadLines.filter(function (p: HumanTaskDefinitionDeadLine) {
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
                    deadLines: {
                        ...state.content.deadLines,
                        startDeadLines: [
                            ...state.content.deadLines.startDeadLines
                        ]
                    }
                }
            };
        case fromActions.ActionTypes.COMPLETE_DELETE_COMPLETION_ESCALATION:
            var dl = state.content.deadLines.completionDeadLines.filter(function (p: HumanTaskDefinitionDeadLine) {
                return p.id === action.deadLineId;
            })[0];
            var esc = dl.escalations.filter(function (esc: Escalation) {
                return esc.id === action.escalation.id;
            })[0];
            id = dl.escalations.indexOf(esc);
            console.log("BINGO");
            dl.escalations.splice(id, 1);
            return {
                ...state,
                content: {
                    ...state.content,
                    deadLines: {
                        ...state.content.deadLines,
                        completionDeadLines: [
                            ...state.content.deadLines.completionDeadLines
                        ]
                    }
                }
            };
        default:
            return state;
    }
};