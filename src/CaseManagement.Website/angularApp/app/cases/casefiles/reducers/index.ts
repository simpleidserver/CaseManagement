import { createSelector } from '@ngrx/store';
import * as fromGet from './get.reducer';
import * as fromSearch from './search.reducer';

export interface CaseFilesState {
    search: fromSearch.State;
    get: fromGet.State
}

export const selectSearch = (state: CaseFilesState) => state.search;
export const selectGet = (state: CaseFilesState) => state.get;

export const selectSearchResults = createSelector(
    selectSearch,
    (state: fromSearch.State) => {
        if (!state || state.content == null) {
            return [];
        }

        return state.content.Content;
    }
);

export const selectGetResult = createSelector(
    selectGet,
    (state: fromGet.State) => {
        if (!state) {
            return null;
        }

        return state.content;
    }
);

export const appReducer = {
    search: fromSearch.searchReducer,
    get: fromGet.getReducer
};