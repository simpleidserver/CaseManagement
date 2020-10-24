import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatSnackBar, MatTableDataSource } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { Escalation } from '@app/stores/common/escalation.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDefinitionDeadLine } from '@app/stores/humantaskdefs/models/humantaskdef-deadlines';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { AddEscalationDialog } from './add-escalation-dialog.component';
import { EditEscalationDialog } from './edit-escalation-dialog.component';

export class ParameterType {
    constructor(public type: string, public displayName: string) { }
}

@Component({
    selector: 'view-humantaskdef-deadlines-component',
    templateUrl: './deadlines.component.html',
    styleUrls: ['./deadlines.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDefDeadlinesComponent implements OnInit {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.DEADLINES";
    humanTaskDef: HumanTaskDef = new HumanTaskDef();
    displayedDeadLineColumns: string[] = ['name', 'for', 'until', 'actions'];
    displayedEscalationColumns: string[] = ['condition', 'actions'];
    addDeadlineForm: FormGroup;
    updateDeadlineForm: FormGroup;
    currentDeadlineType: string;
    currentDeadline: HumanTaskDefinitionDeadLine;
    startDeadlines: MatTableDataSource<HumanTaskDefinitionDeadLine> = new MatTableDataSource<HumanTaskDefinitionDeadLine>();
    completionDeadlines: MatTableDataSource<HumanTaskDefinitionDeadLine> = new MatTableDataSource<HumanTaskDefinitionDeadLine>();
    escalations: MatTableDataSource<Escalation> = new MatTableDataSource<Escalation>();

    constructor(
        private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private dialog: MatDialog,
        private translateService: TranslateService) {
        this.addDeadlineForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            deadlineType: new FormControl('', [
                Validators.required
            ]),
            validityType: new FormControl('', [
                Validators.required
            ]),
            validity: new FormControl('', [
                Validators.required
            ])
        });
        this.updateDeadlineForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            validityType: new FormControl('', [
                Validators.required
            ]),
            validity: new FormControl('', [
                Validators.required
            ])
        });
    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_START_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.START_DEADLINE_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.COMPLETION_DEADLINE_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_START_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_ADD_START_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_ADD_COMPLETION_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_START_DEALINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.START_DEADLINE_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_START_DEALINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_REMOVE_START_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.COMPLETION_DEADLINE_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_START_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.UPDATE_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_START_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_UPDATE_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.UPDATE_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_UPDATE_DEADLINE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_ESCALATION_STARTDEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ADD_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_ESCALATION_STARTDEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_ADD_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ADD_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_ESCALATION_COMPLETIONDEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_ADD_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_START_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.UPDATE_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_START_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_UPDATE_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_COMPLETION_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.UPDATE_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_COMPLETION_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_UPDATE_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_START_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ESCALATION_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_COMPLETION_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ESCALATION_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ESCALATION_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        // COMPLETE_DELETE_COMPLETION_ESCALATION
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_START_ESCALATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_DELETE_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ESCALATION_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_DELETE_ESCALATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTaskDef = e;
            this.startDeadlines.data = this.humanTaskDef.deadLines.startDeadLines;
            this.completionDeadlines.data = this.humanTaskDef.deadLines.completionDeadLines;
            if (this.currentDeadline) {
                const id = this.currentDeadline.id;
                if (this.currentDeadlineType === 'start') {
                    this.currentDeadline = this.humanTaskDef.deadLines.startDeadLines.filter(function (v: HumanTaskDefinitionDeadLine) {
                        return v.id === id;
                    })[0];
                } else {
                    this.currentDeadline = this.humanTaskDef.deadLines.completionDeadLines.filter(function (v: HumanTaskDefinitionDeadLine) {
                        return v.id === id;
                    })[0];
                }

                this.escalations.data = this.currentDeadline.escalations;
            }
        });
    }

    addDeadLine(form: any) {
        if (!this.addDeadlineForm.valid) {
            return;
        }

        const deadline = new HumanTaskDefinitionDeadLine();
        deadline.name = form.name;
        if (form.validityType === 'duration') {
            deadline.for = form.validity;
        } else {
            deadline.until = form.validity;
        }

        if (form.deadlineType === 'start') {
            const request = new fromHumanTaskDefActions.AddStartDeadLine(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        } else {
            const request = new fromHumanTaskDefActions.AddCompletionDeadLine(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        }

        this.addDeadlineForm.reset();
    }

    updateDeadline(form: any) {
        if (!this.updateDeadlineForm.valid || !this.currentDeadline) {
            return;
        }

        const deadline = new HumanTaskDefinitionDeadLine();
        deadline.id = this.currentDeadline.id;
        deadline.name = form.name;
        if (form.validityType === 'duration') {
            deadline.for = form.validity;
        } else {
            deadline.until = form.validity;
        }

        if (this.currentDeadlineType === 'start') {
            const request = new fromHumanTaskDefActions.UpdateStartDeadlineOperation(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        } else {
            const request = new fromHumanTaskDefActions.UpdateCompletionDeadlineOperation(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        }
    }

    removeStartDeadline(deadline: HumanTaskDefinitionDeadLine) {
        this.currentDeadline = null;
        const request = new fromHumanTaskDefActions.DeleteStartDeadlineOperation(this.humanTaskDef.id, deadline.id);
        this.store.dispatch(request);
    }

    removeCompletionDeadline(deadline: HumanTaskDefinitionDeadLine) {
        this.currentDeadline = null;
        const request = new fromHumanTaskDefActions.DeleteCompletionDeadlineOperation(this.humanTaskDef.id, deadline.id);
        this.store.dispatch(request);
    }

    edit(deadlineType: string, deadline: HumanTaskDefinitionDeadLine) {
        this.currentDeadlineType = deadlineType;
        this.currentDeadline = deadline;
        this.escalations.data = this.currentDeadline.escalations;
        this.updateDeadlineForm.get('name').setValue(deadline.name);
        if (deadline.for) {
            this.updateDeadlineForm.get('validityType').setValue('duration');
            this.updateDeadlineForm.get('validity').setValue(deadline.for);
        } else {
            this.updateDeadlineForm.get('validityType').setValue('expiration');
            this.updateDeadlineForm.get('validity').setValue(deadline.until);
        }
    }

    addEscalation() {
        const dialogRef = this.dialog.open(AddEscalationDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((e: any) => {
            if (!e) {
                return;
            }

            if (this.currentDeadlineType === 'start') {
                const request = new fromHumanTaskDefActions.AddEscalationStartDeadlineOperation(this.humanTaskDef.id, this.currentDeadline.id, e.condition);
                this.store.dispatch(request);
            } else {
                const request = new fromHumanTaskDefActions.AddEscalationCompletionDeadlineOperation(this.humanTaskDef.id, this.currentDeadline.id, e.condition);
                this.store.dispatch(request);
            }
        });
    }

    editEscalation(escalation: Escalation) {
        const dialogRef = this.dialog.open(EditEscalationDialog, {
            width: '800px',
            data: escalation
        });
        dialogRef.afterClosed().subscribe((e: any) => {
            if (!e) {
                return;
            }

            if (this.currentDeadlineType === 'start') {
                const request = new fromHumanTaskDefActions.UpdateStartEscalationOperation(this.humanTaskDef.id, this.currentDeadline.id, e);
                this.store.dispatch(request);
            } else {
                const request = new fromHumanTaskDefActions.UpdateCompletionEscalationOperation(this.humanTaskDef.id, this.currentDeadline.id, e);
                this.store.dispatch(request);
            }
        });
    }

    deleteEscalation(escalation: Escalation) {
        if (this.currentDeadlineType === 'start') {
            const request = new fromHumanTaskDefActions.DeleteStartEscalationOperation(this.humanTaskDef.id, this.currentDeadline.id, escalation);
            this.store.dispatch(request);
        } else {
            const request = new fromHumanTaskDefActions.DeleteCompletionEscalationOperation(this.humanTaskDef.id, this.currentDeadline.id, escalation);
            this.store.dispatch(request);
        }
    }
}
