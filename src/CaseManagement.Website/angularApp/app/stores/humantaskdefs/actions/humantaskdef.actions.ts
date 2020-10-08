import { Action } from '@ngrx/store';
import { HumanTaskDef } from '../models/humantaskdef.model';

export enum ActionTypes {
    START_GET_HUMANTASKDEF = "[HumanTaskDef] START_GET_HUMANTASKDEF",
    COMPLETE_GET_HUMANTASKDEF = "[CasePlanInstance] COMPLETE_GET_HUMANTASKDEF",
    ERROR_GET_HUMANTASKDEF = "[CasePlanInstance] ERROR_GET_HUMANTASKDEF",
    UPDATE_HUMANASKDEF = "[HumanTaskDef] UPDATE_HUMANASKDEF",
    COMPLETE_UPDATE_HUMANASKDEF = "[HumanTaskDef] COMPLETE_UPDATE_HUMANASKDEF",
    ERROR_UPDATE_HUMANASKDEF = "[CasePlanInstance] ERROR_UPDATE_HUMANASKDEF",
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

export type ActionsUnion = GetHumanTaskDef |
    GetHumanTaskDefComplete |
    UpdateHumanTaskDef |
    UpdateHumanTaskDefComplete;