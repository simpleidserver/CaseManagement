import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';

@Component({
    selector: 'add-humantaskdef-dialog',
    templateUrl: 'add-humantaskdef-dialog.component.html',
})
export class AddHumanTaskDefDialog {
    addHumanTaskForm: FormGroup;

    constructor(
        private dialogRef: MatDialogRef<AddHumanTaskDefDialog>,
        private formBuilder: FormBuilder) {
        this.addHumanTaskForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ])
        });
    }

    onAddHumanTask(form: any) {
        if (!this.addHumanTaskForm.valid) {
            return;
        }

        this.dialogRef.close(form);
    }

    close() {
        this.dialogRef.close();
    }
}
