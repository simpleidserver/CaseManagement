import * as fromActions from '../actions/caseplaninstance.actions';
import { CasePlanInstanceResult } from '../models/caseplaninstance.model';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';

export interface CasePlanInstanceState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CasePlanInstanceResult;
}

export interface CasePlanInstanceLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCasePlanInstanceResult;
}

export const initialCasePlanInstanceState: CasePlanInstanceState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialCasePlanInstanceLstState: CasePlanInstanceLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function casePlanInstanceReducer(state = initialCasePlanInstanceState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CASE_PLANINSTANCE:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
};

export function casePlanInstanceLstReducer(state = initialCasePlanInstanceLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CASE_PLANINSTANCES:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}