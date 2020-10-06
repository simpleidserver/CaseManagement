export class Rendering {
    constructor() {
        this.input = [];
        this.output = [];
    }

    input: InputRenderingElement[];
    output: OutputRenderingElement[];
}

export class RenderingElement {
    id: string;
    label: string;
}

export class InputRenderingElement extends RenderingElement {
    value: string;
}

export class OutputRenderingElement extends RenderingElement {
    xPath: string;
    default: string;
    value: OutputRenderingElementValue;
}

export class OutputRenderingElementValue {
    type: string;
    values: string[];
}