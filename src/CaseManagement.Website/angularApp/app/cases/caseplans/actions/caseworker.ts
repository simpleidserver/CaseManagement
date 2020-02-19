import { Action } from '@ngrx/store';
import { SearchWorkerTaskResult } from '../models/searchworkertaskresult.model';

export enum ActionTypes {
    START_SEARCH = "[CaseWorker] START_SEARCH",
    COMPLETE_SEARCH = "[CaseWorker] COMPLETE_SEARCH"
}

export class StartSearch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public id : string, public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchWorkerTaskResult) { }
}

export type ActionsUnion = StartSearch | CompleteSearch;