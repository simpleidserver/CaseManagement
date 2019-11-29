import { Action } from '@ngrx/store';
import { CaseDefinition } from '../../casedefinitions/models/case-def.model';
import { CaseInstance } from '../../casedefinitions/models/search-case-instances-result.model';
import { SearchCaseExecutionStepsResult } from '../../casedefinitions/models/search-case-execution-steps-result.model';

export enum ActionTypes {
    CASEINSTANCELOAD = "[CaseInstance] Load",
    CASEINSTANCELOADED = "[CaseInstance] Loaded",
    ERRORLOADCASEINSTANCE = "[CaseInstance] Error Load",
    CASEEXECUTIONSSTEPSLOAD = "[CaseExecutionSteps] Load",
    CASEEXECUTIONSTEPSLOADED = "[CaseExecutionSteps] Loaded",
    ERRORLOADCASEEXECUTIONSTEPS = "[CaseExecutionSteps] Error Load"
}

export class CaseInstanceLoadedAction implements Action {
    type = ActionTypes.CASEINSTANCELOAD
    constructor(public caseInstance : CaseInstance, public caseDefinition : CaseDefinition) { }
}

export class CaseExecutionStepsLoadedAction implements Action {
    type = ActionTypes.CASEEXECUTIONSTEPSLOADED
    constructor(public result: SearchCaseExecutionStepsResult) { }
}

export type ActionsUnion = CaseInstanceLoadedAction | CaseExecutionStepsLoadedAction;