import { Action } from '@ngrx/store';
import { SearchCaseActivationsResult } from '../models/search-case-activations-result.model';

export enum ActionTypes {
    START_SEARCH = "[CaseActivations] START_SEARCH",
    COMPLETE_SEARCH = "[CaseActivations] COMPLETE_SEARCH"
}

export class StartFetch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public id : string, public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteFetch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchCaseActivationsResult) { }
}

export type ActionsUnion = StartFetch | CompleteFetch;