import { CmmnPlanInstanceResult } from './cmmn-planinstance.model';

export class SearchCasePlanInstanceResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: CmmnPlanInstanceResult[];
}