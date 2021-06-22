var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
import { Component, Inject } from "@angular/core";
import { BaseUIComponent } from "../baseui.component";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material";
import { FormGroup, FormControl } from "@angular/forms";
var TxtComponent = (function (_super) {
    __extends(TxtComponent, _super);
    function TxtComponent(dialog) {
        var _this = _super.call(this) || this;
        _this.dialog = dialog;
        _this.control = new FormControl('');
        return _this;
    }
    TxtComponent.prototype.init = function () {
        var _this = this;
        this.subscription = this.control.valueChanges.subscribe(function () {
            _this.option.value = _this.control.value;
        });
    };
    TxtComponent.prototype.ngOnDestroy = function () {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    };
    TxtComponent.prototype.openDialog = function () {
        if (!this.uiOption.editMode) {
            return;
        }
        var dialogRef = this.dialog.open(TxtComponentDialog, {
            data: { opt: this.option }
        });
        dialogRef.afterClosed().subscribe(function (r) {
            if (!r) {
                return;
            }
        });
    };
    TxtComponent = __decorate([
        Component({
            selector: 'view-txt',
            templateUrl: 'txt.component.html',
            styleUrls: ['./txt.component.scss']
        }),
        __metadata("design:paramtypes", [MatDialog])
    ], TxtComponent);
    return TxtComponent;
}(BaseUIComponent));
export { TxtComponent };
var TxtComponentDialog = (function () {
    function TxtComponentDialog(data, dialogRef) {
        this.data = data;
        this.dialogRef = dialogRef;
        this.configureTxtForm = new FormGroup({
            label: new FormControl({ value: '' }),
            name: new FormControl({ value: '' })
        });
        this.configureTxtForm.get('label').setValue(data.opt.label);
        this.configureTxtForm.get('name').setValue(data.opt.name);
    }
    TxtComponentDialog.prototype.onSave = function (val) {
        var opt = this.data.opt;
        this.data.opt.label = val.label;
        this.data.opt.name = val.name;
        this.dialogRef.close(opt);
    };
    TxtComponentDialog = __decorate([
        Component({
            selector: 'view-txt-dialog',
            templateUrl: 'txtdialog.component.html',
        }),
        __param(0, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [Object, MatDialogRef])
    ], TxtComponentDialog);
    return TxtComponentDialog;
}());
export { TxtComponentDialog };
//# sourceMappingURL=txt.component.js.map