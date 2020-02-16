import { Action } from '@ngrx/store';
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';
import { CaseInstance } from '../models/case-instance.model';
import { CaseFileItem } from '../models/case-file-item.model';

export enum ActionTypes {
    START_SEARCH = "[CaseInstances] START_SEARCH",
    COMPLETE_SEARCH = "[CaseInstances] COMPLETE_SEARCH",
    START_GET = "[CaseInstances] START_GET",
    COMPLETE_GET = "[CaseInstances] COMPLETE_GET",
    START_GET_FILE_ITEMS = "[CaseInstances] START_GET_FILE_ITEMS",
    COMPLETE_GET_FILE_ITEMS = "[CaseInstances] COMPLETE_GET_FILE_ITEMS"
}

export class StartFetch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public id: string, public startIndex: number, public count: number, public order: string, public direction: string) { }
}

export class CompleteFetch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchCaseInstancesResult) { }
}

export class StartGet implements Action {
    readonly type = ActionTypes.START_GET
    constructor(public id: string) { }
}

export class CompleteGet implements Action {
    readonly type = ActionTypes.COMPLETE_GET;
    constructor(public content: CaseInstance) { }
}

export class StartGetFileItems implements Action {
    readonly type = ActionTypes.START_GET_FILE_ITEMS;
    constructor(public id : string) { }
}

export class CompleteGetFileItems implements Action {
    readonly type = ActionTypes.COMPLETE_GET_FILE_ITEMS;
    constructor(public content: CaseFileItem[]) { }
}

export type ActionsUnion = StartFetch | CompleteFetch | StartGet | CompleteGet | StartGetFileItems | CompleteGetFileItems;