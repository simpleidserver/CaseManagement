import { Action } from '@ngrx/store';
import { SearchCaseInstanceResult } from '../models/search-caseinstance.model';
import { CaseInstance } from '../models/caseinstance.model';

export enum ActionTypes {
    SEARCH_CASES = "[Cases] SEARCH_CASES",
    ERROR_SEARCH_CASES = "[Cases] ERROR_SEARCH_CASES",
    COMPLETE_SEARCH_CASES = "[Cases] COMPLETE_SEARCH_CASES",
    GET_CASE = "[Cases] GET_CASE",
    ERROR_GET_CASE = "[Cases] ERROR_GET_CASE",
    COMPLETE_GET_CASE = "[Cases] COMPLETE_GET_CASE",
    ACTIVATE = "[Cases] ACTIVATE",
    COMPLETE_ACTIVATE = "[Cases] COMPLETE_ACTIVATE",
    ERROR_ACTIVATE = "[Cases] ERROR_ACTIVATE",
    DISABLE = "[Cases] DISABLE",
    COMPLETE_DISABLE = "[Cases] COMPLETE_DISABLE",
    ERROR_DISABLE = "[Cases] ERROR_DISABLE",
    REENABLE = "[Cases] REENABLE",
    COMPLETE_REENABLE = "[Cases] COMPLETE_REENABLE",
    ERROR_REENABLE = "[Cases] ERROR_REENABLE",
    COMPLETE = "[Cases] COMPLETE",
    COMPLETED = "[Cases] COMPLETED",
    ERROR_COMPLETE = "[Cases] ERROR_COMPLETE"
}

export class SearchCases implements Action {
    readonly type = ActionTypes.SEARCH_CASES;
    constructor(public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearchCases implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_CASES;
    constructor(public content: SearchCaseInstanceResult) { }
}

export class GetCase implements Action {
    readonly type = ActionTypes.GET_CASE
    constructor(public id: string) { }
}

export class CompleteGetCase implements Action {
    readonly type = ActionTypes.COMPLETE_GET_CASE;
    constructor(public content: CaseInstance) { }
}

export class Activate implements Action {
    readonly type = ActionTypes.ACTIVATE
    constructor(public id: string, public elt: string) { }
}

export class Disable implements Action {
    readonly type = ActionTypes.DISABLE
    constructor(public id: string, public elt: string) { }
}

export class Reenable implements Action {
    readonly type = ActionTypes.REENABLE
    constructor(public id: string, public elt: string) { }
}

export class Complete implements Action {
    readonly type = ActionTypes.COMPLETE
    constructor(public id: string, public elt: string) { }
}

export type ActionsUnion = SearchCases |
    CompleteSearchCases |
    GetCase |
    CompleteGetCase |
    Activate |
    Disable |
    Reenable |
    Complete;