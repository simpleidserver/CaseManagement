import { Action } from '@ngrx/store';
import { SearchPerformancesResult } from '../models/search-performances-result.model';

export enum ActionTypes {
    PERFORMANCESLOAD = "[Performances] Load",
    PERFORMANCESLOADED = "[Performances] Loaded",
    ERRORLOADPERFORMANCES = "[CaseDefinitions] Error Load"
}

export class LoadPerformancesAction implements Action {
    type = ActionTypes.PERFORMANCESLOAD
    constructor() { }
}

export class PerformancesLoadedAction implements Action {
    type = ActionTypes.PERFORMANCESLOADED
    constructor(public result: SearchPerformancesResult) { }
}

export type ActionsUnion = LoadPerformancesAction | PerformancesLoadedAction;