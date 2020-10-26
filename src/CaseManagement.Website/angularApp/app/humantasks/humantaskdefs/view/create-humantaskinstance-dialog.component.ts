import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { CreateHumanTaskInstance } from '@app/stores/humantaskinstances/parameters/create-humantaskinstance.model';

@Component({
    selector: 'create-humantask-instance-dialog',
    templateUrl: 'create-humantaskinstance-dialog.component.html',
})
export class CreateHumanTaskInstanceDialog {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.CREATE_HUMANTASKINSTANCE";
    createHumanTaskInstance: CreateHumanTaskInstance;

    constructor(private dialogRef: MatDialogRef<CreateHumanTaskInstanceDialog>,
        @Inject(MAT_DIALOG_DATA) public data: HumanTaskDef) {
        this.createHumanTaskInstance = new CreateHumanTaskInstance();
        this.createHumanTaskInstance.humanTaskName = this.data.name;
    }

    update() {
        this.dialogRef.close(this.createHumanTaskInstance);
    }

    close() {
        this.dialogRef.close();
    }
}
