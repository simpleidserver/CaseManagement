export class BpmnInstance {
    constructor() {
        this.executionPaths = [];
    }

    id: string;
    status: string;
    processFileId: string;
    createDateTime: Date;
    updateDateTime: Date;
    executionPaths: BpmnExecutionPath[];
}

export class BpmnExecutionPath {
    constructor() {
        this.executionPointers = [];
    }

    id: string;
    createDateTime: Date;
    executionPointers: BpmnExecutionPointer[];
}

export class BpmnExecutionPointer {
    constructor() {
        this.incomingTokens = [];
        this.outgoingTokens = [];
    }

    id: string;
    isActive: boolean;
    flowNodeId: string;
    incomingTokens: BpmnMessageToken[];
    outgoingTokens: BpmnMessageToken[];
    flowNodeInstance: BpmnNodeInstance;
}

export class BpmnMessageToken {
    name: string;
    content: any;
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
    message: string;
}