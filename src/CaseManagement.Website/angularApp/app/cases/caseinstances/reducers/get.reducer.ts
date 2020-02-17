/*
import * as fromActions from '../../casedefinitions/actions/case-instances';
import { CaseInstance } from '../../casedefinitions/models/case-instance.model';

export interface State {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CaseInstance;
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
*/