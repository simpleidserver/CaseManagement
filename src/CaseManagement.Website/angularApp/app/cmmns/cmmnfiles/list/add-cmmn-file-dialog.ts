import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material';

@Component({
    selector: 'add-cmmn-file-dialog',
    templateUrl: 'add-cmmn-file-dialog.html',
})
export class AddCmmnFileDialog {
    addCmmnFileForm: FormGroup;

    constructor(private dialogRef: MatDialogRef<AddCmmnFileDialog>,
        private formBuilder: FormBuilder) {
        this.addCmmnFileForm = this.formBuilder.group({
            name: '',
            description: ''
        });
    }

    onSubmit() {
        this.dialogRef.close(this.addCmmnFileForm.value);
    }
}
