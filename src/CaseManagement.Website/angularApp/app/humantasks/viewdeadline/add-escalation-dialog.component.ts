import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { Store, select } from '@ngrx/store';
import * as fromAppState from '@app/stores/appstate';
import { NotificationDefinition } from '@app/stores/notificationdefs/models/notificationdef.model';

@Component({
    selector: 'add-escalation-dialog',
    templateUrl: 'add-escalation-dialog.component.html',
})
export class AddEscalationDialog implements OnInit {
    addEscalationForm: FormGroup;
    notificationDefs: NotificationDefinition[] = [];

    constructor(
        private dialogRef: MatDialogRef<AddEscalationDialog>,
        private formBuilder: FormBuilder,
        private store: Store<fromAppState.AppState>) {
        this.addEscalationForm = this.formBuilder.group({
            condition: new FormControl('', [
                Validators.required
            ]),
            notificationId: new FormControl('')
        });
    }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectNotificationLstResult)).subscribe((notifs: NotificationDefinition[]) => {
            if (!notifs) {
                return;
            }

            this.notificationDefs = notifs;
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
