import { ActionsUnion, ActionTypes, CaseInstanceLoadedAction } from './view-actions';
import { ViewCaseInstanceState } from './view-states';

const initialCaseInstanceAction: ViewCaseInstanceState = {
    caseInstance: null,
    caseDefinition: null,
    caseFile: null,
    isErrorLoadOccured: false,
    isLoading: true
};

export function caseInstanceReducer(state = initialCaseInstanceAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEINSTANCELOADED:
            let caseDefsLoadedAction = <CaseInstanceLoadedAction>action;
            state.caseInstance = caseDefsLoadedAction.caseInstance;
            state.caseDefinition = caseDefsLoadedAction.caseDefinition;
            state.caseFile = caseDefsLoadedAction.caseFile;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEINSTANCE:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}