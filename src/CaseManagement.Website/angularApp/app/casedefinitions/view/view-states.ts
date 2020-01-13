import { CaseDefinitionHistory } from '../models/case-definition-history.model';
import { CaseDefinition } from '../models/case-definition.model';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseFormInstancesResult } from '../models/search-case-form-instances-result.model';
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';
import { SearchCaseActivationsResult } from '../models/search-case-activations-result.model';

export interface ViewCaseDefinitionState {
	isLoading: boolean;
    isErrorLoadOccured: boolean;
    caseDefinition: CaseDefinition;
    caseFile: CaseFile;
    caseDefinitionHistory: CaseDefinitionHistory;
}

export interface ViewCaseInstancesState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCaseInstancesResult
}

export interface ViewFormInstancesState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCaseFormInstancesResult
}

export interface ViewCaseActivationsState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchCaseActivationsResult;
}