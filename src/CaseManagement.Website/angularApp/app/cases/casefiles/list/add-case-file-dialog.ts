import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material';

@Component({
    selector: 'add-case-file-dialog',
    templateUrl: 'add-case-file-dialog.html',
})
export class AddCaseFileDialog {
    addCaseFileForm: FormGroup;

    constructor(private dialogRef: MatDialogRef<AddCaseFileDialog>,
        private formBuilder: FormBuilder) {
        this.addCaseFileForm = this.formBuilder.group({
            name: '',
            description: ''
        });
    }

    onSubmit() {
        this.dialogRef.close(this.addCaseFileForm.value);
    }
}
