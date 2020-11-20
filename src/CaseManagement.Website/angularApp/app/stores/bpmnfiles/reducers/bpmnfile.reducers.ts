import * as fromActions from '../actions/bpmn-files.actions';
import { SearchBpmnFilesResult } from '../models/search-bpmn-files-result.model';

export interface BpmnFileLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchBpmnFilesResult;
}

export const initialBpmnFileLstState: BpmnFileLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function bpmnFileLstReducer(state = initialBpmnFileLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_BPMNFILES:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}