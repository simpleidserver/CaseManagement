import { ActionsUnion, ActionTypes, CaseDefinitionLoadedAction, CaseInstancesLoadedAction } from './view-actions';
import { ViewCaseDefinitionState, ViewCaseInstancesState } from './view-states';

const initialCaseDefAction: ViewCaseDefinitionState = {
    caseDefinition: null,
    caseFile: null,
    caseDefinitionHistory: null,
    isLoading: true,
    isErrorLoadOccured: false
};

const initialCaseInstancesAction: ViewCaseInstancesState = {
    content: null,
    isErrorLoadOccured: false,
    isLoading: true
};

export function caseDefinitionReducer(state = initialCaseDefAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEDEFINITIONLOADED:
            let caseDefsLoadedAction = <CaseDefinitionLoadedAction>action;
            state.caseDefinition = caseDefsLoadedAction.caseDefinition;
            state.caseFile = caseDefsLoadedAction.caseFile;
            state.caseDefinitionHistory = caseDefsLoadedAction.caseDefinitionHistory;
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

export function caseInstancesReducer(state = initialCaseInstancesAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEINSTANCESLOADED:
            let caseInstancesLoadedAction = <CaseInstancesLoadedAction>action;
            state.content = caseInstancesLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEINSTANCES:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}