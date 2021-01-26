import { Action } from '@ngrx/store';
import { SearchNotificationDefsResult } from '../models/searchnotificationdef.model';
import { NotificationDefinition } from '../models/notificationdef.model';
import { Parameter } from '../../common/parameter.model';
import { PresentationParameter } from '../../common/presentationparameter.model';
import { PresentationElement } from '../../common/presentationelement.model';
import { PeopleAssignment } from '../../common/people-assignment.model';

export enum ActionTypes {
    SEARCH_NOTIFICATIONDEFS = "[NotificationDef] SEARCH_NOTIFICATIONDEFS",
    COMPLETE_SEARCH_NOTIFICATIONDEFS = "[NotificationDef] COMPLETE_SEARCH_NOTIFICATIONDEFS",
    ERROR_SEARCH_NOTIFICATIONDEFS = "[NotificationDef] ERROR_SEARCH_NOTIFICATIONDEFS",
    ADD_NOTIFICATIONDEF = "[NotificationDef] ADD_NOTIFICATIONDEF",
    COMPLETE_ADD_NOTIFICATIONDEF = "[NotificationDef] COMPLETE_ADD_NOTIFICATIONDEF",
    ERROR_ADD_NOTIFICATIONDEF = "[NotificationDef] ERROR_ADD_NOTIFICATIONDEF",
    GET_NOTIFICATIONDEF = "[NotificationDef] GET_NOTIFICATIONDEF",
    COMPLETE_GET_NOTIFICATIONDEF = "[NotificationDef] COMPLETE_GET_NOTIFICATIONDEF",
    ERROR_GET_NOTIFICATIONDEF = "[NotificationDef] ERROR_GET_NOTIFICATIONDEF",
    UPDATE_NOTIFICATION_INFO = "[NotificationDef] UPDATE_NOTIFICATION_INFO",
    COMPLETE_UPDATE_NOTIFICATION_INFO = "[NotificationDef] COMPLETE_UPDATE_NOTIFICATION_INFO",
    ERROR_UPDATE_NOTIFICATION_INFO = "[NotificationDef] ERROR_UPDATE_NOTIFICATION_INFO",
    ADD_OPERATION_PARAMETER = "[NotificationDef] ADD_OPERATION_PARAMETER",
    COMPLETE_ADD_OPERATION_PARAMETER = "[NotificationDef] COMPLETE_ADD_OPERATION_PARAMETER",
    ERROR_ADD_OPERATION_PARAMETER = "[NotificationDef] ERROR_ADD_OPERATION_PARAMETER",
    DELETE_OPERATION_PARAMETER = "[NotificationDef] DELETE_OPERATION_PARAMETER",
    COMPLETE_DELETE_OPERATION_PARAMETER = "[NotificationDef] COMPLETE_DELETE_OPERATION_PARAMETER",
    ERROR_DELETE_OPERATION_PARAMETER = "[NotificationDef] ERROR_DELETE_OPERATION_PARAMETER",
    ADD_PRESENTATION_PARAMETER = "[NotificationDef] ADD_PRESENTATION_PARAMETER",
    COMPLETE_ADD_PRESENTATION_PARAMETER = "[NotificationDef] COMPLETE_ADD_PRESENTATION_PARAMETER",
    DELETE_PRESENTATION_PARAMETER = "[NotificationDef] DELETE_PRESENTATION_PARAMETER",
    COMPLETE_DELETE_PRESENTATION_PARAMETER = "[NotificationDef] COMPLETE_DELETE_PRESENTATION_PARAMETER",
    ERROR_ADD_PRESENTATION_PARAMETER = "[NotificationDef] ERROR_ADD_PRESENTATION_PARAMETER",
    ERROR_DELETE_PRESENTATION_PARAMETER = "[NotificationDef] ERROR_DELETE_PRESENTATION_PARAMETER",
    ADD_PRESENTATION_ELT = "[NotificationDef] ADD_PRESENTATION_ELT",
    COMPLETE_ADD_PRESENTATION_ELT = "[NotificationDef] COMPLETE_ADD_PRESENTATION_ELT",
    ERROR_ADD_PRESENTATION_ELT = "[NotificationDef] ERROR_ADD_PRESENTATION_ELT",
    DELETE_PRESENTATION_ELT = "[NotificationDef] DELETE_PRESENTATION_ELT",
    COMPLETE_DELETE_PRESENTATION_ELT = "[NotificationDef] COMPLETE_DELETE_PRESENTATION_ELT",
    ERROR_DELETE_PRESENTATION_ELT = "[NotificationDef] ERROR_DELETE_PRESENTATION_ELT",
    ADD_PEOPLE_ASSIGNMENT = "[NotificationDef] ADD_PEOPLE_ASSIGNMENT",
    COMPLETE_ADD_PEOPLE_ASSIGNMENT = "[NotificationDef] COMPLETE_ADD_PEOPLE_ASSIGNMENT",
    ERROR_ADD_PEOPLE_ASSIGNMENT = "[NotificationDef] ERROR_ADD_PEOPLE_ASSIGNMENT",
    DELETE_PEOPLE_ASSIGNMENT = "[NotificationDef] DELETE_PEOPLE_ASSIGNMENT",
    COMPLETE_DELETE_PEOPLE_ASSIGNMENT = "[NotificationDef] COMPLETE_DELETE_PEOPLE_ASSIGNMENT",
    ERROR_DELETE_PEOPLE_ASSIGNMENT = "[NotificationDef] ERROR_DELETE_PEOPLE_ASSIGNMENT"
}

