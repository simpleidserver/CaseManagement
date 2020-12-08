import { Action } from '@ngrx/store';
import { CmmnPlan } from '../models/cmmn-plan.model';
import { SearchCmmnPlanResult } from '../models/searchcmmnplanresult.model';

export enum ActionTypes {
    SEARCH_CMMN_PLANS = "[CmmnPlans] SEARCH_CMMN_PLANS",
    COMPLETE_SEARCH_CMMN_PLANS = "[CmmnPlans] COMPLETE_SEARCH_CMMN_PLANS",
    ERROR_SEARCH_CMMN_PLANS = "[CmmnPlans] ERROR_SEARCH_CMMN_PLANS",
    GET_CMMN_PLAN = "[CmmnPlans] START_GET",
    COMPLETE_GET_CMMN_PLAN = "[CmmnPlans] COMPLETE_GET_CMMN_PLAN",
    ERROR_GET_CMMN_PLAN = "[CmmnPlans] ERROR_GET_CMMN_PLAN"
}

export class SearchCmmnPlans implements Action {
    readonly type = ActionTypes.SEARCH_CMMN_PLANS
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public caseFileId: string) { }
}

export class CompleteSearchCmmnPlans implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_CMMN_PLANS;
    constructor(public content: SearchCmmnPlanResult) { }
}

export class GetCmmnPlan implements Action {
    readonly type = ActionTypes.GET_CMMN_PLAN
    constructor(public id: string) { }
}

export class CompleteGet implements Action {
    readonly type = ActionTypes.COMPLETE_GET_CMMN_PLAN;
    constructor(public content: CmmnPlan) { }
}

export type ActionsUnion = SearchCmmnPlans |
    CompleteSearchCmmnPlans |
    GetCmmnPlan |
    CompleteGet;