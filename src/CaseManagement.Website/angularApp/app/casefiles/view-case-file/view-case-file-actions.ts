import { Action } from '@ngrx/store';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';

export enum ActionTypes {
    LOADCASEFILE = "[CaseFile] Load",
    CASEFILELOADED = "[CaseFile] Loaded",
    ERRORLOADCASEFILE = "[CaseFile] Error Load",
    LOADCASEDEFINITIONS = "[CaseDefinitions] Load",
    CASEDEFINITIONSLOADED = "[CaseDefinitions] Loaded",
    ERRORLOADCASEDEFINITIONS = "[CaseDefinitions] Error Load"
}

export class LoadCaseFile implements Action {
    type = ActionTypes.LOADCASEFILE
    constructor() { }
}

export class CaseFileLoaded implements Action {
    type = ActionTypes.CASEFILELOADED
    constructor(public caseFile: CaseFile) { }
}

export class LoadCaseDefinitions implements Action {
    type = ActionTypes.LOADCASEFILE
    constructor() { }
}

export class CaseDefinitionsLoaded implements Action {
    type = ActionTypes.CASEFILELOADED
    constructor(public caseDefinitions: SearchCaseDefinitionsResult) { }
}

export type ActionsUnion = LoadCaseFile | CaseFileLoaded | LoadCaseDefinitions | CaseDefinitionsLoaded;