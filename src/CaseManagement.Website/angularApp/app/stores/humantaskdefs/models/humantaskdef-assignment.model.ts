import { PeopleAssignment } from "../../common/people-assignment.model";

export class HumanTaskDefAssignment {
    constructor() {
        this.potentialOwner = new PeopleAssignment();
        this.excludedOwner = new PeopleAssignment();
        this.taskInitiator = new PeopleAssignment();
        this.taskStakeHolder = new PeopleAssignment();
        this.businessAdministrator = new PeopleAssignment();
        this.recipient = new PeopleAssignment();
    }

    potentialOwner: PeopleAssignment;
    excludedOwner: PeopleAssignment;
    taskInitiator: PeopleAssignment;
    taskStakeHolder: PeopleAssignment;
    businessAdministrator: PeopleAssignment;
    recipient: PeopleAssignment;
}