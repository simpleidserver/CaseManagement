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
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Escalation } from '@app/stores/common/escalation.model';
import { ToPart } from '@app/stores/common/topart.model';
var EditEscalationDialog = (function () {
    function EditEscalationDialog(dialogRef, formBuilder, data) {
        this.dialogRef = dialogRef;
        this.formBuilder = formBuilder;
        this.data = data;
        this.baseTranslationKey = "HUMANTASK.DEF.VIEW.DEADLINES";
        this.updateEscalationForm = this.formBuilder.group({
            condition: new FormControl('', [
                Validators.required
            ])
        });
        this.addToPartForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            expression: new FormControl('', [
                Validators.required
            ])
        });
        this.updateNotificationForm = this.formBuilder.group({
            name: new FormControl('', [
                Validators.required
            ]),
            priority: ''
        });
        this.updateEscalationForm.get('condition').setValue(this.data.condition);
        this.updateNotificationForm.get('name').setValue(this.data.notification.name);
        this.updateNotificationForm.get('priority').setValue(this.data.notification.priority);
    }
    EditEscalationDialog.prototype.addToPart = function (form) {
        if (!this.addToPartForm.valid) {
            return;
        }
        var toPart = new ToPart();
        toPart.name = form.name;
        toPart.expression = form.expression;
        this.addToPartForm.reset();
        this.data.toParts.push(toPart);
    };
    EditEscalationDialog.prototype.deleteToPart = function (toPart) {
        var index = this.data.toParts.indexOf(toPart);
        this.data.toParts.splice(index, 1);
    };
    EditEscalationDialog.prototype.updateBusinessAdministrator = function (evt) {
        this.data.notification.peopleAssignment.businessAdministrator = evt;
    };
    EditEscalationDialog.prototype.updateRecipient = function (evt) {
        this.data.notification.peopleAssignment.recipient = evt;
    };
    EditEscalationDialog.prototype.addInputParameter = function (param) {
        this.data.notification.operation.inputParameters.push(param);
    };
    EditEscalationDialog.prototype.addOutputParameter = function (param) {
        this.data.notification.operation.outputParameters.push(param);
    };
    EditEscalationDialog.prototype.deleteInputParameter = function (param) {
        var i = this.data.notification.operation.inputParameters.indexOf(param);
        this.data.notification.operation.outputParameters.splice(i, 1);
    };
    EditEscalationDialog.prototype.deleteOutputParameter = function (param) {
        var i = this.data.notification.operation.outputParameters.indexOf(param);
        this.data.notification.operation.outputParameters.splice(i, 1);
    };
    EditEscalationDialog.prototype.updateEscalation = function () {
        if (!this.updateEscalationForm.valid || !this.updateNotificationForm.valid) {
            return;
        }
        this.data.condition = this.updateEscalationForm.get('condition').value;
        this.data.notification.name = this.updateNotificationForm.get('name').value;
        this.data.notification.priority = this.updateNotificationForm.get('priority').value;
        this.dialogRef.close(this.data);
    };
    EditEscalationDialog = __decorate([
        Component({
            selector: 'edit-escalation-dialog',
            templateUrl: 'edit-escalation-dialog.component.html',
        }),
        __param(2, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [MatDialogRef,
            FormBuilder,
            Escalation])
    ], EditEscalationDialog);
    return EditEscalationDialog;
}());
export { EditEscalationDialog };
//# sourceMappingURL=edit-escalation-dialog.component.js.map