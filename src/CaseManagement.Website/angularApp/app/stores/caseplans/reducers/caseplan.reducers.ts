import * as fromActions from '../actions/caseplan.actions';
import { CasePlan } from '../models/caseplan.model';
import { SearchCasePlanResult } from '../models/searchcaseplanresult.model';

export interface CasePlanState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CasePlan;
}

export interface SearchCasePlanState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCasePlanResult;
}

export interface SearchCasePlanHistoryState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCasePlanResult;
}

export const initialCasePlanState: CasePlanState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialSearchCasePlanState: SearchCasePlanState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialSearchCasePlanHistoryState: SearchCasePlanHistoryState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function casePlanReducer(state = initialCasePlanState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}

export function casePlanLstReducer(state = initialSearchCasePlanState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}

export function casePlanHistoryLstReducer(state = initialSearchCasePlanHistoryState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_HISTORY:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}