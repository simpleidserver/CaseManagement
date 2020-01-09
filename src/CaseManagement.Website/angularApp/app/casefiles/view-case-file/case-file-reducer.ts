import { CaseFileState } from './case-file-states';
import { ActionsUnion, ActionTypes, CaseFileLoaded } from './view-case-file-actions';

const initialCaseFileAction: CaseFileState = {
    caseFile: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function reducer(state = initialCaseFileAction, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEFILELOADED:
            let caseDefsLoadedAction = <CaseFileLoaded>action;
            state.caseFile = caseDefsLoadedAction.caseFile;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEFILE:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return { ...state };
        default:
            return state;
    }
}