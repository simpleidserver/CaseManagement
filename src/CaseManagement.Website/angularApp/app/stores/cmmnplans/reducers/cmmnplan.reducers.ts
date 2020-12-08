import * as fromActions from '../actions/cmmn-plans.actions';
import { CmmnPlan } from '../models/cmmn-plan.model';
import { SearchCmmnPlanResult } from '../models/searchcmmnplanresult.model';

export interface CmmnPlanState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CmmnPlan;
}

export interface SearchCmmnPlanState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCmmnPlanResult;
}

export const initialCmmnPlanState: CmmnPlanState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialSearchCmmnPlanState: SearchCmmnPlanState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function cmmnPlanReducer(state = initialCmmnPlanState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CMMN_PLAN:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}

export function cmmnPlanLstReducer(state = initialSearchCmmnPlanState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CMMN_PLANS:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}