import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar, MatTableDataSource } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDefinitionDeadLine } from '@app/stores/humantaskdefs/models/humantaskdef-deadlines';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

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
    addDeadlineForm: FormGroup;
    updateDeadlineForm: FormGroup;
    currentDeadlineType: string;
    currentDeadline: HumanTaskDefinitionDeadLine;
    startDeadlines: MatTableDataSource<HumanTaskDefinitionDeadLine> = new MatTableDataSource<HumanTaskDefinitionDeadLine>();
    completionDeadlines: MatTableDataSource<HumanTaskDefinitionDeadLine> = new MatTableDataSource<HumanTaskDefinitionDeadLine>();

    constructor(
        private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService) {
        this.addDeadlineForm = this.formBuilder.group({
            name: '',
            deadlineType: '',
            validityType: '',
            validity: ''
        });
        this.updateDeadlineForm = this.formBuilder.group({
            name: '',
            deadlineType: '',
            validityType: '',
            validity: ''
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
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_REMOVE_COMPLETION_DEADLINE'), this.translateService.instant('undo'), {
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
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTaskDef = e;
            this.startDeadlines.data = this.humanTaskDef.deadLines.startDeadLines;
            this.completionDeadlines.data = this.humanTaskDef.deadLines.completionDeadLines;
        });
    }

    addDeadLine(form: any) {
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
        const deadline = new HumanTaskDefinitionDeadLine();
        deadline.name = form.name;
        if (form.validityType === 'duration') {
            deadline.for = form.validity;
        } else {
            deadline.until = form.validity;
        }

        if (form.deadlineType === 'start') {
            const request = new fromHumanTaskDefActions.UpdateStartDeadlineOperation(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        } else {
            const request = new fromHumanTaskDefActions.UpdateCompletionDeadlineOperation(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        }
    }

    removeStartDeadline(deadline: HumanTaskDefinitionDeadLine) {
        this.currentDeadline = null;
        const request = new fromHumanTaskDefActions.DeleteStartDeadlineOperation(this.humanTaskDef.id, deadline.name);
        this.store.dispatch(request);
    }

    removeCompletionDeadline(deadline: HumanTaskDefinitionDeadLine) {
        this.currentDeadline = null;
        const request = new fromHumanTaskDefActions.DeleteCompletionDeadlineOperation(this.humanTaskDef.id, deadline.name);
        this.store.dispatch(request);
    }

    edit(deadlineType: string, deadline: HumanTaskDefinitionDeadLine) {
        this.currentDeadlineType = deadlineType;
        this.currentDeadline = deadline;
        this.updateDeadlineForm.get('name').setValue(deadline.name);
        this.updateDeadlineForm.get('deadlineType').setValue(deadlineType);
        if (deadline.for) {
            this.updateDeadlineForm.get('validityType').setValue('duration');
            this.updateDeadlineForm.get('validity').setValue(deadline.for);
        } else {
            this.updateDeadlineForm.get('expiration').setValue('duration');
            this.updateDeadlineForm.get('validity').setValue(deadline.until);
        }
    }
}
