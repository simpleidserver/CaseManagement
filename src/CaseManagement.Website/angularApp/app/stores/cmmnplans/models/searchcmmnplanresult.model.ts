import { CmmnPlan } from './cmmn-plan.model';

export class SearchCmmnPlanResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: CmmnPlan[];
}