import { SearchCaseInstancesResult } from '../models/search-case-instances.model';

export interface CaseInstancesState {
	isLoading: boolean;
	isErrorLoadOccured: boolean;
    caseInstances: SearchCaseInstancesResult;
}