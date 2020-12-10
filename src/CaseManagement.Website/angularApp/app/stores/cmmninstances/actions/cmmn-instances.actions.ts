import { Action } from '@ngrx/store';
import { SearchCasePlanInstanceResult } from '../models/searchcmmnplaninstanceresult.model';
import { CmmnPlanInstanceResult } from '../models/cmmn-planinstance.model';

export enum ActionTypes {
    LAUNCH_CMMN_PLANINSTANCE = "[CmmnInstances] LAUNCH_CMMN_PLANINSTANCE",
    ERROR_LAUNCH_CMMN_PLANINSTANCE = "[CmmnInstances] ERROR_LAUNCH_CMMN_PLANINSTANCE",
    COMPLETE_LAUNCH_CMMN_PLANINSTANCE = "[CmmnInstances] COMPLETE_LAUNCH_CMMN_PLANINSTANCE",
    SEARCH_CMMN_PLANINSTANCE = "[CmmnInstances] SEARCH_CMMN_PLANINSTANCE",
    ERROR_SEARCH_CMMN_PLANINSTANCE = "[CmmnInstances] ERROR_SEARCH_CMMN_PLANINSTANCE",
    COMPLETE_SEARCH_CMMN_PLANINSTANCE = "[CmmnInstances] COMPLETE_SEARCH_CMMN_PLANINSTANCE",
    GET_CMMN_PLANINSTANCE = "[CmmnInstances] GET_CMMN_PLANINSTANCE",
    ERROR_GET_CMMN_PLANINSTANCE = "[CmmnInstances] ERROR_GET_CMMN_PLANINSTANCE",
    COMPLETE_GET_CMMN_PLANINSTANCE = "[CmmnInstances] COMPLETE_GET_CMMN_PLANINSTANCE"
}

export class LaunchCmmnPlanInstance implements Action {
    readonly type = ActionTypes.LAUNCH_CMMN_PLANINSTANCE;
    constructor(public cmmnPlanId: string) { }
}

export class SearchCmmnPlanInstance implements Action {
    readonly type = ActionTypes.SEARCH_CMMN_PLANINSTANCE;
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public casePlanId: string) { }
}

export class CompleteSearchCmmnPlanInstances implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_CMMN_PLANINSTANCE;
    constructor(public content: SearchCasePlanInstanceResult) { }
}

export class GetCmmnPlanInstance implements Action {
    readonly type = ActionTypes.GET_CMMN_PLANINSTANCE;
    constructor(public cmmnPlanInstanceId: string) { }
}

export class CompleteGetCmmnPlanInstance implements Action {
    readonly type = ActionTypes.COMPLETE_GET_CMMN_PLANINSTANCE;
    constructor(public content: CmmnPlanInstanceResult) { }
}

export type ActionsUnion = LaunchCmmnPlanInstance |
    SearchCmmnPlanInstance |
    CompleteSearchCmmnPlanInstances |
    GetCmmnPlanInstance |
    CompleteGetCmmnPlanInstance;