import { Action } from '@ngrx/store';
import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';

export enum ActionTypes {
    CASEDEFSLOAD = "[CaseDefs] Load",
    CASEDEFSLOADED = "[CaseDefs] Loaded",
    ERRORLOADCASEDEFS = "[CaseDefs] Error Load"
}

export class LoadCaseDefsAction implements Action {
    type = ActionTypes.CASEDEFSLOAD
    constructor() { }
}

export class CaseDefsLoadedAction implements Action {
    type = ActionTypes.CASEDEFSLOADED
    constructor(public result: SearchCaseDefinitionsResult) { }
}

export type ActionsUnion = LoadCaseDefsAction | CaseDefsLoadedAction;