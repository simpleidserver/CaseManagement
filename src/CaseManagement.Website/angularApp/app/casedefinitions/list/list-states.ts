import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';

export interface ListCaseDefinitionsState {
	isLoading: boolean;
	isErrorLoadOccured: boolean;
    content: SearchCaseDefinitionsResult;
}