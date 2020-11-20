import { Action } from '@ngrx/store';
import { SearchBpmnFilesResult } from '../models/search-bpmn-files-result.model';
import { BpmnFile } from '../models/bpmn-file.model';

export enum ActionTypes {
    START_SEARCH_BPMNFILES = "[CaseFiles] START_SEARCH_BPMNFILES",
    ERROR_SEARCH_BPMNFILES = "[CaseFiles] ERROR_SEARCH_BPMNFILES",
    COMPLETE_SEARCH_BPMNFILES = "[CaseFiles] COMPLETE_SEARCH_BPMNFILES",
    START_GET_BPMNFILE = "[CaseFiles] START_GET_BPMNFILE",
    ERROR_GET_BPMNFILE = "[CaseFiles] ERROR_GET_BPMNFILE",
    COMPLETE_GET_BPMNFILE = "[CaseFiles] COMPLETE_GET_BPMNFILE"
}

export class SearchBpmnFiles implements Action {
    readonly type = ActionTypes.START_SEARCH_BPMNFILES
    constructor(public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteBpmnFiles implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_BPMNFILES;
    constructor(public content: SearchBpmnFilesResult) { }
}

export class GetBpmnFile implements Action {
    readonly type = ActionTypes.START_GET_BPMNFILE
    constructor(public id: string) { }
}

export class CompleteBpmnFile implements Action {
    readonly type = ActionTypes.COMPLETE_GET_BPMNFILE
    constructor(public bpmnFile: BpmnFile) { }
}

export type ActionsUnion = SearchBpmnFiles |
    CompleteBpmnFiles |
    GetBpmnFile |
    CompleteBpmnFile;