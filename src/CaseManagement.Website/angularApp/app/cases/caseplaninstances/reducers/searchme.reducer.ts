import * as fromActions from '../actions/caseplaninstance';
import { SearchCasePlanInstanceResult } from "../models/searchcaseplaninstanceresult.model";

export interface State {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCasePlanInstanceResult;
}

export const initialState: State = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function searchReducer(state = initialState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_ME:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}