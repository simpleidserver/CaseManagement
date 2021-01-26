import { Action } from '@ngrx/store';
import { Escalation } from '../../common/escalation.model';
import { Parameter } from '../../common/parameter.model';
import { PeopleAssignment } from '../../common/people-assignment.model';
import { PresentationElement } from '../../common/presentationelement.model';
import { RenderingElement } from '../../common/rendering.model';
import { Deadline } from '../models/deadline';
import { HumanTaskDef } from '../models/humantaskdef.model';
import { SearchHumanTaskDefsResult } from '../models/searchhumantaskdef.model';
import { PresentationParameter } from '../../common/presentationparameter.model';
import { ToPart } from '../../common/topart.model';

export enum ActionTypes {
    START_GET_HUMANTASKDEF = "[HumanTaskDef] START_GET_HUMANTASKDEF",
    COMPLETE_GET_HUMANTASKDEF = "[CasePlanInstance] COMPLETE_GET_HUMANTASKDEF",
    ERROR_GET_HUMANTASKDEF = "[CasePlanInstance] ERROR_GET_HUMANTASKDEF",
    UPDATE_HUMANASKDEF = "[HumanTaskDef] UPDATE_HUMANASKDEF",
    COMPLETE_UPDATE_HUMANASKDEF = "[HumanTaskDef] COMPLETE_UPDATE_HUMANASKDEF",
    ERROR_UPDATE_HUMANASKDEF = "[HumanTaskDef] ERROR_UPDATE_HUMANASKDEF",
    ADD_DEADLINE = "[HumanTaskDef] ADD_DEADLINE",
    COMPLETE_ADD_DEADLINE = "[HumanTaskDef] COMPLETE_ADD_DEADLINE",
    ERROR_ADD_DEADLINE = "[HumanTaskDef] ERROR_ADD_DEADLINE",
    ADD_COMPLETION_DEADLINE = "[HumanTaskDef] ADD_COMPLETION_DEADLINE",
    UPDATE_HUMANTASKDEF_INFO = "[HumanTaskDef] UPDATE_HUMANTASKDEF_INFO",
    COMPLETE_UPDATE_HUMANTASK_INFO = "[HumanTaskDef] COMPLETE_UPDATE_HUMANTASK_INFO",
    ERROR_UPDATE_HUMANTASK_INFO = "[HumanTaskDef] ERROR_UPDATE_HUMANTASK_INFO",
    ADD_OPERATION_PARAMETER = "[HumanTaskDef] ADD_OPERATION_PARAMETER",
    COMPLETE_ADD_OPERATION_PARAMETER = "[HumanTaskDef] COMPLETE_ADD_OPERATION_PARAMETER",
    ERROR_ADD_OPERATION_PARAMETER = "[HumanTaskDef] ERROR_ADD_OPERATION_PARAMETER",
    DELETE_OPERATION_PARAMETER = "[HumanTaskDef] DELETE_OPERATION_PARAMETER",
    COMPLETE_DELETE_OPERATION_PARAMETER = "[HumanTaskDef] COMPLETE_DELETE_OPERATION_PARAMETER",
    ERROR_DELETE_OPERATION_PARAMETER = "[HumanTaskDef] ERROR_DELETE_OPERATION_PARAMETER",
    UPDATE_RENDERING_PARAMETER = "[HumanTaskDef] UPDATE_RENDERING_PARAMETER",
    COMPLETE_UPDATE_RENDERING_PARAMETER = "[HumanTaskDef] COMPLETE_UPDATE_RENDERING_PARAMETER",
    ERROR_UPDATE_RENDERING_PARAMETER = "[HumanTaskDef] ERROR_UPDATE_RENDERING_PARAMETER",
    DELETE_DEADLINE = "[HumanTaskDef] DELETE_DEADLINE",
    COMPLETE_DELETE_DEADLINE = "[HumanTaskDef] COMPLETE_DELETE_DEADLINE",
    ERROR_DELETE_DEADLINE = "[HumanTaskDef] ERROR_DELETE_DEADLINE",
    UPDATE_DEADLINE = "[HumanTaskDef] UPDATE_DEADLINE",
    COMPLETE_UPDATE_DEADLINE = "[HumanTaskDef] COMPLETE_UPDATE_DEADLINE",
    ERROR_UPDATE_DEADLINE = "[HumanTaskDef] ERROR_UPDATE_DEADLINE",
    ADD_ESCALATION_DEADLINE = "[HumanTaskDef] ADD_ESCALATION_DEADLINE",
    COMPLETE_ADD_ESCALATION_DEADLINE = "[HumanTaskDef] COMPLETE_ADD_ESCALATION_DEADLINE",
    ERROR_ADD_ESCALATION_DEADLINE = "[HumanTaskDef] ERROR_ADD_ESCALATION_DEADLINE",
    UPDATE_PEOPLE_ASSIGNMENT = "[HumanTaskDef] UPDATE_PEOPLE_ASSIGNMENT",
    COMPLETE_UPDATE_PEOPLE_ASSIGNMENT = "[HumanTaskDef] COMPLETE_UPDATE_PEOPLE_ASSIGNMENT",
    ERROR_UPDATE_PEOPLE_ASSIGNMENT = "[HumanTaskDef] ERROR_UPDATE_PEOPLE_ASSIGNMENT",
    UPDATE_ESCALATION = "[HumanTaskDef] UPDATE_ESCALATION",
    COMPLETE_UPDATE_ESCALATION = "[HumanTaskDef] COMPLETE_UPDATE_ESCALATION",
    ERROR_UPDATE_ESCALATION = "[HumanTaskDef] ERROR_UPDATE_ESCALATION",
    DELETE_ESCALATION = "[HumanTaskDef] DELETE_ESCALATION",
    ERROR_DELETE_ESCALATION = "[HumanTaskDef] ERROR_DELETE_ESCALATION",
    COMPLETE_DELETE_ESCALATION = "[HumanTaskDef] COMPLETE_DELETE_ESCALATION",
    ADD_HUMANTASKEF = "[HumanTaskDef] ADD_HUMANTASKDEF",
    COMPLETE_ADD_HUMANTASKDEF = "[HumanTaskDef] COMPLETE_ADD_HUMANTASKDEF",
    ERROR_ADD_HUMANTASKDEF = "[HumanTaskDef] ERROR_ADD_HUMANTASKDEF",
    SEARCH_HUMANTASKDEFS = "[HumanTaskDef] SEARCH_HUMANTASKDEFS",
    COMPLETE_SEARCH_HUMANTASKDEFS = "[HumanTaskDef] COMPLETE_SEARCH_HUMANTASKDEFS",
    ERROR_SEARCH_HUMANTASKDEFS = "[HumanTaskDef] ERROR_SEARCH_HUMANTASKDEFS",
    GET_HUMANTASKDEF = "[HumanTaskDef] GET_HUMANTASKDEF",
    UPDATE_PRESENTATIONELEMENT = "[HumanTaskDef] UPDATE_PRESENTATIONELEMENT",
    COMPLETE_UPDATE_PRESENTATIONELEMENT = "[HumanTaskDef] COMPLETE_UPDATE_PRESENTATIONELEMENT",
    ERROR_UPDATE_PRESENTATIONELEMENT = "[HumanTaskDef] ERROR_UPDATE_PRESENTATIONELEMENT",
    ADD_PRESENTATION_PARAMETER = "[HumanTaskDef] ADD_PRESENTATION_PARAMETER",
    COMPLETE_ADD_PRESENTATION_PARAMETER = "[HumanTaskDef] COMPLETE_ADD_PRESENTATION_PARAMETER",
    DELETE_PRESENTATION_PARAMETER = "[HumanTaskDef] DELETE_PRESENTATION_PARAMETER",
    COMPLETE_DELETE_PRESENTATION_PARAMETER = "[HumanTaskDef] COMPLETE_DELETE_PRESENTATION_PARAMETER",
    ERROR_ADD_PRESENTATION_PARAMETER = "[HumanTaskDef] ERROR_ADD_PRESENTATION_PARAMETER",
    ERROR_DELETE_PRESENTATION_PARAMETER = "[HumanTaskDef] ERROR_DELETE_PRESENTATION_PARAMETER",
    ADD_PRESENTATION_ELT = "[HumanTaskDef] ADD_PRESENTATION_ELT",
    COMPLETE_ADD_PRESENTATION_ELT = "[HumanTaskDef] COMPLETE_ADD_PRESENTATION_ELT",
    ERROR_ADD_PRESENTATION_ELT = "[HumanTaskDef] ERROR_ADD_PRESENTATION_ELT",
    DELETE_PRESENTATION_ELT = "[HumanTaskDef] DELETE_PRESENTATION_ELT",
    COMPLETE_DELETE_PRESENTATION_ELT = "[HumanTaskDef] COMPLETE_DELETE_PRESENTATION_ELT",
    ERROR_DELETE_PRESENTATION_ELT = "[HumanTaskDef] ERROR_DELETE_PRESENTATION_ELT",
    ADD_PEOPLE_ASSIGNMENT = "[HumanTaskDef] ADD_PEOPLE_ASSIGNMENT",
    COMPLETE_ADD_PEOPLE_ASSIGNMENT = "[HumanTaskDef] COMPLETE_ADD_PEOPLE_ASSIGNMENT",
    ERROR_ADD_PEOPLE_ASSIGNMENT = "[HumanTaskDef] ERROR_ADD_PEOPLE_ASSIGNMENT",
    DELETE_PEOPLE_ASSIGNMENT = "[HumanTaskDef] DELETE_PEOPLE_ASSIGNMENT",
    COMPLETE_DELETE_PEOPLE_ASSIGNMENT = "[HumanTaskDef] COMPLETE_DELETE_PEOPLE_ASSIGNMENT",
    ERROR_DELETE_PEOPLE_ASSIGNMENT = "[HumanTaskDef] ERROR_DELETE_PEOPLE_ASSIGNMENT",
    ADD_ESCALATION_TOPART = "[HumanTaskDef] ADD_ESCALATION_TOPART",
    COMPLETE_ADD_ESCALATION_TOPART = "[HumanTaskDef] COMPLETE_ADD_ESCALATION_TOPART",
    ERROR_ADD_ESCALATION_TOPART = "[HumanTaskDef] ERROR_ADD_ESCALATION_TOPART",
    DELETE_ESCALATION_TOPART = "[HumanTaskDef] DELETE_ESCALATION_TOPART",
    COMPLETE_DELETE_ESCALATION_TOPART = "[HumanTaskDef] COMPLETE_DELETE_ESCALATION_TOPART",
    ERROR_DELETE_ESCALATION_TOPART = "[HumanTaskDef] ERROR_DELETE_ESCALATION_TOPART"
}

