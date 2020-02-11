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
export var selectSearch = function (state) { return state.search; };
export var selectGet = function (state) { return state.get; };
export var selectSearchInstances = function (state) { return state.searchInstances; };
export var selectSearchFormInstances = function (state) { return state.searchFormInstances; };
export var selectSearchCaseActivations = function (state) { return state.searchCaseActivations; };
export var selectGetHistory = function (state) { return state.getHistory; };
export var selectGetFile = function (state) { return state.getFile; };
export var selectSearchResults = createSelector(selectSearch, function (state) {
    if (!state || state.content == null) {
        return [];
    }
    return state.content.Content;
});
export var selectLengthResults = createSelector(selectSearch, function (state) {
    if (!state || state.content == null) {
        return 0;
    }
    return state.content.TotalLength;
});
export var selectGetResult = createSelector(selectGet, function (state) {
    if (!state || !state.content) {
        return new CaseDefinition();
    }
    return state.content;
});
export var selectGetFileResult = createSelector(selectGetFile, function (state) {
    if (!state || !state.content) {
        return new CaseFile();
    }
    return state.content;
});
export var selectSearchInstancesResult = createSelector(selectSearchInstances, function (state) {
    if (!state || !state.content) {
        return new SearchCaseInstancesResult();
    }
    return state.content;
});
export var selectSearchFormInstancesResult = createSelector(selectSearchFormInstances, function (state) {
    if (!state || !state.content) {
        return new SearchCaseFormInstancesResult();
    }
    return state.content;
});
export var selectSearchCaseActivationsResult = createSelector(selectSearchCaseActivations, function (state) {
    if (!state || !state.content) {
        return new SearchCaseActivationsResult();
    }
    return state.content.Content;
});
export var selectGetHistoryResult = createSelector(selectGetHistory, function (state) {
    if (!state || !state.content) {
        return new CaseDefinitionHistory();
    }
    return state.content;
});
export var appReducer = {
    search: fromSearch.searchReducer,
    get: fromGet.getReducer,
    searchInstances: fromSearchInstances.searchReducer,
    searchFormInstances: fromSearchFormInstances.searchReducer,
    searchCaseActivations: fromSearchCaseActivations.searchReducer,
    getHistory: fromGetHistory.getReducer,
    getFile: fromGetFile.getReducer
};
//# sourceMappingURL=index.js.map