import { ActionsUnion, ActionTypes, PerformancesLoadedAction } from './list-actions';
import { ListPerformancesState } from './list-states';

const initialCaseDefsAction: ListPerformancesState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function performancesReducer(state = initialCaseDefsAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.PERFORMANCESLOADED:
            let caseDefsLoadedAction = <PerformancesLoadedAction>action;
            state.content = caseDefsLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADPERFORMANCES:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}