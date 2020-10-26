import { Action } from '@ngrx/store';
import { CreateHumanTaskInstance } from '../parameters/create-humantaskinstance.model';

export enum ActionTypes {
    CREATE_HUMANTASKINSTANCE = "[HumanTaskInstance] CREATE_HUMANTASKINSTANCE",
    COMPLETE_CREATE_HUMANTASKINSTANCE = "[HumanTaskInstance] COMPLETE_CREATE_HUMANTASKINSTANCE",
    ERROR_CREATE_HUMANTASKINSTANCE = "[HumanTaskInstance] ERROR_CREATE_HUMANTASKINSTANCE"
}

export class CreateHumanTaskInstanceOperation implements Action {
    readonly type = ActionTypes.CREATE_HUMANTASKINSTANCE;
    constructor(public cmd: CreateHumanTaskInstance) { }
}

export type ActionsUnion = CreateHumanTaskInstanceOperation;