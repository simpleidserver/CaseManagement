import { Action } from '@ngrx/store';
import { HumanTaskDef } from '../models/humantaskdef.model';

export enum ActionTypes {
    START_GET_HUMANTASKDEF = "[HumanTaskDef] START_GET_HUMANTASKDEF",
    COMPLETE_GET_HUMANTASKDEF = "[CasePlanInstance] COMPLETE_GET_HUMANTASKDEF",
    ERROR_GET_HUMANTASKDEF = "[CasePlanInstance] ERROR_GET_HUMANTASKDEF",
}

export class GetHumanTaskDef implements Action {
    readonly type = ActionTypes.START_GET_HUMANTASKDEF;
    constructor(public id: string) { }
}

export class GetHumanTaskDefComplete implements Action {
    readonly type = ActionTypes.COMPLETE_GET_HUMANTASKDEF;
    constructor(public content: HumanTaskDef) { }
}

export type ActionsUnion = GetHumanTaskDef |
    GetHumanTaskDefComplete;