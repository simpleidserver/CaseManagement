import { Action } from '@ngrx/store';
import { CaseDefinitionHistory } from '../models/case-definition-history.model';
import { CaseDefinition } from '../models/case-definition.model';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';

export enum ActionTypes {
    CASEDEFINITIONLOAD = "[CaseDefinition] Load",
    CASEDEFINITIONLOADED = "[CaseDefinition] Loaded",
    ERRORLOADCASEDEFINITION = "[CaseDefinition] Error Load",
    CASEINSTANCESLOAD = "[CaseInstances] Load",
    CASEINSTANCESLOADED = "[CaseInstances] Loaded",
    ERRORLOADCASEINSTANCES = "[CaseInstances] Error Load"
}

export class LoadCaseDefinitionAction implements Action {
    type = ActionTypes.CASEDEFINITIONLOAD
    constructor() { }
}

export class CaseDefinitionLoadedAction implements Action {
    type = ActionTypes.CASEDEFINITIONLOADED
    constructor(public caseDefinition: CaseDefinition, public caseFile: CaseFile, public caseDefinitionHistory: CaseDefinitionHistory) { }
}

export class LoadCaseInstancesAction implements Action {
    type = ActionTypes.CASEINSTANCESLOAD
    constructor() { }
}

export class CaseInstancesLoadedAction implements Action {
    type = ActionTypes.CASEINSTANCESLOADED
    constructor(public result: SearchCaseInstancesResult) { }
}

export type ActionsUnion = LoadCaseDefinitionAction | CaseDefinitionLoadedAction | LoadCaseInstancesAction;