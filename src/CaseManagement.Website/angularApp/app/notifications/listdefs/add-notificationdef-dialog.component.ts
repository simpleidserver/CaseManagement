import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';

@Component({
    selector: 'add-notificationdef-dialog',
    templateUrl: 'add-notificationdef-dialog.component.html',
})
export class AddNotificationDefDialog {
    addNotificationForm: FormGroup;

    constructor(
        private dialogRef: MatDialogRef<AddNotificationDefDialog>,
        private formBuilder: FormBuilder) {
        this.addNotificationForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ])
        });
    }

    onAddNotification(form: any) {
        if (!this.addNotificationForm.valid) {
            return;
        }

        this.dialogRef.close(form);
    }

    close() {
        this.dialogRef.close();
    }
}
