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
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
var ListBpmnInstancesComponent = (function () {
    function ListBpmnInstancesComponent(store, route) {
        this.store = store;
        this.route = route;
        this.displayedColumns = ['status', 'create_datetime', 'update_datetime', 'nbExecutionPath', 'actions'];
        this.bpmnInstances$ = [];
    }
    ListBpmnInstancesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectBpmnInstancesResult)).subscribe(function (searchBpmnInstancesResult) {
            if (!searchBpmnInstancesResult) {
                return;
            }
            _this.bpmnInstances$ = searchBpmnInstancesResult.content;
            _this.length = searchBpmnInstancesResult.totalLength;
        });
        this.interval = setInterval(function () {
            _this.refresh();
        }, 2000);
        this.refresh();
    };
    ListBpmnInstancesComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    ListBpmnInstancesComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListBpmnInstancesComponent.prototype.refresh = function () {
        var id = this.route.parent.snapshot.params['id'];
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
        var request = new fromBpmnInstanceActions.SearchBpmnInstances(active, direction, count, startIndex, id);
        this.store.dispatch(request);
    };
    ListBpmnInstancesComponent.prototype.ngOnDestroy = function () {
        if (this.interval) {
            clearInterval(this.interval);
        }
    };
    ListBpmnInstancesComponent.prototype.start = function (evt, bpmnInstance) {
        evt.preventDefault();
        var request = new fromBpmnInstanceActions.StartBpmnInstance(bpmnInstance.id);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListBpmnInstancesComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListBpmnInstancesComponent.prototype, "sort", void 0);
    ListBpmnInstancesComponent = __decorate([
        Component({
            selector: 'list-bpmn-instances',
            templateUrl: './instances.component.html',
            styleUrls: ['./instances.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute])
    ], ListBpmnInstancesComponent);
    return ListBpmnInstancesComponent;
}());
export { ListBpmnInstancesComponent };
//# sourceMappingURL=instances.component.js.map