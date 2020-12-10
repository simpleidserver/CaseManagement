var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, EventEmitter, Output, ViewEncapsulation, Input } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
var ParameterType = (function () {
    function ParameterType(type, displayName) {
        this.type = type;
        this.displayName = displayName;
    }
    return ParameterType;
}());
export { ParameterType };
var OperationParameterComponent = (function () {
    function OperationParameterComponent(translateService, snackBar, formBuilder) {
        this.translateService = translateService;
        this.snackBar = snackBar;
        this.formBuilder = formBuilder;
        this.parameters = [];
        this.parameterAdded = new EventEmitter();
        this.parameterRemoved = new EventEmitter();
        this.parameterTypes = [];
        this.parameterTypes.push(new ParameterType("STRING", "string"));
        this.parameterTypes.push(new ParameterType("BOOL", "boolean"));
        this.parameterForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            type: new FormControl('', [
                Validators.required
            ]),
            isRequired: ''
        });
    }
    OperationParameterComponent.prototype.addParameter = function (param) {
        if (!this.parameterForm.valid) {
            return;
        }
        var filteredOutputParam = this.parameters.filter(function (p) {
            return p.name === param.name;
        });
        if (filteredOutputParam.length === 1) {
            this.snackBar.open(this.translateService.instant('SHARED.PARAMETER_EXISTS'), this.translateService.instant('undo'), {
                duration: 2000
            });
            return;
        }
        if (!param.isRequired) {
            param.isRequired = false;
        }
        param.usage = this.usage;
        this.parameters.push(param);
        this.parameterForm.reset();
        this.parameterAdded.emit(param);
    };
    OperationParameterComponent.prototype.deleteParameter = function (param) {
        var index = this.parameters.indexOf(param);
        this.parameters.splice(index, 1);
        this.parameterRemoved.emit(param);
    };
    __decorate([
        Input(),
        __metadata("design:type", String)
    ], OperationParameterComponent.prototype, "usage", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Array)
    ], OperationParameterComponent.prototype, "parameters", void 0);
    __decorate([
        Output(),
        __metadata("design:type", Object)
    ], OperationParameterComponent.prototype, "parameterAdded", void 0);
    __decorate([
        Output(),
        __metadata("design:type", Object)
    ], OperationParameterComponent.prototype, "parameterRemoved", void 0);
    OperationParameterComponent = __decorate([
        Component({
            selector: 'operationparameter-component',
            templateUrl: './operationparameter.component.html',
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [TranslateService,
            MatSnackBar,
            FormBuilder])
    ], OperationParameterComponent);
    return OperationParameterComponent;
}());
export { OperationParameterComponent };
//# sourceMappingURL=operationparameter.component.js.map