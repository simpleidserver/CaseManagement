var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, ViewEncapsulation } from '@angular/core';
var ViewCaseInstanceComponent = (function () {
    function ViewCaseInstanceComponent() {
    }
    ViewCaseInstanceComponent.prototype.ngOnInit = function () {
    };
    ViewCaseInstanceComponent.prototype.ngOnDestroy = function () {
    };
    ViewCaseInstanceComponent = __decorate([
        Component({
            selector: 'view-case-instances',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss'],
            encapsulation: ViewEncapsulation.None
        })
    ], ViewCaseInstanceComponent);
    return ViewCaseInstanceComponent;
}());
export { ViewCaseInstanceComponent };
var CaseElementInstanceDialog = (function () {
    function CaseElementInstanceDialog() {
    }
    CaseElementInstanceDialog = __decorate([
        Component({
            selector: 'case-element-instance-dialog',
            templateUrl: 'case-element-instance-dialog.html',
        })
    ], CaseElementInstanceDialog);
    return CaseElementInstanceDialog;
}());
export { CaseElementInstanceDialog };
//# sourceMappingURL=view.component.js.map