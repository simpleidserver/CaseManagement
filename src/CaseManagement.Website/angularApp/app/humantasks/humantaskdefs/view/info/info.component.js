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
import { FormBuilder } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
var ParameterType = (function () {
    function ParameterType(type, displayName) {
        this.type = type;
        this.displayName = displayName;
    }
    return ParameterType;
}());
export { ParameterType };
var ViewHumanTaskDefInfoComponent = (function () {
    function ViewHumanTaskDefInfoComponent(store, formBuilder, snackBar, translateService) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.baseTranslationKey = "HUMANTASK.DEF.VIEW.TASK";
        this.humanTaskDef = new HumanTaskDef();
        this.infoForm = this.formBuilder.group({
            name: '',
            priority: ''
        });
        this.inputParameterForm = this.formBuilder.group({
            name: '',
            type: '',
            isRequired: ''
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
    ViewHumanTaskDefInfoComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.infoForm.get('name').setValue(e.name);
            _this.infoForm.get('priority').setValue(e.name);
            _this.humanTaskDef = e;
        });
    };
    ViewHumanTaskDefInfoComponent.prototype.updateInfo = function (form) {
        this.humanTaskDef.name = form.name;
        this.humanTaskDef.priority = form.priority;
        this.refresh();
    };
    ViewHumanTaskDefInfoComponent.prototype.addInputParameter = function (param) {
        var filteredInputParam = this.humanTaskDef.operation.inputParameters.filter(function (p) {
            return p.name === param.name;
        });
        if (filteredInputParam.length === 1) {
            this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.INPUT_PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
        }
        this.humanTaskDef.operation.inputParameters.push(param);
    };
    ViewHumanTaskDefInfoComponent.prototype.addOutputParameter = function (param) {
        var filteredOutputParam = this.humanTaskDef.operation.outputParameters.filter(function (p) {
            return p.name === param.name;
        });
        if (filteredOutputParam.length === 1) {
            this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.OUTPUT_PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
        }
        this.humanTaskDef.operation.outputParameters.push(param);
    };
    ViewHumanTaskDefInfoComponent.prototype.deleteInputParameter = function (param) {
        var index = this.humanTaskDef.operation.inputParameters.indexOf(param);
        this.humanTaskDef.operation.inputParameters.splice(index, 1);
    };
    ViewHumanTaskDefInfoComponent.prototype.deleteOutputParameter = function (param) {
        var index = this.humanTaskDef.operation.outputParameters.indexOf(param);
        this.humanTaskDef.operation.outputParameters.splice(index, 1);
    };
    ViewHumanTaskDefInfoComponent.prototype.update = function () {
        var request = new fromHumanTaskDefActions.UpdateHumanTaskDef(this.humanTaskDef);
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
            TranslateService])
    ], ViewHumanTaskDefInfoComponent);
    return ViewHumanTaskDefInfoComponent;
}());
export { ViewHumanTaskDefInfoComponent };
//# sourceMappingURL=info.component.js.map