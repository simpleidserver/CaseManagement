import { ActionsUnion, ActionTypes, CaseDefinitionLoadedAction, CaseInstancesLoadedAction, LoadCaseFormInstancesLoadedAction, LoadCaseActivationsLoadedAction } from './view-actions';
import { ViewCaseDefinitionState, ViewCaseInstancesState, ViewFormInstancesState, ViewCaseActivationsState } from './view-states';

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

const initialFormInstancesAction: ViewFormInstancesState = {
    content: null,
    isErrorLoadOccured: false,
    isLoading : true
};

const initialCaseActivationsAction: ViewCaseActivationsState = {
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

export function formInstancesReducer(state = initialFormInstancesAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEFORMINSTANCESLOADED:
            let caseInstancesLoadedAction = <LoadCaseFormInstancesLoadedAction>action;
            state.content = caseInstancesLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEFORMINSTANCES:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}

export function caseActivationsReducer(state = initialCaseActivationsAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEACTIVATIONSLOADED:
            let caseActivationsLoadedAction = <LoadCaseActivationsLoadedAction>action;
            state.content = caseActivationsLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEACTIVATIONS:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}