import { Action } from '@ngrx/store';
import { SearchBpmnFilesResult } from '../models/search-bpmn-files-result.model';
import { BpmnFile } from '../models/bpmn-file.model';
import { HumanTaskDef } from '../../humantaskdefs/models/humantaskdef.model';

export enum ActionTypes {
    START_SEARCH_BPMNFILES = "[BpmnFiles] START_SEARCH_BPMNFILES",
    ERROR_SEARCH_BPMNFILES = "[BpmnFiles] ERROR_SEARCH_BPMNFILES",
    COMPLETE_SEARCH_BPMNFILES = "[BpmnFiles] COMPLETE_SEARCH_BPMNFILES",
    START_GET_BPMNFILE = "[BpmnFiles] START_GET_BPMNFILE",
    ERROR_GET_BPMNFILE = "[BpmnFiles] ERROR_GET_BPMNFILE",
    COMPLETE_GET_BPMNFILE = "[BpmnFiles] COMPLETE_GET_BPMNFILE",
    UPDATE_BPMNFILE = "[BpmnFiles] UPDATE_BPMNFILE",
    COMPLETE_UPDATE_BPMNFILE = "[BpmnFiles] COMPLETE_UPDATE_BPMNFILE",
    ERROR_UPDATE_BPMNFILE = "[BpmnFiles] ERROR_UPDATE_BPMNFILE",
    PUBLISH_BPMNFILE = "[BpmnFiles] PUBLISH_BPMNFILE",
    COMPLETE_PUBLISH_BPMNFILE = "[BpmnFiles] COMPLETE_PUBLISH_BPMNFILE",
    ERROR_PUBLISH_BPMNFILE = "[BpmnFiles] ERROR_PUBLISH_BPMNFILE",
    UPDATE_BPMNFILE_PAYLOAD = "[BpmnFiles] UPDATE_BPMNFILE_PAYLOAD",
    COMPLETE_UPDATE_BPMNFILE_PAYLOAD = "[BpmnFiles] COMPLETE_UPDATE_BPMNFILE_PAYLOAD",
    ERROR_UPDATE_BPMNFILE_PAYLOAD = "[BpmnFiles] ERROR_UPDATE_BPMNFILE_PAYLOAD"
}

export class SearchBpmnFiles implements Action {
    readonly type = ActionTypes.START_SEARCH_BPMNFILES
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public takeLatest: boolean, public fileId: string) { }
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
    constructor(public bpmnFile: BpmnFile, public humanTaskDefs: HumanTaskDef[]) { }
}

export class PublishBpmnFile implements Action {
    readonly type = ActionTypes.PUBLISH_BPMNFILE
    constructor(public id: string) { }
}

export class CompletePublishBpmnFile implements Action {
    readonly type = ActionTypes.COMPLETE_PUBLISH_BPMNFILE
    constructor(public id: string) { }
}

export class UpdateBpmnFile implements Action {
    readonly type = ActionTypes.UPDATE_BPMNFILE
    constructor(public id: string, public name: string, public description: string, public payload: string) { }
}

export class CompleteUpdateBpmnFile implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_BPMNFILE
    constructor(public id: string, public name: string, public description: string, public payload: string) { }
}

export class UpdateBpmnFilePayload implements Action {
    readonly type = ActionTypes.UPDATE_BPMNFILE_PAYLOAD
    constructor(public id: string, public payload: string) { }
}

export class CompleteUpdateBpmnFilePayload implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD
    constructor(public id: string, public payload: string) { }
}

export type ActionsUnion = SearchBpmnFiles |
    CompleteBpmnFiles |
    GetBpmnFile |
    CompleteBpmnFile |
    PublishBpmnFile |
    CompletePublishBpmnFile |
    UpdateBpmnFile |
    CompleteUpdateBpmnFile |
    UpdateBpmnFilePayload |
    CompleteUpdateBpmnFilePayload;