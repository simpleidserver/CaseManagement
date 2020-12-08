import * as fromActions from '../actions/cmmn-files.actions';
import { CmmnFile } from "../models/cmmn-file.model";
import { SearchCmmnFilesResult } from "../models/search-cmmn-files-result.model";

export interface CmmnFileState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CmmnFile;
}

export interface CmmnFileLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCmmnFilesResult;
}

export const initialCmmnFileState: CmmnFileState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialCmmnFileLstState: CmmnFileLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function cmmnFileReducer(state = initialCmmnFileState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CMMNFILE:
            state.content = action.content;
            return { ...state };
        case fromActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE:
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
        case fromActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE_PAYLOAD:
            return {
                ...state,
                content: {
                    ...state.content,
                    payload: action.payload
                }
            };
        case fromActions.ActionTypes.COMPLETE_PUBLISH_CMMNFILE:
            return {
                ...state,
                content: {
                    ...state.content,
                    status: 'Published'
                }
            };
        default:
            return state;
    }
}

export function cmmnFileLstReducer(state = initialCmmnFileLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CMMNFILES:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}