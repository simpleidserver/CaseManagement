import { PeopleAssignment } from "../../common/people-assignment.model";
var HumanTaskDefAssignment = (function () {
    function HumanTaskDefAssignment() {
        this.potentialOwner = new PeopleAssignment();
        this.excludedOwner = new PeopleAssignment();
        this.taskInitiator = new PeopleAssignment();
        this.taskStakeHolder = new PeopleAssignment();
        this.businessAdministrator = new PeopleAssignment();
        this.recipient = new PeopleAssignment();
    }
    return HumanTaskDefAssignment;
}());
export { HumanTaskDefAssignment };
//# sourceMappingURL=humantaskdef-assignment.model.js.map