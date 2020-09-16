import * as fromActions from '../actions/case-files.actions';
import { CaseFile } from "../models/case-file.model";
import { SearchCaseFilesResult } from "../models/search-case-files-result.model";

export interface CaseFileState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: CaseFile;
}

export interface CaseFileLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCaseFilesResult;
}

export interface CaseFileHistoryLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCaseFilesResult;
}

export const initialCaseFileState: CaseFileState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialCaseFileLstState: CaseFileLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialCaseFileHistoryLstState: CaseFileHistoryLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function caseFileReducer(state = initialCaseFileState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CASEFILE:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}

export function caseFileLstReducer(state = initialCaseFileLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CASEFILES:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}

export function caseFileHistoryLstReducer(state = initialCaseFileHistoryLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CASEFILES_HISTORY:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}