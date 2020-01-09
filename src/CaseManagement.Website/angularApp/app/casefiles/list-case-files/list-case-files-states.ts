import { SearchCaseFilesResult } from '../models/search-case-files-result.model';

export interface ListCaseFilesState {
	isLoading: boolean;
	isErrorLoadOccured: boolean;
    content: SearchCaseFilesResult;
}