export class GetHumanTaskDef implements Action {
    readonly type = ActionTypes.START_GET_HUMANTASKDEF;
    constructor(public id: string) { }
}

export class GetHumanTaskDefComplete implements Action {
    readonly type = ActionTypes.COMPLETE_GET_HUMANTASKDEF;
    constructor(public content: HumanTaskDef) { }
}

export class UpdateHumanTaskDef implements Action {
    readonly type = ActionTypes.UPDATE_HUMANASKDEF;
    constructor(public humanTaskDef: HumanTaskDef) { }
}

export class UpdateHumanTaskDefComplete implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_HUMANASKDEF;
    constructor(public content : HumanTaskDef) { }
}

export class AddDeadline implements Action {
    readonly type = ActionTypes.ADD_DEADLINE;
    constructor(public id: string, public deadLine: Deadline) { }
}

export class CompleteAddDeadLine implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_DEADLINE;
    constructor(public content: Deadline) { }
}

export class UpdateHumanTaskInfo implements Action {
    readonly type = ActionTypes.UPDATE_HUMANTASKDEF_INFO;
    constructor(public id : string, public name: string, public priority : number) { }
}

export class CompleteUpdateHumanTaskInfo implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO;
    constructor(public name: string, public priority: number) { }
}

export class AddOperationParameterOperation implements Action {
    readonly type = ActionTypes.ADD_OPERATION_PARAMETER;
    constructor(public id: string, public parameter : Parameter) { }
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

export class UpdateRenderingOperation implements Action {
    readonly type = ActionTypes.UPDATE_RENDERING_PARAMETER;
    constructor(public id: string, public renderingElements: RenderingElement[]) { }
}

export class CompleteUpdateRenderingOperation implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER;
    constructor(public renderingElements: RenderingElement[]) { }
}

