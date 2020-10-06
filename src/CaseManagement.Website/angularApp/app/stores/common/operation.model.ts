export class Operation {
    inputParameters: Parameter[];
    outputParameters: Parameter[];
}

export class Parameter {
    name: string;
    type: string;
    isRequired: boolean;
}