import * as fromActions from '../actions/case-definitions';
import { CaseDefinition } from "../models/case-definition.model";

export interface State {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CaseDefinition;
}

export const initialState: State = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function getReducer(state = initialState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}