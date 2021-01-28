import { Parameter } from '../../common/parameter.model';
import * as fromActions from '../actions/notificationdef.actions';
import { NotificationDefinition } from '../models/notificationdef.model';
import { SearchNotificationDefsResult } from '../models/searchnotificationdef.model';
import { PresentationParameter } from '../../common/presentationparameter.model';
import { PresentationElement } from '../../common/presentationelement.model';
import { PeopleAssignment } from '../../common/people-assignment.model';

export interface NotificationDefState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: NotificationDefinition;
}

export interface SearchNotificationDefState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchNotificationDefsResult;
}

export interface NotificationDefLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: NotificationDefinition[];
}

export const initialNotificationDefState: NotificationDefState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialNotificationDefsState: SearchNotificationDefState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialNotificationDefLstState: NotificationDefLstState = {
    content: [],
    isLoading: true,
    isErrorLoadOccured: false
};

export function notificationDefReducer(state = initialNotificationDefState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_NOTIFICATIONDEF:
            state.content = action.notification;
            return { ...state };
        case fromActions.ActionTypes.COMPLETE_UPDATE_NOTIFICATION_INFO:
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
        default:
            return state;
    }
};

export function notificationDefsReducer(state = initialNotificationDefsState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_NOTIFICATIONDEFS:
            return {
                ...state,
                content: action.notificationDefsResult
            };
        default:
            return state;
    }
};

export function notificationDefLstReducer(state = initialNotificationDefLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_ALL:
            return {
                ...state,
                content: action.notificationDefs
            };
        default:
            return state;
    }
}