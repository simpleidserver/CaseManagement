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
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import * as fromAppState from '../../../stores/appstate';
import { StartSearchHistory } from '../../../stores/caseplans/actions/caseplan.actions';
var HistoryCasePlanComponent = (function () {
    function HistoryCasePlanComponent(route, store) {
        this.route = route;
        this.store = store;
        this.displayedColumns = ['name', 'version', 'create_datetime', 'actions'];
        this.casePlans$ = [];
    }
    HistoryCasePlanComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectCasePlanHistoryLstResult)).subscribe(function (searchCaseFilesResult) {
            if (!searchCaseFilesResult) {
                return;
            }
            _this.casePlans$ = searchCaseFilesResult.content;
            _this.length = searchCaseFilesResult.totalLength;
        });
        this.refresh();
    };
    HistoryCasePlanComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    HistoryCasePlanComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    HistoryCasePlanComponent.prototype.refresh = function () {
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
        var request = new StartSearchHistory(this.route.snapshot.params['id'], active, direction, count, startIndex);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], HistoryCasePlanComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], HistoryCasePlanComponent.prototype, "sort", void 0);
    HistoryCasePlanComponent = __decorate([
        Component({
            selector: 'history-case-plan',
            templateUrl: './history.component.html',
            styleUrls: ['./history.component.scss']
        }),
        __metadata("design:paramtypes", [ActivatedRoute, Store])
    ], HistoryCasePlanComponent);
    return HistoryCasePlanComponent;
}());
export { HistoryCasePlanComponent };
//# sourceMappingURL=history.component.js.map