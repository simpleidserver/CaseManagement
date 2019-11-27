import { ActionTypes, ActionsUnion, CaseDefLoadedAction, CaseInstancesLoadedAction } from './case-def-actions';
import { CaseDefState } from './case-def-states';

const initialCaseDefState : CaseDefState = {
    isCaseDefinitionLoading: true,
    isCaseDefinitionErrorLoadOccured: false,
    caseDefinitionContent: null,
    isCaseInstancesLoading: true,
    isCaseInstancesErrorLoadOccured: false,
    caseInstancesContent: null
};

export function reducer(state = initialCaseDefState, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEDEFLOADED:
            let caseDefLoadedAction = <CaseDefLoadedAction>action;
            state.caseDefinitionContent = caseDefLoadedAction.result;
            state.isCaseDefinitionLoading = false;
            state.isCaseDefinitionErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEDEF:
            state.isCaseDefinitionErrorLoadOccured = true;
            state.isCaseDefinitionLoading = false;
            return { ...state };
        case ActionTypes.CASEINSTANCESLOADED:
            let caseInstancesLoadedAction = <CaseInstancesLoadedAction>action;
            state.caseInstancesContent = caseInstancesLoadedAction.result;
            state.isCaseInstancesLoading = false;
            state.isCaseInstancesErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEINSTANCES:
            state.isCaseInstancesErrorLoadOccured = true;
            state.isCaseInstancesLoading = false;
            return { ...state };
        default:
            return state;
    }
}