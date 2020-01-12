import { Action } from '@ngrx/store';
import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';

export enum ActionTypes {
    CASEDEFINITIONSLOAD = "[CaseDefinitions] Load",
    CASEDEFINITIONSLOADED = "[CaseDefinitions] Loaded",
    ERRORLOADCASEDEFINITIONS = "[CaseDefinitions] Error Load"
}

export class LoadCaseDefinitionsAction implements Action {
    type = ActionTypes.CASEDEFINITIONSLOAD
    constructor() { }
}

export class CaseDefinitionsLoadedAction implements Action {
    type = ActionTypes.CASEDEFINITIONSLOADED
    constructor(public result: SearchCaseDefinitionsResult) { }
}

export type ActionsUnion = LoadCaseDefinitionsAction | CaseDefinitionsLoadedAction;