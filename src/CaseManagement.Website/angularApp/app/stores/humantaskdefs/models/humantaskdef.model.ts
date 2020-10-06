import { Operation } from "../../common/operation.model";
import { PresentationElement } from "../../common/presentationelement.model";
import { HumanTaskDefAssignment } from "./humantaskdef-assignment.model";
import { HumanTaskDefinitionDeadLines } from "./humantaskdef-deadlines";
import { Rendering } from "../../common/rendering.model";

export class HumanTaskDef {
    constructor() {
        this.rendering = new Rendering();
    }

    name: string;
    actualOwnerRequired: boolean;
    operation: Operation;
    priority: number;
    peopleAssignment: HumanTaskDefAssignment;
    presentationElement: PresentationElement;
    outcome: string;
    searchBy: string;
    rendering: Rendering;
    deadLines: HumanTaskDefinitionDeadLines;
}