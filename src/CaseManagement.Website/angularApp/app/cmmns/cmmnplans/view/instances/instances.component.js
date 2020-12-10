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
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromCmmnPlanInstancesActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
var ViewCmmnPlanInstancesComponent = (function () {
    function ViewCmmnPlanInstancesComponent(route, store) {
        this.route = route;
        this.store = store;
        this.displayedColumns = ['name', 'version', 'create_datetime', 'update_datetime', 'nb_instances', 'actions'];
        this.cmmnPlanInstances$ = [];
    }
    ViewCmmnPlanInstancesComponent.prototype.ngOnInit = function () {
        this.refresh();
    };
    ViewCmmnPlanInstancesComponent.prototype.refresh = function () {
        var startIndex = 0;
        var count = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }
        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }
        var active = "create_datetime";
        var direction = "desc";
        if (this.sort.active) {
            active = this.sort.active;
        }
        if (this.sort.direction) {
            direction = this.sort.direction;
        }
        var id = this.route.parent.snapshot.params['id'];
        var request = new fromCmmnPlanInstancesActions.SearchCmmnPlanInstance(active, direction, count, startIndex, id);
        this.store.dispatch(request);
    };
    var _a;
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ViewCmmnPlanInstancesComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ViewCmmnPlanInstancesComponent.prototype, "sort", void 0);
    ViewCmmnPlanInstancesComponent = __decorate([
        Component({
            selector: 'view-cmmn-instances-plan',
            templateUrl: './instances.component.html',
            styleUrls: ['./instances.component.scss']
        }),
        __metadata("design:paramtypes", [ActivatedRoute, typeof (_a = typeof Store !== "undefined" && Store) === "function" ? _a : Object])
    ], ViewCmmnPlanInstancesComponent);
    return ViewCmmnPlanInstancesComponent;
}());
export { ViewCmmnPlanInstancesComponent };
//# sourceMappingURL=instances.component.js.map