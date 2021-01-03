import { Action } from '@ngrx/store';
import { SearchCaseInstanceResult } from '../models/search-caseinstance.model';

export enum ActionTypes {
    SEARCH_CASES = "[Cases] SEARCH_CASES",
    ERROR_SEARCH_CASES = "[Cases] ERROR_SEARCH_CASES",
    COMPLETE_SEARCH_CASES = "[Cases] COMPLETE_SEARCH_CASES"
}

export class SearchCases implements Action {
    readonly type = ActionTypes.SEARCH_CASES
    constructor(public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearchCases implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_CASES;
    constructor(public content: SearchCaseInstanceResult) { }
}

export type ActionsUnion = SearchCases |
    CompleteSearchCases;