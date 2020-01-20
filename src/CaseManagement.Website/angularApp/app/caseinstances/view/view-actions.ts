import { Action } from '@ngrx/store';
import { CaseInstance } from '../../casedefinitions/models/case-instance.model';
import { CaseDefinition } from '../../casedefinitions/models/case-definition.model';
import { CaseFile } from '../../casedefinitions/models/case-file.model';
import { CaseFileItem } from '../../casedefinitions/models/case-file-item.model';

export enum ActionTypes {
    CASEINSTANCELOAD = "[CaseInstance] Load",
    CASEINSTANCELOADED = "[CaseInstance] Loaded",
    ERRORLOADCASEINSTANCE = "[CaseInstance] Error Load"
}

export class LoadCaseInstanceAction implements Action {
    type = ActionTypes.CASEINSTANCELOAD
    constructor() { }
}

export class CaseInstanceLoadedAction implements Action {
    type = ActionTypes.CASEINSTANCELOADED
    constructor(public caseInstance: CaseInstance, public caseDefinition: CaseDefinition, public caseFile: CaseFile, public caseFileItems : CaseFileItem[]) { }
}

export type ActionsUnion = LoadCaseInstanceAction | CaseInstanceLoadedAction;