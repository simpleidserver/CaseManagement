import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatTableDataSource, MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { Escalation } from '@app/stores/common/escalation.model';
import { ToPart } from '@app/stores/common/topart.model';
import { Deadline } from '@app/stores/humantaskdefs/models/deadline';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { AddToPartDialog } from './add-topart-dialog.component';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-escalation-component',
    templateUrl: './viewescalation.component.html',
    styleUrls: ['./viewescalation.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewEscalationComponent implements OnInit {
    toParts$: MatTableDataSource<ToPart> = new MatTableDataSource<ToPart>();
    toPartDisplayedColumns: string[] = ['name', 'expression', 'actions'];

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
        this.toParts$.data = [ new ToPart(), new ToPart() ];
        const deadlineId = this.route.parent.snapshot.params['deadlineid'];
        const escalationId = this.route.snapshot.params['escalationid'];
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            const deadline = e.deadLines.filter((d: Deadline) => {
                return d.id === deadlineId;
            })[0];
            const escalation = deadline.escalations.filter((e: Escalation) => {
                return e.id === escalationId;
            })[0];
            if (escalation.toParts.length === 0) {
                this.toParts$.data = [new ToPart(), new ToPart()];
            } else {
                this.toParts$.data = escalation.toParts;
            }
        });
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
