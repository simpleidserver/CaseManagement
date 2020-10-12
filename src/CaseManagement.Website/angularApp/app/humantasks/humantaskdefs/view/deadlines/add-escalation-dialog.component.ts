import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material';

@Component({
    selector: 'add-escalation-dialog',
    templateUrl: 'add-escalation-dialog.component.html',
})
export class AddEscalationDialog {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.DEADLINES";
    addEscalationForm: FormGroup;

    constructor(
        private dialogRef: MatDialogRef<AddEscalationDialog>,
        private formBuilder: FormBuilder) {
        this.addEscalationForm = this.formBuilder.group({
            condition: ''
        });
    }

    onAddEscalation(form: any) {
        this.dialogRef.close(form);
    }

    close() {
        this.dialogRef.close();
    }
}
