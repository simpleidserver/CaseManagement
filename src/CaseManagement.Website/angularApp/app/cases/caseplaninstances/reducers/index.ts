import { createSelector } from '@ngrx/store';
import * as fromSearch from './search.reducer';
import * as fromSearchMe from './search.reducer';

export interface CasePlanInstanceState {
    search: fromSearch.State,
    searchMe: fromSearchMe.State
}

export const selectSearch = (state: CasePlanInstanceState) => state.search;
export const selectSearchMe = (state: CasePlanInstanceState) => state.searchMe;

export const selectSearchResult = createSelector(
    selectSearch,
    (state: fromSearch.State) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }

);
export const selectSearchMeResult = createSelector(
    selectSearchMe,
    (state: fromSearch.State) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const appReducer = {
    search: fromSearch.searchReducer,
    searchMe: fromSearchMe.searchReducer
};