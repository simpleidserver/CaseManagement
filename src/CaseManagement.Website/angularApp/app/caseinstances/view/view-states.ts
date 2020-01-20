import { CaseDefinition } from '../../casedefinitions/models/case-definition.model';
import { CaseFileItem } from '../../casedefinitions/models/case-file-item.model';
import { CaseFile } from '../../casedefinitions/models/case-file.model';
import { CaseInstance } from '../../casedefinitions/models/case-instance.model';

export interface ViewCaseInstanceState {
	isLoading: boolean;
    isErrorLoadOccured: boolean;
    caseInstance: CaseInstance;
    caseFile: CaseFile;
    caseDefinition: CaseDefinition;
    caseFileItems: CaseFileItem[]
}