export class DeleteDeadlineOperation implements Action {
    readonly type = ActionTypes.DELETE_DEADLINE;
    constructor(public id: string, public deadLineId: string) { }
}

export class CompleteDeleteDeadlineOperation implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_DEADLINE;
    constructor(public deadLineId: string) { }
}

export class UpdateDeadlineOperation implements Action {
    readonly type = ActionTypes.UPDATE_DEADLINE;
    constructor(public id: string, public deadline: Deadline) { }
}

export class CompleteUpdateDeadlineOperation implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_DEADLINE;
    constructor(public deadline: Deadline) { }
}

export class AddEscalationDeadlineOperation implements Action {
    readonly type = ActionTypes.ADD_ESCALATION_DEADLINE;
    constructor(public id: string, public deadlineId: string, public condition: string) { }
}

export class CompleteAddEscalationDeadlineOperation implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_ESCALATION_DEADLINE;
    constructor(public deadlineId: string, public condition: string, public escId: string) { }
}

export class UpdatePeopleAssignmentOperation implements Action {
    readonly type = ActionTypes.UPDATE_PEOPLE_ASSIGNMENT;
    constructor(public id: string, public peopleAssignments: PeopleAssignment[]) { }
}

export class CompletePeopleAssignmentOperation implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT;
    constructor(public peopleAssignments: PeopleAssignment[]) { }
}

