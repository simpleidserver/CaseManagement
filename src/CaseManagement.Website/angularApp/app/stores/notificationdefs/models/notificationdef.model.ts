import { Parameter } from "../../common/parameter.model";
import { PeopleAssignment } from "../../common/people-assignment.model";
import { PresentationElement } from "../../common/presentationelement.model";
import { PresentationParameter } from "../../common/presentationparameter.model";

export class NotificationDefinition {
    id: string;
    version: number;
    name: string;
    nbInstances: number;
    updateDateTime: Date;
    createDateTime: Date;
    priority: number;
    operationParameters: Parameter[];
    peopleAssignments: PeopleAssignment[];
    presentationElements: PresentationElement[];
    presentationParameters: PresentationParameter[];
}