import { Action } from '@ngrx/store';
import { SearchCaseFormInstancesResult } from '../models/search-case-form-instances-result.model';

export enum ActionTypes {
    START_SEARCH = "[CaseFormInstances] START_SEARCH",
    COMPLETE_SEARCH = "[CaseFormInstances] COMPLETE_SEARCH"
}

export class StartFetch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public id: string, public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteFetch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchCaseFormInstancesResult) { }
}

export type ActionsUnion = StartFetch | CompleteFetch;