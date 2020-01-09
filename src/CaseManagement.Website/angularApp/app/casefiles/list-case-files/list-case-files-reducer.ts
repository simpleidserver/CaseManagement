import { ActionsUnion, ActionTypes, CaseFilesLoadedAction } from './list-case-files-actions';
import { ListCaseFilesState } from './list-case-files-states';

const initialCaseDefsAction: ListCaseFilesState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function reducer(state = initialCaseDefsAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEFILESLOADED:
            let caseDefsLoadedAction = <CaseFilesLoadedAction>action;
            state.content = caseDefsLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEFILES:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}