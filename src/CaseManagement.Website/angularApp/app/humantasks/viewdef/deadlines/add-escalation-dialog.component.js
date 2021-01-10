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
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
var AddEscalationDialog = (function () {
    function AddEscalationDialog(dialogRef, formBuilder) {
        this.dialogRef = dialogRef;
        this.formBuilder = formBuilder;
        this.addEscalationForm = this.formBuilder.group({
            condition: new FormControl('', [
                Validators.required
            ])
        });
    }
    AddEscalationDialog.prototype.onAddEscalation = function (form) {
        if (!this.addEscalationForm.valid) {
            return;
        }
        this.dialogRef.close(form);
    };
    AddEscalationDialog.prototype.close = function () {
        this.dialogRef.close();
    };
    AddEscalationDialog = __decorate([
        Component({
            selector: 'add-escalation-dialog',
            templateUrl: 'add-escalation-dialog.component.html',
        }),
        __metadata("design:paramtypes", [MatDialogRef,
            FormBuilder])
    ], AddEscalationDialog);
    return AddEscalationDialog;
}());
export { AddEscalationDialog };
//# sourceMappingURL=add-escalation-dialog.component.js.map