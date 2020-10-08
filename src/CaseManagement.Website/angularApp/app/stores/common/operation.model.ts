export class Operation {
    constructor() {
        this.inputParameters = [];
        this.outputParameters = [];
    }

    inputParameters: Parameter[];
    outputParameters: Parameter[];
}

export class Parameter {
    name: string;
    type: string;
    isRequired: boolean;
}