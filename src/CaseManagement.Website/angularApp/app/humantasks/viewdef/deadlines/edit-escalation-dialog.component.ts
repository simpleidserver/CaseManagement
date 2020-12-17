import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Escalation } from '@app/stores/common/escalation.model';
import { Parameter } from '@app/stores/common/parameter.model';
import { PeopleAssignment } from '@app/stores/common/people-assignment.model';
import { ToPart } from '@app/stores/common/topart.model';
import { NotificationDefinition } from '@app/stores/common/notificationdefinition.model';
import { PresentationElement } from '@app/stores/common/presentationelement.model';
import { PresentationParameter } from '@app/stores/common/presentationparameter.model';

@Component({
    selector: 'edit-escalation-dialog',
    templateUrl: 'edit-escalation-dialog.component.html',
})
export class EditEscalationDialog {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.DEADLINES";
    updateEscalationForm: FormGroup;
    addToPartForm: FormGroup;
    updateNotificationForm: FormGroup;
    businessAdministrators: PeopleAssignment[] = [];
    recipients: PeopleAssignment[] = [];
    inputParameters: Parameter[] = [];
    outputParameters: Parameter[] = [];
    names: PresentationElement[] = [];
    subjects: PresentationElement[] = [];
    descriptions: PresentationElement[] = [];
    presentationParameters: PresentationParameter[] = [];

    constructor(
        private dialogRef: MatDialogRef<EditEscalationDialog>,
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: Escalation) {
        this.updateEscalationForm = this.formBuilder.group({
            condition: new FormControl('', [
                Validators.required
            ])
        });
        this.addToPartForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            expression: new FormControl('', [
                Validators.required
            ])
        });
        this.updateNotificationForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            priority: ''
        });
        this.updateEscalationForm.get('condition').setValue(this.data.condition);
        this.updateNotificationForm.get('name').setValue(this.data.notification.name);
        this.updateNotificationForm.get('priority').setValue(this.data.notification.priority);
        this.businessAdministrators = NotificationDefinition.getBusinessAdministrators(data.notification);
        this.recipients = NotificationDefinition.getRecipients(data.notification);
        this.inputParameters = NotificationDefinition.getInputParameters(data.notification);
        this.outputParameters = NotificationDefinition.getOutputParameter(data.notification);
        this.names = NotificationDefinition.getNames(data.notification);
        this.subjects = NotificationDefinition.getSubjects(data.notification);
        this.descriptions = NotificationDefinition.getDescriptions(data.notification);
        this.presentationParameters = data.notification.presentationParameters;
    }

    addToPart(form: any) {
        if (!this.addToPartForm.valid) {
            return;
        }

        const toPart = new ToPart();
        toPart.name = form.name;
        toPart.expression = form.expression;
        this.addToPartForm.reset();
        this.data.toParts.push(toPart);
    }

    deleteToPart(toPart: ToPart) {
        const index = this.data.toParts.indexOf(toPart);
        this.data.toParts.splice(index, 1);
    }

    addInputParameter(param: Parameter) {
        param.usage = 'INPUT';
        this.data.notification.operationParameters.push(param);
    }

    addOutputParameter(param: Parameter) {
        param.usage = 'OUTPUT';
        this.data.notification.operationParameters.push(param);
    }

    deleteInputParameter(param: Parameter) {
        const i = this.data.notification.operationParameters.indexOf(param);
        this.data.notification.operationParameters.splice(i, 1);
    }

    deleteOutputParameter(param: Parameter) {
        const i = this.data.notification.operationParameters.indexOf(param);
        this.data.notification.operationParameters.splice(i, 1);
    }

    updateEscalation() {
        if (!this.updateEscalationForm.valid || !this.updateNotificationForm.valid) {
            return;
        }

        let peopleAssignments: PeopleAssignment[] = [];
        peopleAssignments = peopleAssignments.concat(
            this.businessAdministrators,
            this.recipients);
        let presentationElements: PresentationElement[] = [];
        presentationElements = presentationElements.concat(
            this.names,
            this.subjects,
            this.descriptions
        );
        let parameters: Parameter[] = [];
        parameters = parameters.concat(
            this.inputParameters,
            this.outputParameters);
        this.data.condition = this.updateEscalationForm.get('condition').value;
        this.data.notification.name = this.updateNotificationForm.get('name').value;
        this.data.notification.priority = this.updateNotificationForm.get('priority').value;
        this.data.notification.peopleAssignments = peopleAssignments;
        this.data.notification.presentationParameters = this.presentationParameters;
        this.data.notification.operationParameters = parameters;
        this.data.notification.presentationElements = presentationElements;
        this.dialogRef.close(this.data);
    }
}
