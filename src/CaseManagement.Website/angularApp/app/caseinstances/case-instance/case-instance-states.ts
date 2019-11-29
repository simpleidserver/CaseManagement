import { CaseDefinition } from "../../casedefinitions/models/case-def.model";
import { CaseInstance } from "../../casedefinitions/models/search-case-instances-result.model";
import { SearchCaseExecutionStepsResult } from "../../casedefinitions/models/search-case-execution-steps-result.model";

export interface CaseInstanceState {
    caseDefinition: CaseDefinition;
    caseInstance: CaseInstance;
    executionStepsResult: SearchCaseExecutionStepsResult;
    isCaseInstanceLoading: boolean;
    isCaseInstanceErrorLoadOccured: boolean;
    isCaseExecutionStepsLoading: boolean;
    isCaseExecutionStepsErrorLoadOccured: boolean;
}