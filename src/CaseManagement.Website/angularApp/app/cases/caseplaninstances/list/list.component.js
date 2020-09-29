var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { SearchCasePlanInstances } from '@app/stores/caseplaninstances/actions/caseplaninstance.actions';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
var ListCasePlanInstancesComponent = (function () {
    function ListCasePlanInstancesComponent(store, formBuilder, activatedRoute) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.activatedRoute = activatedRoute;
        this.displayedColumns = ['id', 'state', 'create_datetime', 'actions'];
        this.casePlanInstances$ = [];
        this.searchForm = this.formBuilder.group({
            casePlanId: ''
        });
    }
    ListCasePlanInstancesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectCasePlanInstanceLstResult)).subscribe(function (searchCasePlanInstanceResult) {
            if (!searchCasePlanInstanceResult) {
                return;
            }
            _this.length = searchCasePlanInstanceResult.totalLength;
            _this.casePlanInstances$ = searchCasePlanInstanceResult.content;
        });
        this.activatedRoute.queryParams.subscribe(function (params) {
            var casePlanId = params['casePlanId'];
            if (casePlanId) {
                _this.searchForm.controls['casePlanId'].setValue(casePlanId);
            }
            _this.refresh();
        });
    };
    ListCasePlanInstancesComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    ListCasePlanInstancesComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListCasePlanInstancesComponent.prototype.refresh = function () {
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
        var request = new SearchCasePlanInstances(startIndex, count, active, direction, this.searchForm.get('casePlanId').value);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListCasePlanInstancesComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListCasePlanInstancesComponent.prototype, "sort", void 0);
    ListCasePlanInstancesComponent = __decorate([
        Component({
            selector: 'list-case-instances',
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store, FormBuilder, ActivatedRoute])
    ], ListCasePlanInstancesComponent);
    return ListCasePlanInstancesComponent;
}());
export { ListCasePlanInstancesComponent };
//# sourceMappingURL=list.component.js.map