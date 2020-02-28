import { createSelector } from '@ngrx/store';
import * as fromGet from './get.reducer';
import * as fromSearch from './search.reducer';

export interface CasePlanInstanceState {
    search: fromSearch.State,
    get: fromGet.State
}

export const selectSearch = (state: CasePlanInstanceState) => state.search;
export const selectGet = (state: CasePlanInstanceState) => state.get;

export const selectSearchResult = createSelector(
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
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const appReducer = {
    search: fromSearch.searchReducer,
    get: fromGet.getReducer
};