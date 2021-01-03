import { CasePlanInstanceFileItem } from "./caseplaninstancefileitem.model";
import { CasePlanItemInstance } from "./caseplaniteminstance.model";

export class CaseInstance {
    constructor() {
        this.files = [];
        this.children = [];
    }

    id: string;
    caseFileId: string;
    casePlanId: string;
    name: string;
    state: string;
    executionContext: string;
    files: CasePlanInstanceFileItem[];
    children: CasePlanItemInstance[];
    createDateTime: Date;
    updateDateTime: Date;
}