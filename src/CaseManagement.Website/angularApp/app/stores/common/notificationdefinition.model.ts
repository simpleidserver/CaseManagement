import { Parameter } from "./parameter.model";
import { PeopleAssignment } from "./people-assignment.model";
import { PresentationElement } from "./presentationelement.model";
import { PresentationParameter } from "./presentationparameter.model";

export class NotificationDefinition {
    constructor() {
        this.operationParameters = [];
        this.peopleAssignments = [];
        this.presentationElements = [];
        this.presentationParameters = [];
    }

    name: string;
    priority: string;
    rendering: string;
    operationParameters: Parameter[];
    peopleAssignments: PeopleAssignment[];
    presentationElements: PresentationElement[];
    presentationParameters: PresentationParameter[];
    static getBusinessAdministrators(notifDef: NotificationDefinition) {
        return notifDef.peopleAssignments.filter(function (p: PeopleAssignment) {
            return p.usage === 'BUSINESSADMINISTRATOR';
        });
    }
    static getRecipients(notifDef : NotificationDefinition) {
        return notifDef.peopleAssignments.filter(function (p: PeopleAssignment) {
            return p.usage === 'RECIPIENT';
        });
    }

    static getInputParameters(notifDef: NotificationDefinition) {
        return notifDef.operationParameters.filter(function (p: Parameter) {
            return p.usage === 'INPUT';
        });
    }

    static getOutputParameter(notifDef: NotificationDefinition) {
        return notifDef.operationParameters.filter(function (p: Parameter) {
            return p.usage === 'OUTPUT';
        });
    }

    static getNames(hd: NotificationDefinition) {
        return hd.presentationElements.filter(function (pe: PresentationElement) {
            return pe.usage === 'NAME';
        });
    }
    static getDescriptions(hd: NotificationDefinition) {
        return hd.presentationElements.filter(function (pe: PresentationElement) {
            return pe.usage === 'DESCRIPTION';
        });
    }
    static getSubjects(hd: NotificationDefinition) {
        return hd.presentationElements.filter(function (pe: PresentationElement) {
            return pe.usage === 'SUBJECT';
        });
    }
}
