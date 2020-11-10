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
        this.values = [];
        this.labels = [];
    }

    id: string;
    xPath: string;
    valueType: string;
    default: string;
    values: OptionValue[];
    labels: Translation[];
}

export class OptionValue {
    constructor() {
        this.displayNames = [];
    }

    value: string;
    displayNames: Translation[];
}