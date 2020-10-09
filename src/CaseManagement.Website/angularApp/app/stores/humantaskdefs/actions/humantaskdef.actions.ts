import { Action } from '@ngrx/store';
import { Parameter } from '../../common/operation.model';
import { HumanTaskDefinitionDeadLine } from '../models/humantaskdef-deadlines';
import { HumanTaskDef } from '../models/humantaskdef.model';
import { Rendering } from '../../common/rendering.model';

export enum ActionTypes {
    START_GET_HUMANTASKDEF = "[HumanTaskDef] START_GET_HUMANTASKDEF",
    COMPLETE_GET_HUMANTASKDEF = "[CasePlanInstance] COMPLETE_GET_HUMANTASKDEF",
    ERROR_GET_HUMANTASKDEF = "[CasePlanInstance] ERROR_GET_HUMANTASKDEF",
    UPDATE_HUMANASKDEF = "[HumanTaskDef] UPDATE_HUMANASKDEF",
    COMPLETE_UPDATE_HUMANASKDEF = "[HumanTaskDef] COMPLETE_UPDATE_HUMANASKDEF",
    ERROR_UPDATE_HUMANASKDEF = "[HumanTaskDef] ERROR_UPDATE_HUMANASKDEF",
    ADD_START_DEADLINE = "[HumanTaskDef] ADD_START_DEADLINE",
    COMPLETE_ADD_START_DEADLINE = "[HumanTaskDef] COMPLETE_ADD_STARTDEADLINE",
    ERROR_ADD_START_DEADLINE = "[HumanTaskDef] ERROR_ADD_START_DEADLINE",
    ADD_COMPLETION_DEADLINE = "[HumanTaskDef] ADD_COMPLETION_DEADLINE",
    COMPLETE_ADD_COMPLETION_DEADLINE = "[HumanTaskDef] COMPLETE_ADD_COMPLETION_DEADLINE",
    ERROR_ADD_COMPLETION_DEADLINE = "[HumanTaskDef] ERROR_ADD_COMPLETION_DEADLINE",
    UPDATE_HUMANTASKDEF_INFO = "[HumanTaskDef] UPDATE_HUMANTASKDEF_INFO",
    COMPLETE_UPDATE_HUMANTASK_INFO = "[HumanTaskDef] COMPLETE_UPDATE_HUMANTASK_INFO",
    ERROR_UPDATE_HUMANTASK_INFO = "[HumanTaskDef] ERROR_UPDATE_HUMANTASK_INFO",
    ADD_OPERATION_INPUT_PARAMETER = "[HumanTaskDef] ADD_OPERATION_INPUT_PARAMETER",
    COMPLETE_ADD_OPERATION_INPUT_PARAMETER = "[HumanTaskDef] COMPLETE_ADD_OPERATION_INPUT_PARAMETER",
    ERROR_ADD_OPERATION_INPUT_PARAMETER = "[HumanTaskDef] ERROR_ADD_OPERATION_INPUT_PARAMETER",
    ADD_OPERATION_OUTPUT_PARAMETER = "[HumanTaskDef] ADD_OPERATION_OUTPUT_PARAMETER",
    COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER = "[HumanTaskDef] COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER",
    ERROR_ADD_OPERATION_OUTPUT_PARAMETER = "[HumanTaskDef] ERROR_ADD_OPERATION_OUTPUT_PARAMETER",
    DELETE_OPERATION_INPUT_PARAMETER = "[HumanTaskDef] DELETE_OPERATION_INPUT_PARAMETER",
    COMPLETE_DELETE_OPERATION_INPUT_PARAMETER = "[HumanTaskDef] COMPLETE_DELETE_OPERATION_INPUT_PARAMETER",
    ERROR_DELETE_OPERATION_INPUT_PARAMETER = "[HumanTaskDef] ERROR_DELETE_OPERATION_INPUT_PARAMETER",
    DELETE_OPERATION_OUTPUT_PARAMETER = "[HumanTaskDef] DELETE_OPERATION_OUTPUT_PARAMETER",
    COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER = "[HumanTaskDef] COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER",
    ERROR_DELETE_OPERATION_OUTPUT_PARAMETER = "[HumanTaskDef] ERROR_DELETE_OPERATION_OUTPUT_PARAMETER",
    UPDATE_RENDERING_PARAMETER = "[HumanTaskDef] UPDATE_RENDERING_PARAMETER",
    COMPLETE_UPDATE_RENDERING_PARAMETER = "[HumanTaskDef] COMPLETE_UPDATE_RENDERING_PARAMETER",
    ERROR_UPDATE_RENDERING_PARAMETER = "[HumanTaskDef] ERROR_UPDATE_RENDERING_PARAMETER",
    DELETE_START_DEADLINE = "[HumanTaskDef] DELETE_START_DEADLINE",
    COMPLETE_DELETE_START_DEALINE = "[HumanTaskDef] COMPLETE_DELETE_START_DEALINE",
    ERROR_DELETE_START_DEALINE = "[HumanTaskDef] ERROR_DELETE_START_DEALINE",
    DELETE_COMPLETION_DEADLINE = "[HumanTaskDef] DELETE_COMPLETION_DEADLINE",
    COMPLETE_DELETE_COMPLETION_DEADLINE = "[HumanTaskDef] COMPLETE_DELETE_COMPLETION_DEADLINE",
    ERROR_DELETE_COMPLETION_DEADLINE = "[HumanTaskDef] ERROR_DELETE_COMPLETION_DEADLINE",
    UPDATE_START_DEADLINE = "[HumanTaskDef] UPDATE_START_DEADLINE",
    COMPLETE_UPDATE_START_DEADLINE = "[HumanTaskDef] COMPLETE_UPDATE_START_DEADLINE",
    ERROR_UPDATE_START_DEADLINE = "[HumanTaskDef] ERROR_UPDATE_START_DEADLINE",
    UPDATE_COMPLETION_DEADLINE = "[HumanTaskDef] UPDATE_COMPLETION_DEADLINE",
    COMPLETE_UPDATE_COMPLETION_DEADLINE = "[HumanTaskDef] COMPLETE_UPDATE_COMPLETION_DEADLINE",
    ERROR_UPDATE_COMPLETION_DEADLINE = "[HumanTaskDef] ERROR_UPDATE_COMPLETION_DEADLINE"
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

export class AddStartDeadLine implements Action {
    readonly type = ActionTypes.ADD_START_DEADLINE;
    constructor(public id: string, public deadLine: HumanTaskDefinitionDeadLine) { }
}

export class CompleteAddStartDeadLine implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_START_DEADLINE;
    constructor(public content: HumanTaskDefinitionDeadLine) { }
}

