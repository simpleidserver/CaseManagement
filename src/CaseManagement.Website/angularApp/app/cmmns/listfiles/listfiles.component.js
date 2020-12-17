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
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
var ListBpmnFilesComponent = (function () {
    function ListBpmnFilesComponent(store) {
        this.store = store;
        this.displayedColumns = ['name', 'nbInstances', 'version', 'status', 'create_datetime', 'update_datetime'];
        this.bpmnFiles$ = [];
    }
    ListBpmnFilesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectBpmnFilesResult)).subscribe(function (searchBpmnFilesResult) {
            if (!searchBpmnFilesResult) {
                return;
            }
            _this.bpmnFiles$ = searchBpmnFilesResult.content;
            _this.length = searchBpmnFilesResult.totalLength;
        });
        this.refresh();
    };
    ListBpmnFilesComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    ListBpmnFilesComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListBpmnFilesComponent.prototype.refresh = function () {
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
        var request = new fromBpmnFileActions.SearchBpmnFiles(active, direction, count, startIndex, true, null);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListBpmnFilesComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListBpmnFilesComponent.prototype, "sort", void 0);
    ListBpmnFilesComponent = __decorate([
        Component({
            selector: 'list-bpmn-files',
            templateUrl: './listfiles.component.html',
            styleUrls: ['./listfiles.component.scss']
        }),
        __metadata("design:paramtypes", [Store])
    ], ListBpmnFilesComponent);
    return ListBpmnFilesComponent;
}());
export { ListBpmnFilesComponent };
//# sourceMappingURL=listfiles.component.js.map