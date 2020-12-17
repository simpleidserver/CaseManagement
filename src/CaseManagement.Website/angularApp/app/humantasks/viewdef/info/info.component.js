var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ViewHumanTaskDefInfoComponent = (function () {
    function ViewHumanTaskDefInfoComponent(store, formBuilder, snackBar, translateService, actions$) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.actions$ = actions$;
        this.baseTranslationKey = "HUMANTASK.DEF.VIEW.TASK";
        this.humanTaskDef = new HumanTaskDef();
        this.inputOperationParameters = [];
        this.outputOperationParameters = [];
        this.infoForm = this.formBuilder.group({
            id: new FormControl({ value: '', disabled: true }),
            name: new FormControl('', [
                Validators.required
            ]),
            priority: ''
        });
    }
    ViewHumanTaskDefInfoComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.TASK_INFO_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_HUMANTASK_INFO; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_TASK_INFO_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ADD_OPERATION_INPUT_PARAMETER'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ADD_OPERATION_OUTPUT_PARAMETER'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_OPERATION_INPUT_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_ADD_OPERATION_INPUT_PARAMETER'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_OPERATION_OUTPUT_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_ADD_OPERATION_OUTPUT_PARAMETER'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.DELETE_OPERATION_INPUT_PARAMETER'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.DELETE_OPERATION_OUTPUT_PARAMETER'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_OPERATION_INPUT_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_DELETE_OPERATION_INPUT_PARAMETER'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_DELETE_OPERATION_OUTPUT_PARAMETER; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.ERROR_DELETE_OPERATION_OUTPUT_PARAMETER'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.infoForm.get('id').setValue(e.id);
            _this.infoForm.get('name').setValue(e.name);
            _this.infoForm.get('priority').setValue(e.priority);
            _this.humanTaskDef = e;
            _this.inputOperationParameters = HumanTaskDef.getInputOperationParameters(_this.humanTaskDef);
            _this.outputOperationParameters = HumanTaskDef.getOutputOperationParameters(_this.humanTaskDef);
        });
    };
    ViewHumanTaskDefInfoComponent.prototype.updateInfo = function (form) {
        if (!this.infoForm.valid) {
            return;
        }
        var request = new fromHumanTaskDefActions.UpdateHumanTaskInfo(this.humanTaskDef.id, form.name, form.priority);
        this.store.dispatch(request);
    };
    ViewHumanTaskDefInfoComponent.prototype.addInputParameter = function (param) {
        var request = new fromHumanTaskDefActions.AddInputParameterOperation(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    };
    ViewHumanTaskDefInfoComponent.prototype.addOutputParameter = function (param) {
        var request = new fromHumanTaskDefActions.AddOutputParameterOperation(this.humanTaskDef.id, param);
        this.store.dispatch(request);
    };
    ViewHumanTaskDefInfoComponent.prototype.deleteInputParameter = function (param) {
        var request = new fromHumanTaskDefActions.DeleteInputParameterOperation(this.humanTaskDef.id, param.name);
        this.store.dispatch(request);
    };
    ViewHumanTaskDefInfoComponent.prototype.deleteOutputParameter = function (param) {
        var request = new fromHumanTaskDefActions.DeleteOutputParameterOperation(this.humanTaskDef.id, param.name);
        this.store.dispatch(request);
    };
    ViewHumanTaskDefInfoComponent = __decorate([
        Component({
            selector: 'view-humantaskdef-info-component',
            templateUrl: './info.component.html',
            styleUrls: ['./info.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store,
            FormBuilder,
            MatSnackBar,
            TranslateService,
            ScannedActionsSubject])
    ], ViewHumanTaskDefInfoComponent);
    return ViewHumanTaskDefInfoComponent;
}());
export { ViewHumanTaskDefInfoComponent };
//# sourceMappingURL=info.component.js.map