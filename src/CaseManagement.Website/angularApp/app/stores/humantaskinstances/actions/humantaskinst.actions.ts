import { Action } from '@ngrx/store';
import { CreateHumanTaskInstance } from '../parameters/create-humantaskinstance.model';

export enum ActionTypes {
    CREATE_HUMANTASKINSTANCE = "[HumanTaskInstance] CREATE_HUMANTASKINSTANCE",
    COMPLETE_CREATE_HUMANTASKINSTANCE = "[HumanTaskInstance] COMPLETE_CREATE_HUMANTASKINSTANCE",
    ERROR_CREATE_HUMANTASKINSTANCE = "[HumanTaskInstance] ERROR_CREATE_HUMANTASKINSTANCE",
    CREATE_ME_HUMANTASKINSTANCE = "[HumanTaskInstance] CREATE_ME_HUMANTASKINSTANCE",
    COMPLETE_ME_CREATE_HUMANTASKINSTANCE = "[HumanTaskInstance] COMPLETE_ME_CREATE_HUMANTASKINSTANCE",
    ERROR_ME_CREATE_HUMANTASKINSTANCE = "[HumanTaskInstance] ERROR_ME_CREATE_HUMANTASKINSTANCE"
}

export class CreateHumanTaskInstanceOperation implements Action {
    readonly type = ActionTypes.CREATE_HUMANTASKINSTANCE;
    constructor(public cmd: CreateHumanTaskInstance) { }
}

export class CreateMeHumanTaskInstanceOperation implements Action {
    readonly type = ActionTypes.CREATE_ME_HUMANTASKINSTANCE;
    constructor(public cmd: CreateHumanTaskInstance) { }
}

export type ActionsUnion = CreateHumanTaskInstanceOperation
    | CreateMeHumanTaskInstanceOperation;