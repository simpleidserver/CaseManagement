import * as fromActions from '../actions/cmmn-instances.actions';
import { CmmnPlanInstanceResult } from '../models/cmmn-planinstance.model';
import { SearchCasePlanInstanceResult } from '../models/searchcmmnplaninstanceresult.model';

export interface CmmnPlanInstanceState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CmmnPlanInstanceResult;
}

export interface CmmnPlanInstanceLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCasePlanInstanceResult;
}

export const initialCmmnPlanInstanceState: CmmnPlanInstanceState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialCmmnPlanInstanceLstState: CmmnPlanInstanceLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function cmmnPlanInstanceReducer(state = initialCmmnPlanInstanceState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CMMN_PLANINSTANCE:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}

export function cmmnPlanInstanceLstReducer(state = initialCmmnPlanInstanceLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CMMN_PLANINSTANCE:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}