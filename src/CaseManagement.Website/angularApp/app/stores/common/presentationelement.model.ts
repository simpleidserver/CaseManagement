import { TextDef } from "./textdef.model";
import { Description } from "./description.model";
import { PresentationParameter } from "./presentationparameter.model";

export class PresentationElement {
    names: TextDef[];
    subjects: TextDef[];
    descriptions: Description[];
    presentationParameters: PresentationParameter[];
}