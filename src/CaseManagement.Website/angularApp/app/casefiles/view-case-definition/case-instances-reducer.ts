import { CaseInstancesState } from './case-instances-states';
import { ActionsUnion, ActionTypes, CaseInstancesLoaded } from './view-case-definition-actions';

const initialCaseInstancesAction: CaseInstancesState = {
    caseInstances: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function reducer(state = initialCaseInstancesAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEINSTANCESLOADED:
            let caseDefsLoadedAction = <CaseInstancesLoaded>action;
            state.caseInstances = caseDefsLoadedAction.caseInstances;
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