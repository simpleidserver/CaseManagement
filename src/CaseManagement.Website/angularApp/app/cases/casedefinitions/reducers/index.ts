import { createSelector } from '@ngrx/store';
import { CaseFile } from '../../casefiles/models/case-file.model';
import * as fromGetFile from '../../casefiles/reducers/get.reducer';
import { CaseDefinitionHistory } from '../models/case-definition-history.model';
import { CaseDefinition } from '../models/case-definition.model';
import { SearchCaseActivationsResult } from '../models/search-case-activations-result.model';
import { SearchCaseFormInstancesResult } from '../models/search-case-form-instances-result.model';
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';
import * as fromGetHistory from './get-history.reducer';
import * as fromGet from './get.reducer';
import * as fromSearchCaseActivations from './search-case-activations.reducer';
import * as fromSearchFormInstances from './search-form-instances.reducer';
import * as fromSearchInstances from './search-instances.reducer';
import * as fromSearch from './search.reducer';

export interface CaseDefinitionsState {
    search: fromSearch.State;
    get: fromGet.State;
    searchInstances: fromSearchInstances.State,
    searchFormInstances: fromSearchFormInstances.State,
    searchCaseActivations: fromSearchCaseActivations.State,
    getHistory: fromGetHistory.State,
    getFile: fromGetFile.State
}

export const selectSearch = (state: CaseDefinitionsState) => state.search;
export const selectGet = (state: CaseDefinitionsState) => state.get;
export const selectSearchInstances = (state: CaseDefinitionsState) => state.searchInstances;
export const selectSearchFormInstances = (state: CaseDefinitionsState) => state.searchFormInstances;
export const selectSearchCaseActivations = (state: CaseDefinitionsState) => state.searchCaseActivations;
export const selectGetHistory = (state: CaseDefinitionsState) => state.getHistory
export const selectGetFile = (state: CaseDefinitionsState) => state.getFile

export const selectSearchResults = createSelector(
    selectSearch,
    (state: fromSearch.State) => {
        if (!state || state.content == null) {
            return [];
        }

        return state.content.Content;
    }
);

export const selectLengthResults = createSelector(
    selectSearch,
    (state: fromSearch.State) => {
        if (!state || state.content == null) {
            return 0;
        }

        return state.content.TotalLength;
    }
);

export const selectGetResult = createSelector(
    selectGet,
    (state: fromGet.State) => {
        if (!state || !state.content) {
            return new CaseDefinition();
        }

        return state.content;
    }
);

export const selectGetFileResult = createSelector(
    selectGetFile,
    (state: fromGetFile.State) => {
        if (!state || !state.content) {
            return new CaseFile();
        }

        return state.content;
    }
);

export const selectSearchInstancesResult = createSelector(
    selectSearchInstances,
    (state: fromSearchInstances.State) => {
        if (!state || !state.content) {
            return new SearchCaseInstancesResult();
        }

        return state.content;
    }
);

export const selectSearchFormInstancesResult = createSelector(
    selectSearchFormInstances,
    (state: fromSearchFormInstances.State) => {
        if (!state || !state.content) {
            return new SearchCaseFormInstancesResult();
        }

        return state.content;
    }
);

export const selectSearchCaseActivationsResult = createSelector(
    selectSearchCaseActivations,
    (state: fromSearchCaseActivations.State) => {
        if (!state || !state.content) {
            return new SearchCaseActivationsResult();
        }

        return state.content.Content;
    }
);

export const selectGetHistoryResult = createSelector(
    selectGetHistory,
    (state: fromGetHistory.State) => {
        if (!state || !state.content) {
            return new CaseDefinitionHistory();
        }

        return state.content;
    }
);

export const appReducer = {
    search: fromSearch.searchReducer,
    get: fromGet.getReducer,
    searchInstances: fromSearchInstances.searchReducer,
    searchFormInstances: fromSearchFormInstances.searchReducer,
    searchCaseActivations: fromSearchCaseActivations.searchReducer,
    getHistory: fromGetHistory.getReducer,
    getFile: fromGetFile.getReducer
};