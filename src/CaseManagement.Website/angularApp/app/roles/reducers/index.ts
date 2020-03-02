import { createSelector } from '@ngrx/store';
import * as fromSearch from './search.reducer';
import * as fromGet from './get.reducer';

export interface RoleState {
    search: fromSearch.State;
    get: fromGet.State;
}

export const selectSearch = (state: RoleState) => state.search;
export const selectGet = (state: RoleState) => state.get;

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