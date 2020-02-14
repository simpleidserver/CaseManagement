import { Action } from '@ngrx/store';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseFilesResult } from '../models/search-case-files-result.model';

export enum ActionTypes {
    START_SEARCH = "[CaseFiles] START_SEARCH",
    COMPLETE_SEARCH = "[CaseFiles] COMPLETE_SEARCH",
    START_SEARCH_HISTORY = "[CasesFiles] START_SEARCH_HISTORY",
    COMPLETE_SEARCH_HISTORY = "[CasesFiles] COMPLETE_SEARCH_HISTORY",
    START_GET = "[CaseFiles] START_GET",
    COMPLETE_GET = "[CaseFiles] COMPLETE_GET"
}

export class StartSearch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public text: string) { }
}

export class CompleteSearch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchCaseFilesResult) { }
}

export class StartSearchHistory implements Action {
    readonly type = ActionTypes.START_SEARCH_HISTORY;
    constructor(public caseFileId: string, public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearchHistory {
    readonly type = ActionTypes.COMPLETE_SEARCH_HISTORY;
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

export type ActionsUnion = StartSearch | CompleteSearch | StartSearchHistory | CompleteSearchHistory |  StartGet | CompleteGet;