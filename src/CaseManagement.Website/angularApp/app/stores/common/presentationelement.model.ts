import { TextDef } from "./textdef.model";
import { Description } from "./description.model";
import { PresentationParameter } from "./presentationparameter.model";

export class PresentationElement {
    constructor() {
        this.names = [];
        this.subjects = [];
        this.descriptions = [];
        this.presentationParameters = [];
    }

    names: TextDef[];
    subjects: TextDef[];
    descriptions: Description[];
    presentationParameters: PresentationParameter[];
}