export class UpdateEscalationOperation implements Action {
    readonly type = ActionTypes.UPDATE_ESCALATION;
    constructor(public id: string, public deadLineId: string, public escalation: Escalation) { }
}

export class CompleteUpdateEscalationOperation implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_ESCALATION;
    constructor(public deadLineId: string, public escalation: Escalation) { }
}

export class DeleteEscalationOperation implements Action {
    readonly type = ActionTypes.DELETE_ESCALATION;
    constructor(public id: string, public deadLineId: string, public escalation: Escalation) { }
}

export class CompleteDeleteEscalationOperation implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_ESCALATION;
    constructor(public deadLineId: string, public escalation: Escalation) { }
}

export class AddHumanTaskDefOperation implements Action {
    readonly type = ActionTypes.ADD_HUMANTASKEF;
    constructor(public name: string) { }
}

export class CompleteAddHumanTaskDefOperation implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_HUMANTASKDEF;
    constructor(public humanTaskDef: HumanTaskDef) { }
}

export class SearchHumanTaskDefOperation implements Action {
    readonly type = ActionTypes.SEARCH_HUMANTASKDEFS;
    constructor(public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearchHumanTaskDefOperation implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_HUMANTASKDEFS;
    constructor(public humanTaskDefsResult: SearchHumanTaskDefsResult) { }
}

export class UpdatePresentationElementOperation implements Action {
    readonly type = ActionTypes.UPDATE_PRESENTATIONELEMENT;
    constructor(public id: string, public presentationElements: PresentationElement[], public presentationParameters: PresentationParameter[]) { }
}

export class CompleteUpdatePresentationElementOperation implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT;
    constructor(public presentationElements: PresentationElement[], public presentationParameters: PresentationParameter[]) { }
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

export class AddToPartEscalation implements Action {
    readonly type = ActionTypes.ADD_ESCALATION_TOPART;
    constructor(public id: string, public deadlineId: string, public escalationId: string, public toPart: ToPart) { }
}

export class CompleteAddToPartEscalation implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_ESCALATION_TOPART;
    constructor(public id: string, public deadlineId: string, public escalationId: string, public toPart: ToPart) { }
}

export class DeleteToPartEscalation implements Action {
    readonly type = ActionTypes.DELETE_ESCALATION_TOPART;
    constructor(public id: string, public deadlineId: string, public escalationId: string, public toPartName: string) { }
}

export class CompleteDeleteToPartEscalation implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_ESCALATION_TOPART;
    constructor(public id: string, public deadlineId: string, public escalationId: string, public toPartName: string) { }
}

export type ActionsUnion = GetHumanTaskDef |
    GetHumanTaskDefComplete |
    UpdateHumanTaskDef |
    UpdateHumanTaskDefComplete |
    AddDeadline |
    CompleteAddDeadLine |
    UpdateHumanTaskInfo |
    CompleteUpdateHumanTaskInfo |
    AddOperationParameterOperation |
    CompleteAddOperationParameterOperation |
    DeleteOperationParameterOperation |
    CompleteDeleteOperationParameterOperation |
    UpdateRenderingOperation |
    CompleteUpdateRenderingOperation |
    DeleteDeadlineOperation |
    CompleteDeleteDeadlineOperation |
    UpdateDeadlineOperation |
    CompleteUpdateDeadlineOperation |
    AddEscalationDeadlineOperation |
    CompleteAddEscalationDeadlineOperation |
    UpdatePeopleAssignmentOperation |
    CompletePeopleAssignmentOperation |
    UpdateEscalationOperation |
    CompleteUpdateEscalationOperation |
    DeleteEscalationOperation |
    CompleteDeleteEscalationOperation |
    AddHumanTaskDefOperation |
    CompleteAddHumanTaskDefOperation |
    SearchHumanTaskDefOperation |
    CompleteSearchHumanTaskDefOperation |
    UpdatePresentationElementOperation |
    CompleteUpdatePresentationElementOperation |
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
    CompleteDeletePeopleAssignment |
    AddToPartEscalation |
    CompleteAddToPartEscalation |
    DeleteToPartEscalation |
    CompleteDeleteToPartEscalation;