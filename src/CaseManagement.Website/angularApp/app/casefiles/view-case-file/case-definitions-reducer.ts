import { CaseDefinitionsState } from './case-definitions-states';
import { ActionsUnion, ActionTypes, CaseDefinitionsLoaded } from './view-case-file-actions';

const initialCaseFileAction: CaseDefinitionsState = {
    caseDefinitions: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function reducer(state = initialCaseFileAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEDEFINITIONSLOADED:
            let caseDefsLoadedAction = <CaseDefinitionsLoaded>action;
            state.caseDefinitions = caseDefsLoadedAction.caseDefinitions;
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