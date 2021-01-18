import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';

@Component({
    selector: 'add-topart-dialog',
    templateUrl: 'add-topart-dialog.component.html',
})
export class AddToPartDialog {
    addParameterForm: FormGroup;

    constructor(
        private dialogRef: MatDialogRef<AddToPartDialog>,
        private formBuilder: FormBuilder) {
        this.addParameterForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            expression: new FormControl('', [
                Validators.required
            ])
        });
    }

    onAddParameter(form: any) {
        if (!this.addParameterForm.valid) {
            return;
        }

        this.dialogRef.close(form);
    }

    close() {
        this.dialogRef.close();
    }
}
