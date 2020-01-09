import { CaseDefinitionState } from './case-definition-states';
import { ActionsUnion, ActionTypes, CaseDefinitionLoaded } from './view-case-definition-actions';

const initialCaseDefinitionAction: CaseDefinitionState = {
    caseDefinition: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function reducer(state = initialCaseDefinitionAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEDEFINITIONLOADED:
            let caseDefsLoadedAction = <CaseDefinitionLoaded>action;
            state.caseDefinition = caseDefsLoadedAction.caseDefinition;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEDEFINITION:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}