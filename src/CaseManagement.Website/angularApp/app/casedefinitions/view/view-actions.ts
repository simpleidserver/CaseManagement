import { Action } from '@ngrx/store';
import { CaseDefinitionHistory } from '../models/case-definition-history.model';
import { CaseDefinition } from '../models/case-definition.model';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';
import { SearchCaseFormInstancesResult } from '../models/search-case-form-instances-result.model';
import { SearchCaseActivationsResult } from '../models/search-case-activations-result.model';

export enum ActionTypes {
    CASEDEFINITIONLOAD = "[CaseDefinition] Load",
    CASEDEFINITIONLOADED = "[CaseDefinition] Loaded",
    ERRORLOADCASEDEFINITION = "[CaseDefinition] Error Load",
    CASEINSTANCESLOAD = "[CaseInstances] Load",
    CASEINSTANCESLOADED = "[CaseInstances] Loaded",
    ERRORLOADCASEINSTANCES = "[CaseInstances] Error Load",
    CASEFORMINSTANCESLOAD = "[CaseFormInstances] Load",
    CASEFORMINSTANCESLOADED = "[CaseFormInstances] Loaded",
    ERRORLOADCASEFORMINSTANCES = "[CaseFormInstances] Error Load",
    CASEACTIVATIONSLOAD = "[CaseActivations] Load",
    CASEACTIVATIONSLOADED = "[CaseActivations] Loaded",
    ERRORLOADCASEACTIVATIONS = "[CaseActivations] Error Load"
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

export class LoadCaseFormInstancesAction implements Action {
    type = ActionTypes.CASEFORMINSTANCESLOAD
    constructor() { }
}

export class LoadCaseFormInstancesLoadedAction implements Action {
    type = ActionTypes.CASEFORMINSTANCESLOADED
    constructor(public result: SearchCaseFormInstancesResult) { }
}

export class LoadCaseFormActivationsAction implements Action {
    type = ActionTypes.CASEACTIVATIONSLOAD
    constructor() { }
}

export class LoadCaseActivationsLoadedAction implements Action {
    type = ActionTypes.CASEFORMINSTANCESLOADED
    constructor(public result: SearchCaseActivationsResult) { }
}

export type ActionsUnion = LoadCaseDefinitionAction | CaseDefinitionLoadedAction | LoadCaseInstancesAction | LoadCaseFormInstancesAction | LoadCaseFormInstancesLoadedAction | LoadCaseFormActivationsAction | LoadCaseActivationsLoadedAction;