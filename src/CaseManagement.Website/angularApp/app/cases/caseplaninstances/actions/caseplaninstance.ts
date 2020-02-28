import { Action } from '@ngrx/store';
import { CasePlanInstance } from '../models/caseplaninstance.model';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';

export enum ActionTypes {
    START_SEARCH = "[CaseInstances] START_SEARCH",
    COMPLETE_SEARCH = "[CaseInstances] COMPLETE_SEARCH",
    START_GET = "[CaseInstances] START_GET",
    COMPLETE_GET = "[CaseInstances] COMPLETE_GET"
}

export class StartSearch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public startIndex: number, public count: number, public order: string, public direction: string, public casePlanId: string) { }
}

export class CompleteSearch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchCasePlanInstanceResult) { }
}

export class StartGet implements Action {
    readonly type = ActionTypes.START_GET;
    constructor(public id: string) { }
}

export class CompleteGet implements Action {
    readonly type = ActionTypes.COMPLETE_GET;
    constructor(public content: CasePlanInstance) { }
}

export type ActionsUnion = StartSearch | CompleteSearch | StartGet | CompleteGet;