export class CmmnPlanInstanceResult {
    id: string;
    caseFileId: string;
    casePlanId: string;
    name: string;
    state: string;
    files: CmmnPlanInstanceFileItemResult[];
    roles: CmmnPlanInstanceRoleResult[];
    children: CmmnPlanItemInstanceResult[];
    createDateTime: Date;
    updateDateTime: Date;
}

export class CmmnPlanInstanceFileItemResult {
    casePlanElementInstanceId: string;
    caseFileItemType: string;
    externalValue: string;
}

export class CmmnPlanInstanceRoleResult {
    id: string;
    name: string;
}

export class TransitionHistoryResult {
    transition: string;
    executionDateTime: Date;
    message: string;
}

export class CmmnPlanItemInstanceResult {
    id: string;
    eltId: string;
    nbOccurrence: number;
    name: string;
    type: string;
    state: string;
    transitionHistories: TransitionHistoryResult[];
}