import { createSelector } from '@ngrx/store';
import * as fromGet from './get.reducer';
import * as fromSearch from './search.reducer';
import * as fromSearchHistory from './search-history.reducer';
export var selectSearch = function (state) { return state.search; };
export var selectGet = function (state) { return state.get; };
export var selectSearchHistory = function (state) { return state.searchHistory; };
export var selectSearchResults = createSelector(selectSearch, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectGetResult = createSelector(selectGet, function (state) {
    if (!state) {
        return null;
    }
    return state.content;
});
export var selectSearchHistoryResult = createSelector(selectSearchHistory, function (state) {
    if (!state) {
        return null;
    }
    return state.content;
});
export var appReducer = {
    search: fromSearch.searchReducer,
    get: fromGet.getReducer,
    searchHistory: fromSearchHistory.searchHistoryReducer
};
//# sourceMappingURL=index.js.map