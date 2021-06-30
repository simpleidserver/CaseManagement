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
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
var ListCmmnFilesComponent = (function () {
    function ListCmmnFilesComponent(store) {
        this.store = store;
        this.displayedColumns = ['name', 'version', 'status', 'create_datetime', 'update_datetime'];
        this.cmmnFiles$ = [];
    }
    ListCmmnFilesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.cmmnFilesListener = this.store.pipe(select(fromAppState.selectCmmnFileLstResult)).subscribe(function (searchCmmnFilesResult) {
            if (!searchCmmnFilesResult) {
                return;
            }
            _this.cmmnFiles$ = searchCmmnFilesResult.content;
            _this.length = searchCmmnFilesResult.totalLength;
        });
        this.refresh();
    };
    ListCmmnFilesComponent.prototype.ngOnDestroy = function () {
        this.cmmnFilesListener.unsubscribe();
    };
    ListCmmnFilesComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    ListCmmnFilesComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListCmmnFilesComponent.prototype.refresh = function () {
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
        var request = new fromCmmnFileActions.SearchCmmnFiles(active, direction, count, startIndex, null, null, true);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListCmmnFilesComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListCmmnFilesComponent.prototype, "sort", void 0);
    ListCmmnFilesComponent = __decorate([
        Component({
            selector: 'list-cmmn-files',
            templateUrl: './listfiles.component.html',
            styleUrls: ['./listfiles.component.scss']
        }),
        __metadata("design:paramtypes", [Store])
    ], ListCmmnFilesComponent);
    return ListCmmnFilesComponent;
}());
export { ListCmmnFilesComponent };
//# sourceMappingURL=listfiles.component.js.map