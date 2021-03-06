﻿import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { Escalation } from '@app/stores/common/escalation.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import * as fromNotificationDefActions from '@app/stores/notificationdefs/actions/notificationdef.actions';
import { Deadline } from '@app/stores/humantaskdefs/models/deadline';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { AddEscalationDialog } from './add-escalation-dialog.component';

@Component({
    selector: 'view-deadline-component',
    templateUrl: './viewdeadline.component.html',
    styleUrls: ['./viewdeadline.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewDeadlineComponent implements OnInit {
    escalationDisplayedColumns: string[] = ['condition', 'nbToParts', 'nbNotifications'];
    humanTaskDef: HumanTaskDef = new HumanTaskDef();
    deadline: Deadline = new Deadline();
    infoForm: FormGroup = new FormGroup({
        name: new FormControl('', [
            Validators.required
        ]),
        for: new FormControl(''),
        until: new FormControl('')
    });
    escalations$: Escalation[] = [];

    constructor(
        private store: Store<fromAppState.AppState>,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private router: Router,
        private translateService: TranslateService,
        private route: ActivatedRoute,
        private dialog: MatDialog) {

    }

    ngOnInit(): void {
        const deadlineId = this.route.snapshot.params['deadlineid'];
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_GET_HUMANTASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.UNKNOWN_HUMANTASK'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_ESCALATION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ADD_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_ESCALATION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.DELETE_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ESCALATION_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                const id = this.route.snapshot.params['id'];
                const deadlineId = this.route.snapshot.params['deadlineid'];
                this.router.navigate(['/humantasks/' + id + '/deadline/' + deadlineId]);
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_DELETE_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.DEADLINE_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_UPDATE_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTaskDef = e;
            this.deadline = this.humanTaskDef.deadLines.filter((d: Deadline) => {
                return d.id === deadlineId;
            })[0];
            this.infoForm.get('name').setValue(this.deadline.name);
            this.infoForm.get('for').setValue(this.deadline.for);
            this.infoForm.get('until').setValue(this.deadline.until);
            this.escalations$ = this.deadline.escalations;
        });
        this.refresh();
    }

    updateInfo(deadline: Deadline) {
        const id = this.route.snapshot.params['id'];
        const deadlineId = this.route.snapshot.params['deadlineid'];
        deadline.id = deadlineId;
        const request = new fromHumanTaskDefActions.UpdateDeadlineOperation(id, deadline);
        this.store.dispatch(request);
    }

    addDeadline(evt: any) {
        evt.preventDefault();
        const dialogRef = this.dialog.open(AddEscalationDialog, {
            width: '800px'
        })
        dialogRef.afterClosed().subscribe((esc: Escalation) => {
            if (!esc) {
                return;
            }

            const request = new fromHumanTaskDefActions.AddEscalationDeadlineOperation(this.humanTaskDef.id, this.deadline.id, esc.condition, esc.notificationId);
            this.store.dispatch(request);
        });
    }

    removeEscalation(escalation: Escalation) {
        const id = this.route.snapshot.params['id'];
        const deadlineId = this.route.snapshot.params['deadlineid'];
        const request = new fromHumanTaskDefActions.DeleteEscalationOperation(id, deadlineId, escalation.id);
        this.store.dispatch(request);
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromHumanTaskDefActions.GetHumanTaskDef(id);
        this.store.dispatch(request);
        const getNotificationDefs = new fromNotificationDefActions.GetAll();
        this.store.dispatch(getNotificationDefs);
    }
}
