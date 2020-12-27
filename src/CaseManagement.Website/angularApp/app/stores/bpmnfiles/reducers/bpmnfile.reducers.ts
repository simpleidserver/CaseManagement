import * as fromActions from '../actions/bpmn-files.actions';
import { BpmnFile } from '../models/bpmn-file.model';
import { SearchBpmnFilesResult } from '../models/search-bpmn-files-result.model';
import { HumanTaskDef } from '../../humantaskdefs/models/humantaskdef.model';

export interface BpmnFileLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchBpmnFilesResult;
}

export interface BpmnFileState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: BpmnFile;
    humanTaskDefs: HumanTaskDef[];
}

export const initialBpmnFileLstState: BpmnFileLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialBpmnFileState: BpmnFileState = {
    content: null,
    humanTaskDefs: [],
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
            state.humanTaskDefs = action.humanTaskDefs;
            return { ...state };
        case fromActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE:
            return {
                ...state,
                content: {
                    ...state.content,
                    name: action.name,
                    description: action.description,
                    payload: action.payload,
                    updateDateTime: new Date()
                }
            };
        case fromActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD:
            return {
                ...state,
                content: {
                    ...state.content,
                    payload: action.payload,
                    updateDateTime: new Date()
                }
            };
        default:
            return state;
    }
}