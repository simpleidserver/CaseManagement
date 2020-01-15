import { SearchPerformancesResult } from '../models/search-performances-result.model';

export interface ListPerformancesState {
	isLoading: boolean;
	isErrorLoadOccured: boolean;
    content: SearchPerformancesResult;
}