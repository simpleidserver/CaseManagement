export class Rendering {
    constructor() {
        this.input = [];
        this.output = [];
    }

    input: InputRenderingElement[];
    output: OutputRenderingElement[];
}

export class Translation {
    constructor(lng: string, val: string) {
        this.language = lng;
        this.value = val;
    }

    language: string;
    value: string;
}

export class RenderingElement {
    constructor() {
        this.label = [];
    }

    id: string;
    label: Translation[];
}

export class InputRenderingElement extends RenderingElement {
    value: string;
}

export class OutputRenderingElementValue {
    constructor() {
        this.values = [];
    }

    type: string;
    values: OptionValue[];
}

export class OutputRenderingElement extends RenderingElement {
    constructor() {
        super();
        this.value = new OutputRenderingElementValue();
    }

    xPath: string;
    default: string;
    value: OutputRenderingElementValue;
}

export class OptionValue {
    constructor() {
        this.displayNames = [];
    }

    value: string;
    displayNames: Translation[];
}