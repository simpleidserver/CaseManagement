import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Escalation } from '@app/stores/common/escalation.model';
import { Parameter } from '@app/stores/common/operation.model';
import { PeopleAssignment } from '@app/stores/common/people-assignment.model';
import { ToPart } from '@app/stores/common/topart.model';

@Component({
    selector: 'edit-escalation-dialog',
    templateUrl: 'edit-escalation-dialog.component.html',
})
export class EditEscalationDialog {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.DEADLINES";
    updateEscalationForm: FormGroup;
    addToPartForm: FormGroup;
    updateNotificationForm: FormGroup;

    constructor(
        private dialogRef: MatDialogRef<EditEscalationDialog>,
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data: Escalation) {
        this.updateEscalationForm = this.formBuilder.group({
            condition: ''
        });
        this.addToPartForm = this.formBuilder.group({
            name: '',
            expression: ''
        });
        this.updateNotificationForm = this.formBuilder.group({
            name: '',
            priority: ''
        });
    }

    addToPart(form: any) {
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

    updateBusinessAdministrator(evt: PeopleAssignment) {
        this.data.notification.peopleAssignment.businessAdministrator = evt;
    }

    updateRecipient(evt: PeopleAssignment) {
        this.data.notification.peopleAssignment.recipient = evt;
    }

    addInputParameter(param: Parameter) {
        this.data.notification.operation.inputParameters.push(param);
    }

    addOutputParameter(param: Parameter) {
        this.data.notification.operation.outputParameters.push(param);
    }

    deleteInputParameter(param: Parameter) {
        const i = this.data.notification.operation.inputParameters.indexOf(param);
        this.data.notification.operation.outputParameters.splice(i, 1);
    }

    deleteOutputParameter(param: Parameter) {
        const i = this.data.notification.operation.outputParameters.indexOf(param);
        this.data.notification.operation.outputParameters.splice(i, 1);
    }

    updateEscalation() {
        this.dialogRef.close(this.data);
    }
}
