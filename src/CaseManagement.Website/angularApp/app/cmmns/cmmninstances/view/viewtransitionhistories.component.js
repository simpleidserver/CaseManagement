var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { select, Store } from '@ngrx/store';
var ViewTransitionHistoriesComponent = (function () {
    function ViewTransitionHistoriesComponent(store, route) {
        this.store = store;
        this.route = route;
        this.displayedColumns = ['transition', 'executionDateTime', 'message'];
        this.transitionHistories$ = new MatTableDataSource();
        this.cmmnInstance = null;
    }
    ViewTransitionHistoriesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectCmmnPlanInstanceResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.cmmnInstance = e;
            _this.refresh();
        });
        this.route.params.subscribe(function () {
            _this.refresh();
        });
    };
    ViewTransitionHistoriesComponent.prototype.ngAfterViewInit = function () {
        this.transitionHistories$.sort = this.sort;
    };
    ViewTransitionHistoriesComponent.prototype.refresh = function () {
        if (!this.cmmnInstance) {
            return;
        }
        var id = this.route.snapshot.params['instid'];
        var filteredExecutionPath = this.cmmnInstance.children.filter(function (ep) {
            return ep.id === id;
        });
        if (filteredExecutionPath.length === 1) {
            this.transitionHistories$.data = filteredExecutionPath[0].transitionHistories;
        }
    };
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ViewTransitionHistoriesComponent.prototype, "sort", void 0);
    ViewTransitionHistoriesComponent = __decorate([
        Component({
            selector: 'view-transition-histories',
            templateUrl: './viewtransitionhistories.component.html',
            styleUrls: ['./viewtransitionhistories.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute])
    ], ViewTransitionHistoriesComponent);
    return ViewTransitionHistoriesComponent;
}());
export { ViewTransitionHistoriesComponent };
//# sourceMappingURL=viewtransitionhistories.component.js.map