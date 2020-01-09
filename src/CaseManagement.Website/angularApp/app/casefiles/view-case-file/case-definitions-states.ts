import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';

export interface CaseDefinitionsState {
	isLoading: boolean;
	isErrorLoadOccured: boolean;
    caseDefinitions: SearchCaseDefinitionsResult;
}