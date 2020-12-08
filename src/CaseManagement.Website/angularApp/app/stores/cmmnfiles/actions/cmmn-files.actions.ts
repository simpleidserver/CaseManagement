import { Action } from '@ngrx/store';
import { CmmnFile } from '../models/cmmn-file.model';
import { SearchCmmnFilesResult } from '../models/search-cmmn-files-result.model';

export enum ActionTypes {
    START_SEARCH_CMMNFILES = "[CmmnFiles] START_SEARCH_CMMNFILES",
    ERROR_SEARCH_CMMNFILES = "[CmmnFiles] ERROR_SEARCH_CMMNFILES",
    COMPLETE_SEARCH_CMMNFILES = "[CmmnFiles] COMPLETE_SEARCH_CASEFILES",
    START_SEARCH_CASEFILES_HISTORY = "[CmmnFiles] START_SEARCH_CASEFILES_HISTORY",
    ERROR_SEARCH_CASEFILES_HISTORY = "[CmmnFiles] ERROR_SEARCH_CASEFILES_HISTORY",
    COMPLETE_SEARCH_CASEFILES_HISTORY = "[CmmnFiles] COMPLETE_SEARCH_CASEFILES_HISTORY",
    START_GET_CMMNFILE = "[CmmnFiles] START_GET_CMMNFILE",
    ERROR_GET_CMMNFILE = "[CmmnFiles] ERROR_GET_CMMNFILE",
    COMPLETE_GET_CMMNFILE = "[CmmnFiles] COMPLETE_GET_CMMNFILE",
    ADD_CMMNFILE = "[CmmnFiles] ADD_CMMNFILE",
    ERROR_ADD_CMMNFILE = "[CmmnFiles] ERROR_ADD_CMMNFILE",
    COMPLETE_ADD_CMMNFILE = "[CmmnFiles] COMPLETE_ADD_CMMNFILE",
    PUBLISH_CMMNFILE = "[CmmnFiles] PUBLISH_CMMNFILE",
    ERROR_PUBLISH_CMMNFILE = "[CmmnFiles] ERROR_PUBLISH_CMMNFILE",
    COMPLETE_PUBLISH_CMMNFILE = "[CmmnFiles] COMPLETE_PUBLISH_CMMNFILE",
    UPDATE_CMMNFILE = "[CmmnFiles] UPDATE_CMMNFILE",
    ERROR_UPDATE_CMMNFILE = "[CmmnFiles] ERROR_UPDATE_CMMNFILE",
    COMPLETE_UPDATE_CMMNFILE = "[CmmnFiles] COMPLETE_UPDATE_CMMNFILE",
    UPDATE_CMMNFILE_PAYLOAD = "[CmmnFiles] UPDATE_CMMNFILE_PAYLOAD",
    COMPLETE_UPDATE_CMMNFILE_PAYLOAD = "[CmmnFiles] COMPLETE_UPDATE_CMMNFILE_PAYLOAD",
    ERROR_UPDATE_CMMNFILE_PAYLOAD = "[CmmnFiles] ERROR_UPDATE_CMMNFILE_PAYLOAD"
}

export class SearchCmmnFiles implements Action {
    readonly type = ActionTypes.START_SEARCH_CMMNFILES
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public text: string, public caseFileId: string, public takeLatest: boolean) { }
}

export class CompleteSearchCmmnFiles implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_CMMNFILES;
    constructor(public content: SearchCmmnFilesResult) { }
}

export class GetCmmnFile implements Action {
    readonly type = ActionTypes.START_GET_CMMNFILE
    constructor(public id: string) { }
}

export class CompleteGetCmmnFile implements Action {
    readonly type = ActionTypes.COMPLETE_GET_CMMNFILE;
    constructor(public content: CmmnFile) { }
}

export class AddCmmnFile implements Action {
    readonly type = ActionTypes.ADD_CMMNFILE
    constructor(public name: string, public description : string) { }
}

export class CompleteAddCmmnFile implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_CMMNFILE
    constructor(public id: string) { }
}

export class PublishCmmnFile implements Action {
    readonly type = ActionTypes.PUBLISH_CMMNFILE
    constructor(public id: string) { }
}

export class CompletePublishCmmnFile implements Action {
    readonly type = ActionTypes.COMPLETE_PUBLISH_CMMNFILE
    constructor(public id: string) { }
}

export class UpdateCmmnFile implements Action {
    readonly type = ActionTypes.UPDATE_CMMNFILE
    constructor(public id: string, public name : string, public description : string) { }
}

export class CompleteUpdateCmmnFile implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_CMMNFILE
    constructor(public id: string, public name: string, public description: string) { }
}

export class UpdateCmmnFilePayload implements Action {
    readonly type = ActionTypes.UPDATE_CMMNFILE_PAYLOAD
    constructor(public id: string, public payload: string) { }
}

export class CompleteUpdateCmmnFilePayload implements Action {
    readonly type = ActionTypes.COMPLETE_UPDATE_CMMNFILE_PAYLOAD
    constructor(public id: string, public payload: string) { }
}

export type ActionsUnion = SearchCmmnFiles |
    CompleteSearchCmmnFiles |
    GetCmmnFile |
    CompleteGetCmmnFile |
    AddCmmnFile |
    CompleteAddCmmnFile |
    PublishCmmnFile |
    CompletePublishCmmnFile |
    UpdateCmmnFile |
    UpdateCmmnFilePayload |
    CompleteUpdateCmmnFile |
    CompleteUpdateCmmnFilePayload;