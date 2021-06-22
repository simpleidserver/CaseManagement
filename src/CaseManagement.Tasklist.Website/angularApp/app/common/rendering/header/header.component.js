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
import { FormControl, FormGroup } from "@angular/forms";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { BaseUIComponent } from "../baseui.component";
var HeaderComponent = (function (_super) {
    __extends(HeaderComponent, _super);
    function HeaderComponent(dialog) {
        var _this = _super.call(this) || this;
        _this.dialog = dialog;
        return _this;
    }
    HeaderComponent.prototype.openDialog = function () {
        var dialogRef = this.dialog.open(HeaderComponentDialog, {
            data: { opt: this.option }
        });
        dialogRef.afterClosed().subscribe(function (r) {
            if (!r) {
                return;
            }
        });
    };
    HeaderComponent = __decorate([
        Component({
            selector: 'view-header',
            templateUrl: 'header.component.html',
            styleUrls: ['./header.component.scss']
        }),
        __metadata("design:paramtypes", [MatDialog])
    ], HeaderComponent);
    return HeaderComponent;
}(BaseUIComponent));
export { HeaderComponent };
var HeaderComponentDialog = (function () {
    function HeaderComponentDialog(data, dialogRef) {
        this.data = data;
        this.dialogRef = dialogRef;
        this.configureHeaderForm = new FormGroup({
            txt: new FormControl({ value: '' }),
            class: new FormControl({ value: '' })
        });
        this.configureHeaderForm.get('txt').setValue(data.opt.txt);
        this.configureHeaderForm.get('class').setValue(data.opt.class);
    }
    HeaderComponentDialog.prototype.onSave = function (val) {
        var opt = this.data.opt;
        opt.txt = val.txt;
        opt.class = val.class;
        this.dialogRef.close(opt);
    };
    HeaderComponentDialog = __decorate([
        Component({
            selector: 'view-header-dialog',
            templateUrl: 'headerdialog.component.html',
        }),
        __param(0, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [Object, MatDialogRef])
    ], HeaderComponentDialog);
    return HeaderComponentDialog;
}());
export { HeaderComponentDialog };
//# sourceMappingURL=header.component.js.map