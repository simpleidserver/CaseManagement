import { ActionsUnion, ActionTypes, CaseDefinitionsLoadedAction } from './list-actions';
import { ListCaseDefinitionsState } from './list-states';

const initialCaseDefsAction: ListCaseDefinitionsState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function reducer(state = initialCaseDefsAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEDEFINITIONSLOADED:
            let caseDefsLoadedAction = <CaseDefinitionsLoadedAction>action;
            state.content = caseDefsLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEDEFINITIONS:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}