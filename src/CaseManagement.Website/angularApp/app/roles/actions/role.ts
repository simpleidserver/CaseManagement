import { Action } from '@ngrx/store';
import { SearchRolesResult } from '../models/search-roles.model';
import { Role } from '../models/role.model';

export enum ActionTypes {
    START_SEARCH = "[Role] START_SEARCH",
    COMPLETE_SEARCH = "[Role] COMPLETE_SEARCH",
    START_GET = "[Role] START_GET",
    COMPLETE_GET = "[Role] COMPLETE_GET"
}

export class StartSearch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public startIndex: number, public count: number, public order: string, public direction: string) { }
}

export class CompleteSearch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchRolesResult) { }
}

export class StartGet implements Action {
    readonly type = ActionTypes.START_GET
    constructor(public role: string) { }
}

export class CompleteGet implements Action {
    readonly type = ActionTypes.COMPLETE_GET
    constructor(public content: Role) { }
}

export type ActionsUnion = StartSearch | CompleteSearch | StartGet | CompleteGet;