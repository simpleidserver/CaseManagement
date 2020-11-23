import * as fromActions from '../actions/bpmn-files.actions';
import { BpmnFile } from '../models/bpmn-file.model';
import { SearchBpmnFilesResult } from '../models/search-bpmn-files-result.model';

export interface BpmnFileLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchBpmnFilesResult;
}

export interface BpmnFileState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: BpmnFile;
}

export const initialBpmnFileLstState: BpmnFileLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialBpmnFileState: BpmnFileState = {
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

export function bpmnFileReducer(state = initialBpmnFileState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_BPMNFILE:
            state.content = action.bpmnFile;
            return { ...state };
        case fromActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE:
            return {
                ...state,
                content: {
                    ...state.content,
                    name: action.name,
                    description: action.description,
                    version: (state.content.version + 1),
                    updateDateTime: new Date()
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD:
            return {
                ...state,
                content: {
                    ...state.content,
                    version: (state.content.version + 1),
                    payload: action.payload,
                    updateDateTime: new Date()
                }
            };
        default:
            return state;
    }
}