export class BpmnInstance {
    constructor() {
        this.elementInstances = [];
    }

    id: string;
    status: string;
    processFileId: string;
    createDateTime: Date;
    updateDateTime: Date;
    elementInstances: BpmnNodeInstance[];
}

export class BpmnNodeInstance {
    constructor() {
        this.activityStates = [];
    }

    id: string;
    flowNodeId: string;
    state: string;
    activityState: string;
    metadata: any;
    activityStates: ActivityStateHistory[];
}

export class ActivityStateHistory {
    state: string;
    executionDateTime: Date;
}