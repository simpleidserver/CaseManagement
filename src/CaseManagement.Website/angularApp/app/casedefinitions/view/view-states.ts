import { CaseDefinitionHistory } from '../models/case-definition-history.model';
import { CaseDefinition } from '../models/case-definition.model';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';

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