import { Component } from '@angular/core';
import { NominateParameter } from '@app/stores/tasks/parameters/nominate-parameter';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';

@Component({
    selector: 'nominate-task-dialog',
    templateUrl: 'nominate-task-dialog.component.html',
})
export class NominateTaskDialogComponent {
    nominateParameterForm: FormGroup;
    baseTranslationKey: string = "NOMINATE";
    values: string[] = [];
    nominateParameter: NominateParameter;

    constructor(
        private dialogRef: MatDialogRef<NominateTaskDialogComponent>,
        private formBuilder: FormBuilder) {
        this.nominateParameterForm = this.formBuilder.group({
            type: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ]),
        });
        this.nominateParameter = new NominateParameter();
    }

    deleteValue(val: string) {
        const index = this.values.indexOf(val);
        this.values.splice(index, 1);
    }

    addPeopleAssignment(form: any) {
        if (!this.nominateParameterForm.valid) {
            return;
        }

        this.nominateParameterForm.get('value').setValue('');
        this.values.push(form.value);
    }

    update() {
        const type = this.nominateParameterForm.get('type').value;
        switch (type) {
            case 'GROUPNAMES':
                this.nominateParameter.groupNames = this.values;
                this.nominateParameter.userIdentifiers = [];
                break;
            case 'USERIDENTIFIERS':
                this.nominateParameter.userIdentifiers = this.values;
                this.nominateParameter.groupNames = [];
                break;
        }

        this.dialogRef.close(this.nominateParameter);
    }
}