import { CasePlanInstanceFileItem } from "./caseplaninstancefileitem.model";
import { CasePlanItemInstance } from "./caseplaniteminstance.model";
import { WorkerTask } from "./workertask.model";

export class CaseInstance {
    constructor() {
        this.files = [];
        this.children = [];
        this.workerTasks = [];
    }

    id: string;
    caseFileId: string;
    casePlanId: string;
    name: string;
    state: string;
    executionContext: string;
    files: CasePlanInstanceFileItem[];
    children: CasePlanItemInstance[];
    workerTasks: WorkerTask[];
    createDateTime: Date;
    updateDateTime: Date;
}