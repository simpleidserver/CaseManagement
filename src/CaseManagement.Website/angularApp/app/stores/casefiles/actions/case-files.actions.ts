import { Action } from '@ngrx/store';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseFilesResult } from '../models/search-case-files-result.model';

export enum ActionTypes {
    START_SEARCH_CASEFILES = "[CaseFiles] START_SEARCH_CASEFILES",
    ERROR_SEARCH_CASEFILES = "[CaseFiles] ERROR_SEARCH_CASEFILES",
    COMPLETE_SEARCH_CASEFILES = "[CaseFiles] COMPLETE_SEARCH_CASEFILES",
    START_SEARCH_CASEFILES_HISTORY = "[CasesFiles] START_SEARCH_CASEFILES_HISTORY",
    ERROR_SEARCH_CASEFILES_HISTORY = "[CasesFiles] ERROR_SEARCH_CASEFILES_HISTORY",
    COMPLETE_SEARCH_CASEFILES_HISTORY = "[CasesFiles] COMPLETE_SEARCH_CASEFILES_HISTORY",
    START_GET_CASEFILE = "[CaseFiles] START_GET_CASEFILE",
    ERROR_GET_CASEFILE = "[CaseFiles] ERROR_GET_CASEFILE",
    COMPLETE_GET_CASEFILE = "[CaseFiles] COMPLETE_GET_CASEFILE",
    ADD_CASEFILE = "[CaseFiles] ADD_CASEFILE",
    ERROR_ADD_CASEFILE = "[CaseFiles] ERROR_ADD_CASEFILE",
    COMPLETE_ADD_CASEFILE = "[CaseFiles] COMPLETE_ADD_CASEFILE",
    PUBLISH_CASEFILE = "[CaseFiles] PUBLISH_CASEFILE",
    ERROR_PUBLISH_CASEFILE = "[CaseFiles] ERROR_PUBLISH_CASEFILE",
    COMPLETE_PUBLISH_CASEFILE = "[CaseFiles] COMPLETE_PUBLISH_CASEFILE",
    UPDATE_CASEFILE = "[CaseFiles] UPDATE_CASEFILE",
    ERROR_UPDATE_CASEFILE = "[CaseFiles] ERROR_UPDATE_CASEFILE",
    COMPLETE_UPDATE_CASEFILE = "[CaseFiles] COMPLETE_UPDATE_CASEFILE"
}

export class SearchCaseFiles implements Action {
    readonly type = ActionTypes.START_SEARCH_CASEFILES
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public text: string) { }
}

export class CompleteSearchCaseFiles implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_CASEFILES;
    constructor(public content: SearchCaseFilesResult) { }
}

export class SearchCaseFilesHistory implements Action {
    readonly type = ActionTypes.START_SEARCH_CASEFILES_HISTORY;
    constructor(public caseFileId: string, public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearchCaseFilesHistory implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_CASEFILES_HISTORY;
    constructor(public content: SearchCaseFilesResult) { }
}

export class GetCaseFile implements Action {
    readonly type = ActionTypes.START_GET_CASEFILE
    constructor(public id: string) { }
}

export class CompleteGetCaseFile implements Action {
    readonly type = ActionTypes.COMPLETE_GET_CASEFILE;
    constructor(public content: CaseFile) { }
}

export class AddCaseFile implements Action {
    readonly type = ActionTypes.ADD_CASEFILE
    constructor(public name: string, public description : string) { }
}

export class CompleteAddCaseFile implements Action {
    readonly type = ActionTypes.COMPLETE_ADD_CASEFILE
    constructor(public id: string) { }
}

export class PublishCaseFile implements Action {
    readonly type = ActionTypes.PUBLISH_CASEFILE
    constructor(public id: string) { }
}

export class CompletePublishCaseFile implements Action {
    readonly type = ActionTypes.COMPLETE_PUBLISH_CASEFILE
    constructor(public id: string) { }
}

export class UpdateCaseFile implements Action {
    readonly type = ActionTypes.UPDATE_CASEFILE
    constructor(public id: string, public name : string, public description : string, public payload : string) { }
}

export type ActionsUnion = SearchCaseFiles |
    CompleteSearchCaseFiles |
    SearchCaseFilesHistory |
    CompleteSearchCaseFilesHistory |
    GetCaseFile |
    CompleteGetCaseFile |
    AddCaseFile |
    CompleteAddCaseFile |
    PublishCaseFile |
    CompletePublishCaseFile |
    UpdateCaseFile;