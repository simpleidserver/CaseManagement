import { Action } from '@ngrx/store';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseFilesResult } from '../models/search-case-files-result.model';

export enum ActionTypes {
    START_SEARCH = "[CaseFiles] START_SEARCH",
    COMPLETE_SEARCH = "[CaseFiles] COMPLETE_SEARCH",
    START_GET = "[CaseFiles] START_GET",
    COMPLETE_GET = "[CaseFiles] COMPLETE_GET"
}

export class StartFetch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public text: string, public user : string) { }
}

export class CompleteFetch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchCaseFilesResult) { }
}

export class StartGet implements Action {
    readonly type = ActionTypes.START_GET
    constructor(public id: string) { }
}

export class CompleteGet implements Action {
    readonly type = ActionTypes.COMPLETE_GET;
    constructor(public content: CaseFile) { }
}

export type ActionsUnion = StartFetch | CompleteFetch | StartGet | CompleteGet;