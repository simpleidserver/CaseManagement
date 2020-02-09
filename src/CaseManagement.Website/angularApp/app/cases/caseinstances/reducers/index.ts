import { createSelector } from '@ngrx/store';
import { CaseDefinition } from '../../casedefinitions/models/case-definition.model';
import { CaseFileItem } from '../../casedefinitions/models/case-file-item.model';
import { CaseInstance } from '../../casedefinitions/models/case-instance.model';
import * as fromGetCaseDefinition from '../../casedefinitions/reducers/get.reducer';
import { CaseFile } from '../../casefiles/models/case-file.model';
import * as fromGetCaseFile from '../../casefiles/reducers/get.reducer';
import * as fromGetCaseFileItems from './get-case-file-items.reducer';
import * as fromGetInstance from './get.reducer';

export interface CaseInstancesState {
    casefile: fromGetCaseFile.State,
    caseinstance: fromGetInstance.State,
    casedefinition: fromGetCaseDefinition.State,
    casefileitems: fromGetCaseFileItems.State
}

export const selectCaseFile = (state: CaseInstancesState) => state.casefile;
export const selectCaseInstance = (state: CaseInstancesState) => state.caseinstance;
export const selectCaseDefinition = (state: CaseInstancesState) => state.casedefinition;
export const selectCaseFileItems = (state: CaseInstancesState) => state.casefileitems;

export const selectCaseFileResult = createSelector(
    selectCaseFile,
    (state: fromGetCaseFile.State) => {
        if (!state || state.content == null) {
            return new CaseFile();
        }

        return state.content;
    }
);

export const selectCaseInstanceResult = createSelector(
    selectCaseInstance,
    (state: fromGetInstance.State) => {
        if (!state) {
            return new CaseInstance();
        }

        return state.content;
    }
);

export const selectCaseDefinitionResult = createSelector(
    selectCaseDefinition,
    (state: fromGetCaseDefinition.State) => {
        if (!state) {
            return new CaseDefinition();
        }

        return state.content;
    }
);

export const selectCaseFileItemsResult = createSelector(
    selectCaseFileItems,
    (state: fromGetCaseFileItems.State) => {
        if (!state) {
            return new CaseFileItem();
        }

        return state.content;
    }
);

export const appReducer = {
    casefile: fromGetCaseFile.getReducer,
    caseinstance: fromGetInstance.getReducer,
    casedefinition: fromGetCaseDefinition.getReducer,
    casefileitems: fromGetCaseFileItems.getReducer
};