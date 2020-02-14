import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MatSnackBar } from '@angular/material';
import { CaseFilesService } from '../services/casefiles.service';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';

@Component({
    selector: 'add-case-file-dialog',
    templateUrl: 'add-case-file-dialog.html',
})
export class AddCaseFileDialog {
    addCaseFileForm: FormGroup;

    constructor(private route: Router, private dialogRef: MatDialogRef<AddCaseFileDialog>, private caseFilesService: CaseFilesService, private formBuilder: FormBuilder, private snackBar: MatSnackBar, private translateService: TranslateService) {
        this.addCaseFileForm = this.formBuilder.group({
            name: '',
            description: ''
        });
    }

    onSubmit() {
        this.caseFilesService.add(this.addCaseFileForm.get('name').value, this.addCaseFileForm.get('description').value).subscribe((caseFileId : string) => {
            this.snackBar.open(this.translateService.instant('CASES_FILE_ADDED'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
            this.dialogRef.close();
            this.route.navigate(["/cases/casefiles/"+ caseFileId]);
        }, () => {
            this.snackBar.open(this.translateService.instant('ERROR_ADD_CASE_FILE'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    }
}
