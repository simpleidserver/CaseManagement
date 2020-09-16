import { CaseFile } from './case-file.model';

export class SearchCaseFilesResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: CaseFile[];
}