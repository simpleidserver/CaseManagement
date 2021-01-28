import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { MatDialog, MatSnackBar, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { Escalation } from '@app/stores/common/escalation.model';
import { ToPart } from '@app/stores/common/topart.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { Deadline } from '@app/stores/humantaskdefs/models/deadline';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { AddToPartDialog } from './add-topart-dialog.component';
import { NotificationDefinition } from '@app/stores/notificationdefs/models/notificationdef.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'view-escalation-component',
    templateUrl: './viewescalation.component.html',
    styleUrls: ['./viewescalation.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewEscalationComponent implements OnInit, OnDestroy {
    humanTask: HumanTaskDef;
    subscription: any;
    toParts$: MatTableDataSource<ToPart> = new MatTableDataSource<ToPart>();
    toPartDisplayedColumns: string[] = ['name', 'expression', 'actions'];
    notificationDefs: NotificationDefinition[] = [];
    escalationInfoForm: FormGroup = new FormGroup({
        condition: new FormControl('', [
            Validators.required    
        ]),
        notificationId: new FormControl('')
    });

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private dialog: MatDialog,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private snackBar: MatSnackBar) { }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_ESCALATION_TOPART))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ESCALATION_TO_PART_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_ESCALATION_TOPART))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_ESCALATION_TO_PART'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_ESCALATION_TOPART))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ESCALATION_TO_PART_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_ESCALATION_TOPART))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_REMOVE_ESCALATION_TOPART'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.UPDATE_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ESCALATION_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_UPDATE_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.toParts$.data = [ new ToPart(), new ToPart() ];
        this.store.pipe(select(fromAppState.selectNotificationLstResult)).subscribe((notifs: NotificationDefinition[]) => {
            if (!notifs) {
                return;
            }

            this.notificationDefs = notifs;
        });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTask = e;
            this.refresh();
        });
        this.subscription = this.route.params.subscribe(() => {
            this.refresh();
        });
    }

    ngOnDestroy(): void {
        if (!this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    refresh() {
        if (!this.humanTask) {
            return;
        }

        const deadlineId = this.route.parent.snapshot.params['deadlineid'];
        const escalationId = this.route.snapshot.params['escalationid'];
        const filteredDeadline = this.humanTask.deadLines.filter((d: Deadline) => {
            return d.id === deadlineId;
        });
        if (filteredDeadline.length !== 1) {
            return;
        }

        const filteredEscalation = filteredDeadline[0].escalations.filter((e: Escalation) => {
            return e.id === escalationId;
        });
        if (filteredEscalation.length !== 1) {
            return;
        }

        const escalation = filteredEscalation[0];
        this.escalationInfoForm.get('condition').setValue(escalation.condition);
        this.escalationInfoForm.get('notificationId').setValue(escalation.notificationId);
        if (escalation.toParts.length === 0) {
            this.toParts$.data = [new ToPart(), new ToPart()];
        } else {
            this.toParts$.data = escalation.toParts;
        }
    }

    updateEscalationInfo(form: any) {
        const id = this.route.parent.snapshot.params['id'];
        const deadlineId = this.route.parent.snapshot.params['deadlineid'];
        const escalationId = this.route.snapshot.params['escalationid'];
        const request = new fromHumanTaskDefActions.UpdateEscalationOperation(id, deadlineId, escalationId, form.condition, form.notificationId);
        this.store.dispatch(request);
    }

    addToPart(evt: any) {
        evt.preventDefault();
        const id = this.route.parent.snapshot.params['id'];
        const deadlineId = this.route.parent.snapshot.params['deadlineid'];
        const escalationId = this.route.snapshot.params['escalationid'];
        const dialogRef = this.dialog.open(AddToPartDialog, {
            width: '800px'
        })
        dialogRef.afterClosed().subscribe((esc: ToPart) => {
            if (!esc) {
                return;
            }

            const request = new fromHumanTaskDefActions.AddToPartEscalation(id, deadlineId, escalationId, esc);
            this.store.dispatch(request);
        });
    }

    deleteToPart(toPart: ToPart) {
        const id = this.route.parent.snapshot.params['id'];
        const deadlineId = this.route.parent.snapshot.params['deadlineid'];
        const escalationId = this.route.snapshot.params['escalationid'];
        const request = new fromHumanTaskDefActions.DeleteToPartEscalation(id, deadlineId, escalationId, toPart.name);
        this.store.dispatch(request);
    }
}
