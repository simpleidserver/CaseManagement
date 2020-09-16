import { CasePlanInstanceResult } from './caseplaninstance.model';

export class SearchCasePlanInstanceResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: CasePlanInstanceResult[];
}