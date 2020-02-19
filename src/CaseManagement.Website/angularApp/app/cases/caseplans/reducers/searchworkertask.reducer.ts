import * as fromActions from '../actions/caseworker';
import { SearchWorkerTaskResult } from '../models/searchworkertaskresult.model';

export interface State {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchWorkerTaskResult;
}

export const initialState: State = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function searchReducer(state = initialState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}