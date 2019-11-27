import { Action } from '@ngrx/store';
import { CaseDefinition } from '../models/case-def.model';
import { SearchCaseInstancesResult } from '../models/search-case-instances-result.model';

export enum ActionTypes {
    CASEDEFLOAD = "[CaseDef] Load",
    CASEDEFLOADED = "[CaseDef] Loaded",
    ERRORLOADCASEDEF = "[CaseDef] Error Load",
    CASEINSTANCESLOAD = "[CaseInstances] Load",
    CASEINSTANCESLOADED = "[CaseInstances] Loaded",
    ERRORLOADCASEINSTANCES = "[CaseInstances] Error Load"
}

export class LoadCaseDefAction implements Action {
    type = ActionTypes.CASEDEFLOAD
    constructor() { }
}

export class CaseDefLoadedAction implements Action {
    type = ActionTypes.CASEDEFLOADED
    constructor(public result: CaseDefinition) { }
}

export class LoadCaseInstancesAction implements Action {
    type = ActionTypes.CASEINSTANCESLOAD
    constructor() { }
}

export class CaseInstancesLoadedAction implements Action {
    type = ActionTypes.CASEINSTANCESLOADED
    constructor(public result: SearchCaseInstancesResult) { }
}

export type ActionsUnion = LoadCaseDefAction | CaseDefLoadedAction | LoadCaseInstancesAction | CaseInstancesLoadedAction;