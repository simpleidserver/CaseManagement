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

export interface CasePlanState {
    search: fromSearch.State;
    get: fromGet.State;
    searchInstance: fromCasePlanInstance.State,
    searchFormInstance: fromFormInstance.State,
    searchWorkerTask: fromWorkerTask.State,
    searchHistory: fromSearchHistory.State
}

export const selectSearch = (state: CasePlanState) => state.search;
export const selectGet = (state: CasePlanState) => state.get;
export const selectSearchInstance = (state: CasePlanState) => state.searchInstance;
export const selectSearchFormInstance = (state: CasePlanState) => state.searchFormInstance;
export const selectSearchWorkerTask = (state: CasePlanState) => state.searchWorkerTask;
export const selectSearchHistory = (state: CasePlanState) => state.searchHistory;

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
        if (!state || !state.content) {
            return new CasePlan();
        }

        return state.content;
    }
);

export const selectSearchInstanceResult = createSelector(
    selectSearchInstance,
    (state: fromCasePlanInstance.State) => {
        if (!state || !state.content) {
            return new SearchCasePlanInstanceResult();
        }

        return state.content;
    }
);

export const selectSearchFormInstancesResult = createSelector(
    selectSearchFormInstance,
    (state: fromFormInstance.State) => {
        if (!state || !state.content) {
            return new SearchFormInstanceResult();
        }

        return state.content;
    }
);

export const selectSearchCaseWorkerResult = createSelector(
    selectSearchWorkerTask,
    (state: fromWorkerTask.State) => {
        if (!state || !state.content) {
            return new SearchWorkerTaskResult();
        }

        return state.content;
    }
);

export const selectSearchHistoryResult = createSelector(
    selectSearchHistory,
    (state: fromSearchHistory.State) => {
        if (!state || !state.content) {
            return new SearchCasePlanInstanceResult();
        }

        return state.content;
    }
);

export const appReducer = {
    search: fromSearch.searchReducer,
    get: fromGet.getReducer,
    searchInstance: fromCasePlanInstance.searchReducer,
    searchFormInstance: fromFormInstance.searchReducer,
    searchWorkerTask: fromWorkerTask.searchReducer,
    searchHistory: fromSearchHistory.searchReducer
};