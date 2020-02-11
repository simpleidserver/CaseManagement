import { createSelector } from '@ngrx/store';
import { CaseDefinition } from '../../casedefinitions/models/case-definition.model';
import { CaseFileItem } from '../../casedefinitions/models/case-file-item.model';
import { CaseInstance } from '../../casedefinitions/models/case-instance.model';
import * as fromGetCaseDefinition from '../../casedefinitions/reducers/get.reducer';
import { CaseFile } from '../../casefiles/models/case-file.model';
import * as fromGetCaseFile from '../../casefiles/reducers/get.reducer';
import * as fromGetCaseFileItems from './get-case-file-items.reducer';
import * as fromGetInstance from './get.reducer';
export var selectCaseFile = function (state) { return state.casefile; };
export var selectCaseInstance = function (state) { return state.caseinstance; };
export var selectCaseDefinition = function (state) { return state.casedefinition; };
export var selectCaseFileItems = function (state) { return state.casefileitems; };
export var selectCaseFileResult = createSelector(selectCaseFile, function (state) {
    if (!state || state.content == null) {
        return new CaseFile();
    }
    return state.content;
});
export var selectCaseInstanceResult = createSelector(selectCaseInstance, function (state) {
    if (!state) {
        return new CaseInstance();
    }
    return state.content;
});
export var selectCaseDefinitionResult = createSelector(selectCaseDefinition, function (state) {
    if (!state) {
        return new CaseDefinition();
    }
    return state.content;
});
export var selectCaseFileItemsResult = createSelector(selectCaseFileItems, function (state) {
    if (!state) {
        return new CaseFileItem();
    }
    return state.content;
});
export var appReducer = {
    casefile: fromGetCaseFile.getReducer,
    caseinstance: fromGetInstance.getReducer,
    casedefinition: fromGetCaseDefinition.getReducer,
    casefileitems: fromGetCaseFileItems.getReducer
};
//# sourceMappingURL=index.js.map