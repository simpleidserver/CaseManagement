import { CaseFile } from '../models/case-file.model';

export interface CaseFileState {
	isLoading: boolean;
	isErrorLoadOccured: boolean;
    caseFile: CaseFile;
}