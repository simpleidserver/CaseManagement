import { createSelector } from '@ngrx/store';
import * as fromSearch from './search.reducer';
import * as fromGet from './get.reducer';
export var selectSearch = function (state) { return state.search; };
export var selectGet = function (state) { return state.get; };
export var selectSearchResults = createSelector(selectSearch, function (state) {
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
    get: fromGet.getReducer
};
//# sourceMappingURL=index.js.map