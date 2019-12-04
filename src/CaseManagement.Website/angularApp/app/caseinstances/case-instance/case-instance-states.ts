import { CaseDefinition } from "../../casedefinitions/models/case-def.model";
import { CaseInstance } from "../../casedefinitions/models/search-case-instances-result.model";
import { SearchCaseExecutionStepsResult } from "../../casedefinitions/models/search-case-execution-steps-result.model";

export class CaseInstanceState {
    caseDefinition: CaseDefinition;
    caseInstance: CaseInstance;
    executionStepsResult: SearchCaseExecutionStepsResult;
    isCaseInstanceLoading: boolean;
    isCaseInstanceErrorLoadOccured: boolean;
    isCaseExecutionStepsLoading: boolean;
    isCaseExecutionStepsErrorLoadOccured: boolean;

    constructor() {
        this.caseDefinition = null;
        this.caseInstance = null;
        this.executionStepsResult = null;
        this.isCaseInstanceLoading = true;
        this.isCaseInstanceErrorLoadOccured = false;
        this.isCaseExecutionStepsLoading = true;
        this.isCaseExecutionStepsErrorLoadOccured = false;
    }
}