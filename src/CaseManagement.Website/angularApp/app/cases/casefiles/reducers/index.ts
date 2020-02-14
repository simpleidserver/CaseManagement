import { createSelector } from '@ngrx/store';
import * as fromGet from './get.reducer';
import * as fromSearch from './search.reducer';
import * as fromSearchHistory from './search-history.reducer';

export interface CaseFilesState {
    search: fromSearch.State;
    get: fromGet.State;
    searchHistory: fromSearchHistory.State
}

export const selectSearch = (state: CaseFilesState) => state.search;
export const selectGet = (state: CaseFilesState) => state.get;
export const selectSearchHistory = (state: CaseFilesState) => state.searchHistory;

export const selectSearchResults = createSelector(
    selectSearch,
    (state: fromSearch.State) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
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

export const selectSearchHistoryResult = createSelector(
    selectSearchHistory,
    (state: fromSearchHistory.State) => {
        if (!state) {
            return null;
        }

        return state.content;
    }
);

export const appReducer = {
    search: fromSearch.searchReducer,
    get: fromGet.getReducer,
    searchHistory: fromSearchHistory.searchHistoryReducer
};