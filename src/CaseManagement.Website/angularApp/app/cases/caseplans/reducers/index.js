import { createSelector } from '@ngrx/store';
import { CasePlan } from '../models/caseplan.model';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';
import { SearchFormInstanceResult } from '../models/searchforminstanceresult.model';
import { SearchWorkerTaskResult } from '../models/searchworkertaskresult.model';
import * as fromGet from './get.reducer';
import * as fromSearch from './search.reducer';
import * as fromCasePlanInstance from './searchcaseplaninstance.reducer';
import * as fromFormInstance from './searchforminstance.reducer';
import * as fromWorkerTask from './searchworkertask.reducer';
import * as fromSearchHistory from './searchhistory.reducer';
export var selectSearch = function (state) { return state.search; };
export var selectGet = function (state) { return state.get; };
export var selectSearchInstance = function (state) { return state.searchInstance; };
export var selectSearchFormInstance = function (state) { return state.searchFormInstance; };
export var selectSearchWorkerTask = function (state) { return state.searchWorkerTask; };
export var selectSearchHistory = function (state) { return state.searchHistory; };
export var selectSearchResult = createSelector(selectSearch, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectGetResult = createSelector(selectGet, function (state) {
    if (!state || !state.content) {
        return new CasePlan();
    }
    return state.content;
});
export var selectSearchInstanceResult = createSelector(selectSearchInstance, function (state) {
    if (!state || !state.content) {
        return new SearchCasePlanInstanceResult();
    }
    return state.content;
});
export var selectSearchFormInstancesResult = createSelector(selectSearchFormInstance, function (state) {
    if (!state || !state.content) {
        return new SearchFormInstanceResult();
    }
    return state.content;
});
export var selectSearchCaseWorkerResult = createSelector(selectSearchWorkerTask, function (state) {
    if (!state || !state.content) {
        return new SearchWorkerTaskResult();
    }
    return state.content;
});
export var selectSearchHistoryResult = createSelector(selectSearchHistory, function (state) {
    if (!state || !state.content) {
        return new SearchCasePlanInstanceResult();
    }
    return state.content;
});
export var appReducer = {
    search: fromSearch.searchReducer,
    get: fromGet.getReducer,
    searchInstance: fromCasePlanInstance.searchReducer,
    searchFormInstance: fromFormInstance.searchReducer,
    searchWorkerTask: fromWorkerTask.searchReducer,
    searchHistory: fromSearchHistory.searchReducer
};
//# sourceMappingURL=index.js.map