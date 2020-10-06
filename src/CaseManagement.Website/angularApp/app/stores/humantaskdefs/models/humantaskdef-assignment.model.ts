import { PeopleAssignment } from "../../common/people-assignment.model";

export class HumanTaskDefAssignment {
    potentialOwner: PeopleAssignment;
    excludedOwner: PeopleAssignment;
    taskInitiator: PeopleAssignment;
    taskStakeHolder: PeopleAssignment;
    businessAdministrator: PeopleAssignment;
    recipient: PeopleAssignment;
}