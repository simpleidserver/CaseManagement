import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { Parameter } from '@app/stores/common/operation.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

export class ParameterType {
    constructor(public type: string, public displayName: string) { }
}

@Component({
    selector: 'view-humantaskdef-info-component',
    templateUrl: './info.component.html',
    styleUrls: ['./info.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDefInfoComponent implements OnInit {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.TASK";
    humanTaskDef: HumanTaskDef = new HumanTaskDef();
    infoForm: FormGroup;
    inputParameterForm: FormGroup;
    outputParameterForm: FormGroup;
    parameterTypes: ParameterType[];

    constructor(
        private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private actions$: ScannedActionsSubject) {
        this.infoForm = this.formBuilder.group({
            name: '',
            priority: ''
        });
        this.inputParameterForm = this.formBuilder.group({
            name: '',
            type: '',
            isRequired:''
        });
        this.outputParameterForm = this.formBuilder.group({
            name: '',
            type: '',
            isRequired: ''
        });
        this.parameterTypes = [];
        this.parameterTypes.push(new ParameterType("string", "string"));
        this.parameterTypes.push(new ParameterType("bool", "boolean"));
    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.TASK_INFO_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_HUMANTASK_INFO))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_TASK_INFO_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ADD_OPERATION_INPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ADD_OPERATION_OUTPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_ADD_OPERATION_INPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_ADD_OPERATION_OUTPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.DELETE_OPERATION_INPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.DELETE_OPERATION_OUTPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_OPERATION_INPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_DELETE_OPERATION_INPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_OPERATION_OUTPUT_PARAMETER))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_DELETE_OPERATION_OUTPUT_PARAMETER'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.infoForm.get('name').setValue(e.name);
            this.infoForm.get('priority').setValue(e.priority);
            this.humanTaskDef = e;
        });
    }

    updateInfo(form: any) {
        const request = new fromHumanTaskDefActions.UpdateHumanTaskInfo(this.humanTaskDef.id, form.name, form.priority);
        this.store.dispatch(request);
    }

    addInputParameter(param: Parameter) {
        const filteredInputParam = this.humanTaskDef.operation.inputParameters.filter(function (p: Parameter) {
            return p.name === param.name
        });
        if (filteredInputParam.length === 1) {
            this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.INPUT_PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }

        if (!param.isRequired) {
            param.isRequired = false;
        }

        this.inputParameterForm.reset();
        const request = new fromHumanTaskDefActions.AddInputParameterOperation(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    }

    addOutputParameter(param: Parameter) {
        const filteredOutputParam = this.humanTaskDef.operation.outputParameters.filter(function (p: Parameter) {
            return p.name === param.name
        });
        if (filteredOutputParam.length === 1) {
            this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.OUTPUT_PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }

        if (!param.isRequired) {
            param.isRequired = false;
        }

        this.outputParameterForm.reset();
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
