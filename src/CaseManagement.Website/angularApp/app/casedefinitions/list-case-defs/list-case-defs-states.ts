import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';

export interface ListCaseDefsState {
	isLoading: boolean;
	isErrorLoadOccured: boolean;
    content: SearchCaseDefinitionsResult;
}