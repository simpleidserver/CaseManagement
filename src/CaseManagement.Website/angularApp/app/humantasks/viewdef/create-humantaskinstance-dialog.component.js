var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { CreateHumanTaskInstance } from '@app/stores/humantaskinstances/parameters/create-humantaskinstance.model';
var CreateHumanTaskInstanceDialog = (function () {
    function CreateHumanTaskInstanceDialog(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
        this.createHumanTaskInstance = new CreateHumanTaskInstance();
        this.createHumanTaskInstance.humanTaskName = this.data.name;
    }
    CreateHumanTaskInstanceDialog.prototype.update = function () {
        this.dialogRef.close(this.createHumanTaskInstance);
    };
    CreateHumanTaskInstanceDialog.prototype.close = function () {
        this.dialogRef.close();
    };
    CreateHumanTaskInstanceDialog = __decorate([
        Component({
            selector: 'create-humantask-instance-dialog',
            templateUrl: 'create-humantaskinstance-dialog.component.html',
        }),
        __param(1, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [MatDialogRef,
            HumanTaskDef])
    ], CreateHumanTaskInstanceDialog);
    return CreateHumanTaskInstanceDialog;
}());
export { CreateHumanTaskInstanceDialog };
//# sourceMappingURL=create-humantaskinstance-dialog.component.js.map