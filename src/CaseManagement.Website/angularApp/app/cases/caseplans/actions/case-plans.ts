import { Action } from '@ngrx/store';
import { CasePlan } from '../models/case-plan.model';
import { SearchCasePlansResult } from '../models/search-case-plans-result.model';
import { CaseDefinitionHistory } from '../models/case-definition-history.model';

export enum ActionTypes {
    START_SEARCH = "[CasePlans] START_SEARCH",
    COMPLETE_SEARCH = "[CasePlans] COMPLETE_SEARCH",
    START_GET = "[CasePlans] START_GET",
    COMPLETE_GET = "[CasePlans] COMPLETE_GET",
    START_GET_HISTORY = "[CasePlans] START_GET_HISTORY",
    COMPLETE_GET_HISTORY = "[CasePlans] COMPLETE_GET_HISTORY"
}

export class StartSearch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public text: string) { }
}

export class CompleteSearch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchCasePlansResult) { }
}

export class StartGet implements Action {
    readonly type = ActionTypes.START_GET
    constructor(public id: string) { }
}

export class CompleteGet implements Action {
    readonly type = ActionTypes.COMPLETE_GET;
    constructor(public content: CasePlan) { }
}

export class StartGetHistory implements Action {
    readonly type = ActionTypes.START_GET_HISTORY;
    constructor(public id: string) { }
}

export class CompleteGetHistory implements Action {
    readonly type = ActionTypes.COMPLETE_GET_HISTORY;
    constructor(public content: CaseDefinitionHistory) { }
}

export type ActionsUnion = StartSearch | CompleteSearch | StartGet | CompleteGet | StartGetHistory | CompleteGetHistory;