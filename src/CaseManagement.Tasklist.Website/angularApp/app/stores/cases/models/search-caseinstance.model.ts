import { CaseInstance } from './caseinstance.model';

export class SearchCaseInstanceResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: CaseInstance[];
}