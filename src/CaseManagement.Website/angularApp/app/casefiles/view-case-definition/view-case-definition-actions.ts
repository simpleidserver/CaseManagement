import { Action } from '@ngrx/store';
import { CaseDefinition } from '../models/case-definition.model';
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';

export enum ActionTypes {
    LOADCASEDEFINITION = "[CaseDefinition] Load",
    CASEDEFINITIONLOADED = "[CaseDefinition] Loaded",
    ERRORLOADCASEDEFINITION = "[CaseDefinition] Error Load",
    LOADCASEINSTANCES = "[CaseInstances] Load",
    CASEINSTANCESLOADED = "[CaseInstances] Loaded",
    ERRORLOADCASEINSTANCES = "[CaseInstances] Error Load"
}

export class LoadCaseInstances implements Action {
    type = ActionTypes.LOADCASEINSTANCES
    constructor() { }
}

export class CaseInstancesLoaded implements Action {
    type = ActionTypes.CASEINSTANCESLOADED
    constructor(public caseInstances: SearchCaseInstancesResult) { }
}

export class LoadCaseDefinition implements Action {
    type = ActionTypes.LOADCASEDEFINITION
    constructor() { }
}

export class CaseDefinitionLoaded implements Action {
    type = ActionTypes.CASEDEFINITIONLOADED
    constructor(public caseDefinition: CaseDefinition) { }
}

export type ActionsUnion = LoadCaseInstances | CaseInstancesLoaded | LoadCaseDefinition | CaseDefinitionLoaded;