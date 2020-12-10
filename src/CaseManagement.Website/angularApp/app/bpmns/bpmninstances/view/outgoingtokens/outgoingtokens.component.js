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
import * as fromAppState from '@app/stores/appstate';
import { select, Store } from '@ngrx/store';
var OutgoingTokensComponent = (function () {
    function OutgoingTokensComponent(store, route) {
        this.store = store;
        this.route = route;
        this.displayedColumns = ['name', 'content'];
        this.outgoingTokens$ = [];
        this.bpmnInstance = null;
    }
    OutgoingTokensComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectBpmnInstanceResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.bpmnInstance = e;
        });
        this.route.parent.parent.params.subscribe(function () {
            _this.refresh();
        });
        this.route.parent.params.subscribe(function () {
            _this.refresh();
        });
    };
    OutgoingTokensComponent.prototype.refresh = function () {
        if (!this.bpmnInstance) {
            return;
        }
        var pathid = this.route.parent.parent.snapshot.params['pathid'];
        var eltid = this.route.parent.snapshot.params['eltid'];
        var filteredExecutionPath = this.bpmnInstance.executionPaths.filter(function (ep) {
            return ep.id === pathid;
        });
        if (filteredExecutionPath.length === 1) {
            var filteredElt = filteredExecutionPath[0].executionPointers.filter(function (ep) {
                return ep.id === eltid;
            });
            if (filteredElt.length === 1) {
                this.outgoingTokens$ = filteredElt[0].outgoingTokens;
            }
        }
    };
    OutgoingTokensComponent = __decorate([
        Component({
            selector: 'list-outgoing-token',
            templateUrl: './outgoingtokens.component.html',
            styleUrls: ['./outgoingtokens.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute])
    ], OutgoingTokensComponent);
    return OutgoingTokensComponent;
}());
export { OutgoingTokensComponent };
//# sourceMappingURL=outgoingtokens.component.js.map