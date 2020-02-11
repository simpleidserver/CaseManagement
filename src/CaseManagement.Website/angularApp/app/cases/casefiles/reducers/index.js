import { createSelector } from '@ngrx/store';
import * as fromGet from './get.reducer';
import * as fromSearch from './search.reducer';
export var selectSearch = function (state) { return state.search; };
export var selectGet = function (state) { return state.get; };
export var selectSearchResults = createSelector(selectSearch, function (state) {
    if (!state || state.content == null) {
        return [];
    }
    return state.content.Content;
});
export var selectGetResult = createSelector(selectGet, function (state) {
    if (!state) {
        return null;
    }
    return state.content;
});
export var appReducer = {
    search: fromSearch.searchReducer,
    get: fromGet.getReducer
};
//# sourceMappingURL=index.js.map