import { ActionTypes, ActionsUnion, CaseDefsLoadedAction } from './list-case-defs-actions';
import { ListCaseDefsState } from './list-case-defs-states';

const initialCaseDefsAction: ListCaseDefsState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function reducer(state = initialCaseDefsAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEDEFSLOADED:
            let caseDefsLoadedAction = <CaseDefsLoadedAction>action;
            state.content = caseDefsLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEDEFS:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}