import { Action } from '@ngrx/store';
import { SearchBpmnFilesResult } from '../models/search-bpmn-files-result.model';

export enum ActionTypes {
    START_SEARCH_BPMNFILES = "[CaseFiles] START_SEARCH_BPMNFILES",
    ERROR_SEARCH_BPMNFILES = "[CaseFiles] ERROR_SEARCH_BPMNFILES",
    COMPLETE_SEARCH_BPMNFILES = "[CaseFiles] COMPLETE_SEARCH_BPMNFILES"
}

export class SearchBpmnFiles implements Action {
    readonly type = ActionTypes.START_SEARCH_BPMNFILES
    constructor(public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteBpmnFiles implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_BPMNFILES;
    constructor(public content: SearchBpmnFilesResult) { }
}

export type ActionsUnion = SearchBpmnFiles |
    CompleteBpmnFiles;