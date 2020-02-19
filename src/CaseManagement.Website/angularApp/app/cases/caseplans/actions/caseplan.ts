import { Action } from '@ngrx/store';
import { CasePlan } from '../models/caseplan.model';
import { SearchCasePlanResult } from '../models/searchcaseplanresult.model';

export enum ActionTypes {
    START_SEARCH = "[CasePlan] START_SEARCH",
    COMPLETE_SEARCH = "[CasePlan] COMPLETE_SEARCH",
    START_GET = "[CasePlan] START_GET",
    COMPLETE_GET = "[CasePlan] COMPLETE_GET",
    START_SEARCH_HISTORY = "[CasePlan] START_SEARCH_HISTORY",
    COMPLETE_SEARCH_HISTORY = "[CasePlan] COMPLETE_SEARCH_HISTORY"
}

export class StartSearch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public text: string) { }
}

export class CompleteSearch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchCasePlanResult) { }
}

export class StartGet implements Action {
    readonly type = ActionTypes.START_GET
    constructor(public id: string) { }
}

export class CompleteGet implements Action {
    readonly type = ActionTypes.COMPLETE_GET;
    constructor(public content: CasePlan) { }
}

export class StartSearchHistory implements Action {
    readonly type = ActionTypes.START_SEARCH_HISTORY
    constructor(public id: string, public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearchHistory implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_HISTORY;
    constructor(public content: SearchCasePlanResult) { }
}

export type ActionsUnion = StartSearch | CompleteSearch | StartGet | CompleteGet | StartSearchHistory | CompleteSearchHistory;