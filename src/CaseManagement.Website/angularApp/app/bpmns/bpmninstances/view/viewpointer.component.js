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
import { ActivatedRoute } from '@angular/router';
var ViewExecutionPointerComponent = (function () {
    function ViewExecutionPointerComponent(route) {
        this.route = route;
        this.id = '';
        this.pathid = '';
        this.eltid = '';
    }
    ViewExecutionPointerComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.route.parent.parent.params.subscribe(function (p) {
            _this.id = p['id'];
        });
        this.route.parent.params.subscribe(function (p) {
            _this.pathid = p['pathid'];
        });
        this.route.params.subscribe(function (p) {
            _this.eltid = p['eltid'];
        });
    };
    ViewExecutionPointerComponent = __decorate([
        Component({
            selector: 'view-execution-pointer',
            templateUrl: './viewpointer.component.html',
            styleUrls: ['./viewpointer.component.scss']
        }),
        __metadata("design:paramtypes", [ActivatedRoute])
    ], ViewExecutionPointerComponent);
    return ViewExecutionPointerComponent;
}());
export { ViewExecutionPointerComponent };
//# sourceMappingURL=viewpointer.component.js.map