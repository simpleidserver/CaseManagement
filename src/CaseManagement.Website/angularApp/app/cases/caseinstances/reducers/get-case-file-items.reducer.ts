import * as fromActions from '../../casedefinitions/actions/case-instances';
import { CaseFileItem } from '../../casedefinitions/models/case-file-item.model';

export interface State {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CaseFileItem[];
}

export const initialState: State = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function getReducer(state = initialState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_FILE_ITEMS:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}