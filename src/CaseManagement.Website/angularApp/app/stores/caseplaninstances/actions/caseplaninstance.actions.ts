import { Action } from '@ngrx/store';
import { CasePlanInstanceResult } from '../models/caseplaninstance.model';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';

export enum ActionTypes {
    START_SEARCH_CASE_PLANINSTANCES = "[CasePlanInstance] START_SEARCH_CASE_PLANINSTANCES",
    COMPLETE_SEARCH_CASE_PLANINSTANCES = "[CasePlanInstance] COMPLETE_SEARCH_CASE_PLANINSTANCES",
    ERROR_SEARCH_CASE_PLANINSTANCES = "[CasePlanInstance] ERROR_SEARCH_CASE_PLANINSTANCES",
    START_GET_CASE_PLANINSTANCE = "[CasePlanInstance] START_GET_CASE_PLANINSTANCE",
    COMPLETE_GET_CASE_PLANINSTANCE = "[CasePlanInstance] COMPLETE_GET_CASE_PLANINSTANCE",
    ERROR_GET_CASE_PLANINSTANCE = "[CasePlanInstance] ERROR_GET_CASE_PLANINSTANCE",
    LAUNCH_CASE_PLANINSTANCE = "[CasePlanInstance] LAUNCH_CASE_PLANINSTANCE",
    ERROR_LAUNCH_CASE_PLANINSTANCE = "[CasePlanInstance] ERROR_LAUNCH_CASE_PLANINSTANCE",
    COMPLETE_LAUNCH_CASE_PLANINSTANCE = "[CasePlanInstance] COMPLETE_LAUNCH_CASE_PLANINSTANCE",
    REACTIVATE_CASE_PLANINSTANCE = "[CasePlanInstance] REACTIVATE_CASE_PLANINSTANCE",
    ERROR_REACTIVATE_CASE_PLANINSTANCE = "[CasePlanInstance] ERROR_REACTIVATE_CASE_PLANINSTANCE",
    COMPLETE_REACTIVATE_CASE_PLANINSTANCE = "[CasePlanInstance] COMPLETE_REACTIVATE_CASE_PLANINSTANCE",
    SUSPEND_CASE_PLANINSTANCE = "[CasePlanInstance] SUSPEND_CASE_PLANINSTANCE",
    ERROR_SUSPEND_CASE_PLANINSTANCE = "[CasePlanInstance] ERROR_SUSPEND_CASE_PLANINSTANCE",
    COMPLETE_SUSPEND_CASE_PLANINSTANCE = "[CasePlanInstance] COMPLETE_SUSPEND_CASE_PLANINSTANCE",
    RESUME_CASE_PLANINSTANCE = "[CasePlanInstance] RESUME_CASE_PLANINSTANCE",
    ERROR_RESUME_CASE_PLANINSTANCE = "[CasePlanInstance] ERROR_RESUME_CASE_PLANINSTANCE",
    COMPLETE_RESUME_CASE_PLANINSTANCE = "[CasePlanInstance] COMPLETE_RESUME_CASE_PLANINSTANCE",
    CLOSE_CASE_PLANINSTANCE = "[CasePlanInstance] CLOSE_CASE_PLANINSTANCE",
    ERROR_CLOSE_CASE_PLANINSTANCE = "[CasePlanInstance] ERROR_CLOSE_CASE_PLANINSTANCE",
    COMPLETE_CLOSE_CASE_PLANINSTANCE = "[CasePlanInstance] COMPLETE_CLOSE_CASE_PLANINSTANCE",
    ENABLE_CASE_PLANINSTANCE_ELT = "[CasePlanInstance] ENABLE_CASE_PLANINSTANCE_ELT",
    ERROR_ENABLE_CASE_PLANINSTANCE_ELT = "[CasePlanInstance] ERROR_ENABLE_CASE_PLANINSTANCE_ELT",
    COMPLETE_ENABLE_CASE_PLANINSTANCE_ELT = "[CasePlanInstance] COMPLETE_ENABLE_CASE_PLANINSTANCE_ELT"

}

export class SearchCasePlanInstances implements Action {
    readonly type = ActionTypes.START_SEARCH_CASE_PLANINSTANCES
    constructor(public startIndex: number, public count: number, public order: string, public direction: string, public casePlanId: string) { }
}

export class SearchCasePlanInstancesComplete implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_CASE_PLANINSTANCES;
    constructor(public content: SearchCasePlanInstanceResult) { }
}

export class GetCasePlanInstance implements Action {
    readonly type = ActionTypes.START_GET_CASE_PLANINSTANCE;
    constructor(public id: string) { }
}

export class GetCasePlanInstanceComplete implements Action {
    readonly type = ActionTypes.COMPLETE_GET_CASE_PLANINSTANCE;
    constructor(public content: CasePlanInstanceResult) { }
}

export class LaunchCasePlanInstance implements Action {
    readonly type = ActionTypes.LAUNCH_CASE_PLANINSTANCE;
    constructor(public casePlanId: string) { }
}

export class ReactivateCasePlanInstance implements Action {
    readonly type = ActionTypes.REACTIVATE_CASE_PLANINSTANCE;
    constructor(public casePlanInstanceId: string) { }
}

export class SuspendCasePlanInstance implements Action {
    readonly type = ActionTypes.SUSPEND_CASE_PLANINSTANCE;
    constructor(public casePlanInstanceId: string) { }
}

export class ResumeCasePlanInstance implements Action {
    readonly type = ActionTypes.RESUME_CASE_PLANINSTANCE;
    constructor(public casePlanInstanceId: string) { }
}

export class CloseCasePlanInstance implements Action {
    readonly type = ActionTypes.CLOSE_CASE_PLANINSTANCE;
    constructor(public casePlanInstanceId: string) { }
}

export class EnableCasePlanInstanceElt implements Action {
    readonly type = ActionTypes.ENABLE_CASE_PLANINSTANCE_ELT;
    constructor(public casePlanInstanceId: string, public casePlanInstanceEltId: string) { }
}

export type ActionsUnion = SearchCasePlanInstances |
    SearchCasePlanInstancesComplete |
    GetCasePlanInstance |
    GetCasePlanInstanceComplete |
    LaunchCasePlanInstance |
    ReactivateCasePlanInstance |
    SuspendCasePlanInstance |
    ResumeCasePlanInstance |
    CloseCasePlanInstance |
    EnableCasePlanInstanceElt;