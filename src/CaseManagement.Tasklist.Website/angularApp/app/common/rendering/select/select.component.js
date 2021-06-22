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
import { FormGroup } from "@angular/forms";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { BaseUIComponent } from "../baseui.component";
var SelectComponent = (function (_super) {
    __extends(SelectComponent, _super);
    function SelectComponent(dialog) {
        var _this = _super.call(this) || this;
        _this.dialog = dialog;
        return _this;
    }
    SelectComponent.prototype.openDialog = function () {
        if (!this.uiOption.editMode) {
            return;
        }
        var dialogRef = this.dialog.open(SelectComponentDialog, {
            data: { opt: this.option }
        });
        dialogRef.afterClosed().subscribe(function (r) {
            if (!r) {
                return;
            }
        });
    };
    SelectComponent = __decorate([
        Component({
            selector: 'view-select',
            templateUrl: 'select.component.html',
            styleUrls: ['./select.component.scss']
        }),
        __metadata("design:paramtypes", [MatDialog])
    ], SelectComponent);
    return SelectComponent;
}(BaseUIComponent));
export { SelectComponent };
var SelectComponentDialog = (function () {
    function SelectComponentDialog(data, dialogRef) {
        this.data = data;
        this.dialogRef = dialogRef;
        this.configureSelectForm = new FormGroup({});
    }
    SelectComponentDialog.prototype.onSave = function (val) {
        if (val) { }
        var opt = this.data.opt;
        this.dialogRef.close(opt);
    };
    SelectComponentDialog = __decorate([
        Component({
            selector: 'view-select-dialog',
            templateUrl: 'selectdialog.component.html',
        }),
        __param(0, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [Object, MatDialogRef])
    ], SelectComponentDialog);
    return SelectComponentDialog;
}());
export { SelectComponentDialog };
//# sourceMappingURL=select.component.js.map