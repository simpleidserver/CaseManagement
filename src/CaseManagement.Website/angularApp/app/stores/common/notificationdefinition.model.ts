import { Operation } from "./operation.model";
import { PeopleAssignment } from "./people-assignment.model";
import { PresentationElement } from "./presentationelement.model";

export class NotificationDefinitionPeopleAssignment {
    constructor() {
        this.recipient = new PeopleAssignment();
        this.businessAdministrator = new PeopleAssignment();
    }

    recipient: PeopleAssignment;
    businessAdministrator: PeopleAssignment;
}

export class NotificationRendering {
    content: string;
}

export class NotificationDefinition {
    constructor() {
        this.operation = new Operation();
        this.peopleAssignment = new NotificationDefinitionPeopleAssignment();
        this.presentationElement = new PresentationElement();
        this.rendering = new NotificationRendering();
    }

    name: string;
    operation: Operation;
    priority: string;
    peopleAssignment: NotificationDefinitionPeopleAssignment;
    presentationElement: PresentationElement;
    rendering: NotificationRendering;
}
