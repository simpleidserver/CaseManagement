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
import { FormBuilder } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { CaseFilesService } from '../services/casefiles.service';
var AddCaseFileDialog = (function () {
    function AddCaseFileDialog(dialogRef, caseFilesService, formBuilder) {
        this.dialogRef = dialogRef;
        this.caseFilesService = caseFilesService;
        this.formBuilder = formBuilder;
        this.addCaseFileForm = this.formBuilder.group({
            name: '',
            description: ''
        });
    }
    AddCaseFileDialog.prototype.onSubmit = function () {
        var _this = this;
        this.caseFilesService.add(this.addCaseFileForm.get('name').value, this.addCaseFileForm.get('description').value).subscribe(function () {
            _this.dialogRef.close();
        });
    };
    AddCaseFileDialog = __decorate([
        Component({
            selector: 'add-case-file-dialog',
            templateUrl: 'add-case-file-dialog.html',
        }),
        __metadata("design:paramtypes", [MatDialogRef, CaseFilesService, FormBuilder])
    ], AddCaseFileDialog);
    return AddCaseFileDialog;
}());
export { AddCaseFileDialog };
//# sourceMappingURL=add-case-file-dialog.js.map