export class AddCompletionDeadLine implements Action {
    readonly type = ActionTypes.ADD_COMPLETION_DEADLINE;
    constructor(public id: string, public deadLine: HumanTaskDefinitionDeadLine) { }
}

export class CompleteCompletionDeadLine implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE;
    constructor(public content: HumanTaskDefinitionDeadLine) { }
}

export class UpdateHumanTaskInfo implements Action {
    readonly type = ActionTypes.UPDATE_HUMANTASKDEF_INFO;
    constructor(public id : string, public name: string, public priority : number) { }
}

export class CompleteUpdateHumanTaskInfo implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO;
    constructor(public name: string, public priority: number) { }
}

export class AddInputParameterOperation implements Action {
    readonly type = ActionTypes.ADD_OPERATION_INPUT_PARAMETER;
    constructor(public id: string, public parameter : Parameter) { }
}

export class CompleteAddInputParameterOperation implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER;
    constructor(public parameter: Parameter) { }
}

export class AddOutputParameterOperation implements Action {
    readonly type = ActionTypes.ADD_OPERATION_OUTPUT_PARAMETER;
    constructor(public id: string, public parameter: Parameter) { }
}

export class CompleteAddOutputParameterOperation implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER;
    constructor(public id: string, public parameter: Parameter) { }
}

