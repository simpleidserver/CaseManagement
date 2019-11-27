import { CaseDefinition } from '../models/case-def.model';
import { SearchCaseInstancesResult } from '../models/search-case-instances-result.model';

export interface CaseDefState {
	isCaseDefinitionLoading: boolean;
    isCaseDefinitionErrorLoadOccured: boolean;
    caseDefinitionContent: CaseDefinition,
    isCaseInstancesLoading: boolean;
    isCaseInstancesErrorLoadOccured: boolean;
    caseInstancesContent: SearchCaseInstancesResult
}