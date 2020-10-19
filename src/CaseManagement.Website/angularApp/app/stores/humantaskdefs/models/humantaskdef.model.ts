import { Operation } from "../../common/operation.model";
import { PresentationElement } from "../../common/presentationelement.model";
import { Rendering } from "../../common/rendering.model";
import { HumanTaskDefAssignment } from "./humantaskdef-assignment.model";
import { HumanTaskDefinitionDeadLines } from "./humantaskdef-deadlines";

export class HumanTaskDef {
    constructor() {
        this.rendering = new Rendering();
        this.operation = new Operation();
        this.deadLines = new HumanTaskDefinitionDeadLines();
        this.peopleAssignment = new HumanTaskDefAssignment();
        this.presentationElementResult = new PresentationElement();
    }

    id: string;
    version: number;
    name: string;
    actualOwnerRequired: boolean;
    operation: Operation;
    priority: number;
    peopleAssignment: HumanTaskDefAssignment;
    presentationElementResult: PresentationElement;
    outcome: string;
    searchBy: string;
    rendering: Rendering;
    deadLines: HumanTaskDefinitionDeadLines;
}