import { CasePlan } from './caseplan.model';

export class SearchCasePlanResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: CasePlan[];
}