import { Action } from '@ngrx/store';
import { SearchCaseFilesResult } from '../models/search-case-files-result.model';

export enum ActionTypes {
    CASEFILESLOAD = "[CaseFiles] Load",
    CASEFILESLOADED = "[CaseFiles] Loaded",
    ERRORLOADCASEFILES = "[CaseFiles] Error Load"
}

export class LoadCaseFilesAction implements Action {
    type = ActionTypes.CASEFILESLOAD
    constructor() { }
}

export class CaseFilesLoadedAction implements Action {
    type = ActionTypes.CASEFILESLOADED
    constructor(public result: SearchCaseFilesResult) { }
}

export type ActionsUnion = LoadCaseFilesAction | CaseFilesLoadedAction;