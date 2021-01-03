import * as fromActions from '../actions/cases.actions';
import { SearchCaseInstanceResult } from '../models/search-caseinstance.model';

export interface CaseLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCaseInstanceResult;
}

export const initialCaseLstState: CaseLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function caseLstReducer(state = initialCaseLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CASES:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}