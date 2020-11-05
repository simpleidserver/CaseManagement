import { Parameter } from "../../common/parameter.model";
import { PeopleAssignment } from "../../common/people-assignment.model";
import { PresentationElement } from "../../common/presentationelement.model";
import { RenderingElement } from "../../common/rendering.model";
import { Deadline } from "./deadline";

export class HumanTaskDef {
    constructor() {
        this.operationParameters = [];
        this.peopleAssignments = [];
        this.presentationElements = [];
        this.deadLines = [];
    }

    id: string;
    version: number;
    name: string;
    updateDateTime: Date;
    createDateTime: Date;
    actualOwnerRequired: boolean;
    priority: number;
    outcome: string;
    searchBy: string;
    operationParameters: Parameter[];
    get inputOperationParameters() {
        return this.operationParameters.filter(function (v: Parameter) {
            return v.usage == 'INPUT';
        });
    }
    get outputOperationParameters() {
        return this.operationParameters.filter(function (v: Parameter) {
            return v.usage == 'OUTPUT';
        });
    }
    peopleAssignments: PeopleAssignment[];
    presentationElements: PresentationElement[];
    renderingElements: RenderingElement[];
    deadLines: Deadline[];
}