export class SearchNotificationDefOperation implements Action {
    readonly type = ActionTypes.SEARCH_NOTIFICATIONDEFS;
    constructor(public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearchNotificationDefOperation implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_NOTIFICATIONDEFS;
    constructor(public notificationDefsResult: SearchNotificationDefsResult) { }
}

export class AddNotificationDefOperation implements Action {
    readonly type = ActionTypes.ADD_NOTIFICATIONDEF;
    constructor(public name: string) { }
}

export class CompleteAddNotificationDefOperation implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_NOTIFICATIONDEF;
    constructor(public name: string) { }
}

export class GetNotificationOperation implements Action {
    readonly type = ActionTypes.GET_NOTIFICATIONDEF;
    constructor(public id: string) { }
}

export class CompleteGetNotificationOperation implements Action {
    readonly type = ActionTypes.COMPLETE_GET_NOTIFICATIONDEF;
    constructor(public notification: NotificationDefinition) { }
}

export class UpdateNotificationInfo implements Action {
    readonly type = ActionTypes.UPDATE_NOTIFICATION_INFO;
    constructor(public id: string, public name: string, public priority: number) { }
}

export class CompleteUpdateNotificationInfo implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_NOTIFICATION_INFO;
    constructor(public name: string, public priority: number) { }
}

export class AddOperationParameterOperation implements Action {
    readonly type = ActionTypes.ADD_OPERATION_PARAMETER;
    constructor(public id: string, public parameter: Parameter) { }
}

export class CompleteAddOperationParameterOperation implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_OPERATION_PARAMETER;
    constructor(public parameter: Parameter, public id: string) { }
}

export class DeleteOperationParameterOperation implements Action {
    readonly type = ActionTypes.DELETE_OPERATION_PARAMETER;
    constructor(public id: string, public parameterId: string) { }
}

export class CompleteDeleteOperationParameterOperation implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_OPERATION_PARAMETER;
    constructor(public parameterId: string) { }
}

export class AddPresentationParameter implements Action {
    readonly type = ActionTypes.ADD_PRESENTATION_PARAMETER;
    constructor(public id: string, public presentationParameter: PresentationParameter) { }
}

export class CompleteAddPresentationParameter implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_PRESENTATION_PARAMETER;
    constructor(public presentationParameter: PresentationParameter) { }
}

export class DeletePresentationParameter implements Action {
    readonly type = ActionTypes.DELETE_PRESENTATION_PARAMETER;
    constructor(public id: string, public presentationParameter: PresentationParameter) { }
}

export class CompleteDeletePresentationParameter implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_PRESENTATION_PARAMETER;
    constructor(public presentationParameter: PresentationParameter) { }
}

export class AddPresentationElt implements Action {
    readonly type = ActionTypes.ADD_PRESENTATION_ELT;
    constructor(public id: string, public presentationElt: PresentationElement) { }
}

export class CompleteAddPresentationElt implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_PRESENTATION_ELT;
    constructor(public presentationElt: PresentationElement) { }
}

export class DeletePresentationElt implements Action {
    readonly type = ActionTypes.DELETE_PRESENTATION_ELT;
    constructor(public id: string, public presentationElt: PresentationElement) { }
}

export class CompleteDeletePresentationElt implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_PRESENTATION_ELT;
    constructor(public presentationElt: PresentationElement) { }
}

export class AddPeopleAssignment implements Action {
    readonly type = ActionTypes.ADD_PEOPLE_ASSIGNMENT;
    constructor(public id: string, public peopleAssignment: PeopleAssignment) { }
}

export class CompleteAddPeopleAssigment implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_PEOPLE_ASSIGNMENT;
    constructor(public peopleAssignment: PeopleAssignment, public assignmentId: string) { }
}

export class DeletePeopleAssignment implements Action {
    readonly type = ActionTypes.DELETE_PEOPLE_ASSIGNMENT;
    constructor(public id: string, public peopleAssignment: PeopleAssignment) { }
}

export class CompleteDeletePeopleAssignment implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_PEOPLE_ASSIGNMENT;
    constructor(public id: string, public peopleAssignment: PeopleAssignment) { }
}

export type ActionsUnion =
    SearchNotificationDefOperation |
    CompleteSearchNotificationDefOperation |
    AddNotificationDefOperation |
    CompleteAddNotificationDefOperation |
    GetNotificationOperation |
    CompleteGetNotificationOperation |
    UpdateNotificationInfo |
    CompleteUpdateNotificationInfo |
    AddOperationParameterOperation |
    CompleteAddOperationParameterOperation |
    DeleteOperationParameterOperation |
    CompleteDeleteOperationParameterOperation |
    AddPresentationParameter |
    CompleteAddPresentationParameter |
    DeletePresentationParameter |
    CompleteDeletePresentationParameter |
    AddPresentationElt |
    CompleteAddPresentationElt |
    DeletePresentationElt |
    CompleteDeletePresentationElt |
    AddPeopleAssignment |
    CompleteAddPeopleAssigment |
    DeletePeopleAssignment |
    CompleteDeletePeopleAssignment;