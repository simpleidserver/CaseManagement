import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { Parameter } from '@app/stores/common/parameter.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
@Component({
    selector: 'view-humantaskdef-info-component',
    templateUrl: './info.component.html',
    styleUrls: ['./info.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDefInfoComponent implements OnInit, OnDestroy {
    humanTaskListener: any;
    humanTaskDef: HumanTaskDef = new HumanTaskDef();
    inputOperationParameters: Parameter[] = [];
    outputOperationParameters: Parameter[] = [];
    infoForm: FormGroup;

    constructor(
        private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private actions$: ScannedActionsSubject) {
        this.infoForm = this.formBuilder.group({
            id: new FormControl({value: '', disabled: true}),
            name: new FormControl('', [
                Validators.required
            ]),
            priority: ''
        });
    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.TASK_INFO_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_HUMANTASK_INFO))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_UPDATE_TASK_INFO'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.INPUT_PARAMETER_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.OUTPUT_PARAMETER_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_INPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_OUTPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.INPUT_PARAMETER_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.OUTPUT_PARAMETER_REMOVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_DELETE_INPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_DELETE_OUTPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.humanTaskListener = this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.infoForm.get('id').setValue(e.id);
            this.infoForm.get('name').setValue(e.name);
            this.infoForm.get('priority').setValue(e.priority);
            this.humanTaskDef = e;
            this.inputOperationParameters = HumanTaskDef.getInputOperationParameters(this.humanTaskDef);
            this.outputOperationParameters = HumanTaskDef.getOutputOperationParameters(this.humanTaskDef);
        });
    }

    ngOnDestroy(): void {
        this.humanTaskListener.unsubscribe();
    }

    updateInfo(form: any) {
        if (!this.infoForm.valid) {
            return;
        }

        const request = new fromHumanTaskDefActions.UpdateHumanTaskInfo(this.humanTaskDef.id, form.name, form.priority);
        this.store.dispatch(request);
    }

    addInputParameter(param: Parameter) {
        const request = new fromHumanTaskDefActions.AddInputParameterOperation(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    }

    addOutputParameter(param: Parameter) {
        const request = new fromHumanTaskDefActions.AddOutputParameterOperation(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    }

    deleteInputParameter(param: Parameter) {
        const request = new fromHumanTaskDefActions.DeleteInputParameterOperation(this.humanTaskDef.id, param.name);
        this.store.dispatch(request);
    }

    deleteOutputParameter(param: Parameter) {
        const request = new fromHumanTaskDefActions.DeleteOutputParameterOperation(this.humanTaskDef.id, param.name);
        this.store.dispatch(request);
    }
}
