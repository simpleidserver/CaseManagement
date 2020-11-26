import { SearchBpmnInstancesResult } from '../models/search-bpmn-instances-result.model';
import * as fromActions from '../actions/bpmn-instances.actions';
import { BpmnInstance } from '../models/bpmn-instance.model';

export interface BpmnInstanceLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchBpmnInstancesResult;
}

export interface BpmnInstanceState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: BpmnInstance;
}

export const initialBpmnInstancesLstState: BpmnInstanceLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialBpmnInstanceState: BpmnInstanceState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function bpmnInstanceLstReducer(state = initialBpmnInstancesLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_BPMNINSTANCES:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}

export function bpmnInstanceReducer(state = initialBpmnInstanceState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_BPMNINSTANCE:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}