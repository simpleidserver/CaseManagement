import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { CaseFilesService } from '../services/casefiles.service';

@Component({
    selector: 'add-case-file-dialog',
    templateUrl: 'add-case-file-dialog.html',
})
export class AddCaseFileDialog {
    addCaseFileForm: FormGroup;

    constructor(private dialogRef: MatDialogRef<AddCaseFileDialog>, private caseFilesService: CaseFilesService, private formBuilder: FormBuilder) {
        this.addCaseFileForm = this.formBuilder.group({
            name: '',
            description: ''
        });
    }

    onSubmit() {
        this.caseFilesService.add(this.addCaseFileForm.get('name').value, this.addCaseFileForm.get('description').value).subscribe(() => {
            this.dialogRef.close();
        });
    }
}
