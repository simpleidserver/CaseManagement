import { CaseInstance } from '../../casedefinitions/models/case-instance.model';
import { CaseFile } from '../../casedefinitions/models/case-file.model';
import { CaseDefinition } from '../../casedefinitions/models/case-definition.model';

export interface ViewCaseInstanceState {
	isLoading: boolean;
    isErrorLoadOccured: boolean;
    caseInstance: CaseInstance;
    caseFile: CaseFile;
    caseDefinition: CaseDefinition;
}