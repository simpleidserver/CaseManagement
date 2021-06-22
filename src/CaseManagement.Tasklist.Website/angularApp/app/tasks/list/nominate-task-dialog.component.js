var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { NominateParameter } from '@app/stores/tasks/parameters/nominate-parameter';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
var NominateTaskDialogComponent = (function () {
    function NominateTaskDialogComponent(dialogRef, formBuilder) {
        this.dialogRef = dialogRef;
        this.formBuilder = formBuilder;
        this.values = [];
        this.nominateParameterForm = this.formBuilder.group({
            type: new FormControl('', [
                Validators.required
            ]),
            value: new FormControl('', [
                Validators.required
            ]),
        });
        this.nominateParameter = new NominateParameter();
    }
    NominateTaskDialogComponent.prototype.deleteValue = function (val) {
        var index = this.values.indexOf(val);
        this.values.splice(index, 1);
    };
    NominateTaskDialogComponent.prototype.addPeopleAssignment = function (form) {
        if (!this.nominateParameterForm.valid) {
            return;
        }
        this.nominateParameterForm.get('value').setValue('');
        this.values.push(form.value);
    };
    NominateTaskDialogComponent.prototype.update = function () {
        var type = this.nominateParameterForm.get('type').value;
        switch (type) {
            case 'GROUPNAMES':
                this.nominateParameter.groupNames = this.values;
                this.nominateParameter.userIdentifiers = [];
                break;
            case 'USERIDENTIFIERS':
                this.nominateParameter.userIdentifiers = this.values;
                this.nominateParameter.groupNames = [];
                break;
        }
        this.dialogRef.close(this.nominateParameter);
    };
    NominateTaskDialogComponent = __decorate([
        Component({
            selector: 'nominate-task-dialog',
            templateUrl: 'nominate-task-dialog.component.html',
        }),
        __metadata("design:paramtypes", [MatDialogRef,
            FormBuilder])
    ], NominateTaskDialogComponent);
    return NominateTaskDialogComponent;
}());
export { NominateTaskDialogComponent };
//# sourceMappingURL=nominate-task-dialog.component.js.map