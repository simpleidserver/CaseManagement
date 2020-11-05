import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Escalation } from '@app/stores/common/escalation.model';
import { Parameter } from '@app/stores/common/parameter.model';
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
    get businessAdministrator() {
        return this.data.notification.peopleAssignments.filter(function (p: PeopleAssignment) {
            return p.usage === 'BUSINESSADMINISTRATOR';
        })[0];
    }
    get recipient() {
        return this.data.notification.peopleAssignments.filter(function (p: PeopleAssignment) {
            return p.usage === 'RECIPIENT';
        })[0];
    }

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

    updateBusinessAdministrator(evt: PeopleAssignment) {
        const record = this.businessAdministrator;
        this.updatePeopleAssignment(record, evt, 'BUSINESSADMINISTRATOR');
    }

    updateRecipient(evt: PeopleAssignment) {
        const record = this.recipient;
        this.updatePeopleAssignment(record, evt, 'RECIPIENT');
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

        this.data.condition = this.updateEscalationForm.get('condition').value;
        this.data.notification.name = this.updateNotificationForm.get('name').value;
        this.data.notification.priority = this.updateNotificationForm.get('priority').value;
        this.dialogRef.close(this.data);
    }

    private updatePeopleAssignment(pa: PeopleAssignment, newPa: PeopleAssignment, usage: string) {
        if (pa) {
            const index = this.data.notification.peopleAssignments.indexOf(pa);
            this.data.notification.peopleAssignments.splice(index, 1);
        }

        newPa.usage = usage;
        this.data.notification.peopleAssignments.push(newPa);
    }
}
