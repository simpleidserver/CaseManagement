import { createSelector } from '@ngrx/store';
import * as fromGet from './get.reducer';
import * as fromSearch from './search.reducer';
import * as fromSearchMe from './search.reducer';
export var selectSearch = function (state) { return state.search; };
export var selectSearchMe = function (state) { return state.searchMe; };
export var selectGet = function (state) { return state.get; };
export var selectSearchResult = createSelector(selectSearch, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectSearchMeResult = createSelector(selectSearchMe, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectGetResult = createSelector(selectGet, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var appReducer = {
    search: fromSearch.searchReducer,
    searchMe: fromSearchMe.searchReducer,
    get: fromGet.getReducer
};
//# sourceMappingURL=index.js.map