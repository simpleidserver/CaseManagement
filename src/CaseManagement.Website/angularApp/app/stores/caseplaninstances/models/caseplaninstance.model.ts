export class CasePlanInstanceResult {
    id: string;
    casePlanId: string;
    name: string;
    state: string;
    files: CasePlanInstanceFileItemResult[];
    roles: CasePlanInstanceRoleResult[];
    children: CasePlanItemInstanceResult[];
    createDateTime: Date;
    updateDateTime: Date;
}

export class CasePlanInstanceFileItemResult {
    casePlanElementInstanceId: string;
    caseFileItemType: string;
    externalValue: string;
}

export class CasePlanInstanceRoleResult {
    id: string;
    name: string;
}

export class TransitionHistoryResult {
    transition: string;
    executionDateTime: Date;
}

export class CasePlanItemInstanceResult {
    id: string;
    name: string;
    type: string;
    state: string;
    transitionHistories: TransitionHistoryResult[];
}