export class DeleteInputParameterOperation implements Action {
    readonly type = ActionTypes.DELETE_OPERATION_INPUT_PARAMETER;
    constructor(public id: string, public name: string) { }
}

export class CompleteDeleteInputParameterOperation implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER;
    constructor(public name: string) { }
}

export class DeleteOutputParameterOperation implements Action {
    readonly type = ActionTypes.DELETE_OPERATION_OUTPUT_PARAMETER;
    constructor(public id: string, public name: string) { }
}

export class CompleteDeleteOutputParameterOperation implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER;
    constructor(public name: string) { }
}

export class UpdateRenderingOperation implements Action {
    readonly type = ActionTypes.UPDATE_RENDERING_PARAMETER;
    constructor(public id: string, public rendering: Rendering) { }
}

export class CompleteUpdateRenderingOperation implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER;
    constructor(public rendering: Rendering) { }
}

export class DeleteStartDeadlineOperation implements Action {
    readonly type = ActionTypes.DELETE_START_DEADLINE;
    constructor(public id: string, public name: string) { }
}

export class CompleteDeleteStartDeadlineOperation implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_START_DEALINE;
    constructor(public name: string) { }
}

export class DeleteCompletionDeadlineOperation implements Action {
    readonly type = ActionTypes.DELETE_COMPLETION_DEADLINE;
    constructor(public id: string, public name: string) { }
}

export class CompleteDeleteCompletionDeadlineOperation implements Action {
    readonly type = ActionTypes.COMPLETE_DELETE_COMPLETION_DEADLINE;
    constructor(public name: string) { }
}

export class UpdateStartDeadlineOperation implements Action {
    readonly type = ActionTypes.UPDATE_START_DEADLINE;
    constructor(public id: string, public deadline: HumanTaskDefinitionDeadLine) { }
}

export class CompleteUpdateStartDeadlineOperation implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_START_DEADLINE;
    constructor(public deadline: HumanTaskDefinitionDeadLine) { }
}

export class UpdateCompletionDeadlineOperation implements Action {
    readonly type = ActionTypes.UPDATE_COMPLETION_DEADLINE;
    constructor(public id: string, public deadline: HumanTaskDefinitionDeadLine) { }
}

export class CompleteCompletionStartDeadlineOperation implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_COMPLETION_DEADLINE;
    constructor(public deadline: HumanTaskDefinitionDeadLine) { }
}

export type ActionsUnion = GetHumanTaskDef |
    GetHumanTaskDefComplete |
    UpdateHumanTaskDef |
    UpdateHumanTaskDefComplete |
    AddStartDeadLine |
    CompleteAddStartDeadLine |
    AddCompletionDeadLine |
    CompleteCompletionDeadLine | 
    UpdateHumanTaskInfo |
    CompleteUpdateHumanTaskInfo |
    AddInputParameterOperation |
    CompleteAddInputParameterOperation |
    AddOutputParameterOperation |
    CompleteAddOutputParameterOperation |
    DeleteInputParameterOperation |
    CompleteDeleteInputParameterOperation |
    DeleteOutputParameterOperation |
    CompleteDeleteOutputParameterOperation |
    UpdateRenderingOperation |
    CompleteUpdateRenderingOperation |
    DeleteStartDeadlineOperation |
    CompleteDeleteStartDeadlineOperation |
    DeleteCompletionDeadlineOperation |
    CompleteDeleteCompletionDeadlineOperation |
    UpdateStartDeadlineOperation |
    CompleteUpdateStartDeadlineOperation |
    UpdateCompletionDeadlineOperation |
    CompleteCompletionStartDeadlineOperation;