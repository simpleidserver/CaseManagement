import { Operation } from "./operation.model";
import { PeopleAssignment } from "./people-assignment.model";
import { PresentationElement } from "./presentationelement.model";
var NotificationDefinitionPeopleAssignment = (function () {
    function NotificationDefinitionPeopleAssignment() {
        this.recipient = new PeopleAssignment();
        this.businessAdministrator = new PeopleAssignment();
    }
    return NotificationDefinitionPeopleAssignment;
}());
export { NotificationDefinitionPeopleAssignment };
var NotificationRendering = (function () {
    function NotificationRendering() {
    }
    return NotificationRendering;
}());
export { NotificationRendering };
var NotificationDefinition = (function () {
    function NotificationDefinition() {
        this.operation = new Operation();
        this.peopleAssignment = new NotificationDefinitionPeopleAssignment();
        this.presentationElement = new PresentationElement();
        this.rendering = new NotificationRendering();
    }
    return NotificationDefinition;
}());
export { NotificationDefinition };
//# sourceMappingURL=notificationdefinition.model.js.map