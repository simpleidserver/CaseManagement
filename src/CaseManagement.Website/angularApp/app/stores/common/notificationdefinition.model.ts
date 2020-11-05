import { Parameter } from "./parameter.model";
import { PeopleAssignment } from "./people-assignment.model";
import { PresentationElement } from "./presentationelement.model";

export class NotificationDefinition {
    constructor() {
        this.operationParameters = [];
        this.peopleAssignments = [];
        this.presentationElements = [];
    }

    name: string;
    priority: string;
    rendering: string;
    operationParameters: Parameter[];
    peopleAssignments: PeopleAssignment[];
    presentationElements: PresentationElement[];
}
