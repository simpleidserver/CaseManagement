import * as fromActions from '../actions/delegateconfiguration.actions';
import { DelegateConfiguration } from '../models/delegateconfiguration.model';
import { SearchDelegateConfigurationResult } from '../models/searchdelegateconfiguration.model';

export interface DelegateConfigurationLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchDelegateConfigurationResult;
    lstIds: string[];
}

export interface DelegateConfigurationState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: DelegateConfiguration;
}

export const initialDelegateConfigurationLstState: DelegateConfigurationLstState  = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false,
    lstIds: []
};

export const initialDelegateConfigurationState: DelegateConfigurationState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function delegateConfigurationLstReducer(state = initialDelegateConfigurationLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_DELEGATE_CONFIGURATION:
            state.content = action.content;
            return { ...state };
        case fromActions.ActionTypes.COMPLETE_GET_ALL_DELEGATE_CONFIGURATION:
            state.lstIds = action.content;
            return { ...state };
        default:
            return state;
    }
}

export function delegateConfigurationReducer(state = initialDelegateConfigurationState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_DELEGATE_CONFIGURATION:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}