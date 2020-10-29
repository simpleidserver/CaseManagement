export class Rendering {
    constructor() {
        this.input = [];
        this.output = [];
    }

    input: InputRenderingElement[];
    output: OutputRenderingElement[];
}

export class Translation {
    language: string;
    value: string;
}

export class RenderingElement {
    constructor() {
        this.labels = [];
    }

    id: string;   
    labels: Translation[];
}

export class InputRenderingElement extends RenderingElement {
    value: string;
}

export class OutputRenderingElement extends RenderingElement {
    xPath: string;
    value: OutputRenderingElementValue;
    default: string;
}

export class OutputRenderingElementValue {
    constructor() {
        this.values = [];
    }

    type: string;
    values: OptionValue[];
}

export class OptionValue {
    constructor() {
        this.displayNames = [];
    }

    value: string;
    displayNames: Translation[];
}