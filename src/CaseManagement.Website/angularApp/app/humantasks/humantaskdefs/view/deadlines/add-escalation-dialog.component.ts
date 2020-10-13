import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
            condition: new FormControl('', [
                Validators.required
            ])
        });
    }

    onAddEscalation(form: any) {
        if (!this.addEscalationForm.valid) {
            return;
        }

        this.dialogRef.close(form);
    }

    close() {
        this.dialogRef.close();
    }
}
