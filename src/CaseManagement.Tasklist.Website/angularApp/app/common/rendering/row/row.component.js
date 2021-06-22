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
import { FormGroup, FormControl } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material";
import { BaseUIComponent } from "../baseui.component";
import { GuidGenerator } from '../../guidgenerator';
var RowComponent = (function (_super) {
    __extends(RowComponent, _super);
    function RowComponent(dialog) {
        var _this = _super.call(this) || this;
        _this.dialog = dialog;
        return _this;
    }
    RowComponent.prototype.openDialog = function () {
        var dialogRef = this.dialog.open(RowComponentDialog, {
            data: { opt: this.option }
        });
        dialogRef.afterClosed().subscribe(function (r) {
            if (!r) {
                return;
            }
        });
    };
    RowComponent = __decorate([
        Component({
            selector: 'view-row',
            templateUrl: 'row.component.html',
            styleUrls: ['./row.component.scss']
        }),
        __metadata("design:paramtypes", [MatDialog])
    ], RowComponent);
    return RowComponent;
}(BaseUIComponent));
export { RowComponent };
var RowComponentDialog = (function () {
    function RowComponentDialog(data, dialogRef) {
        this.data = data;
        this.dialogRef = dialogRef;
        this.configureRowForm = new FormGroup({
            nbColumns: new FormControl({ value: '' })
        });
        this.configureRowForm.get('nbColumns').setValue(data.opt.children.length);
    }
    RowComponentDialog.prototype.onSave = function (val) {
        var opt = this.data.opt;
        opt.children = [];
        var percentage = (100 / val.nbColumns) + '%';
        for (var i = 0; i < val.nbColumns; i++) {
            opt.children.push({ id: GuidGenerator.newGUID(), children: [], type: 'column', width: percentage });
        }
        this.dialogRef.close(opt);
    };
    RowComponentDialog = __decorate([
        Component({
            selector: 'view-row-dialog',
            templateUrl: 'rowdialog.component.html',
        }),
        __param(0, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [Object, MatDialogRef])
    ], RowComponentDialog);
    return RowComponentDialog;
}());
export { RowComponentDialog };
//# sourceMappingURL=row.component.js.map