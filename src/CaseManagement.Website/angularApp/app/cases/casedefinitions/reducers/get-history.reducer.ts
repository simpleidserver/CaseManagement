import * as fromActions from '../actions/case-definitions';
import { CaseDefinitionHistory } from "../models/case-definition-history.model";

export interface State {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CaseDefinitionHistory;
}

export const initialState: State = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function getReducer(state = initialState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_HISTORY:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}