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
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { ActionTypes } from './list-case-defs-actions';
var ListCaseDefsComponent = (function () {
    function ListCaseDefsComponent(store) {
        this.store = store;
        this.caseDefinitions = [];
        this.displayedColumns = ['Id', 'Name', 'CreateDateTime', 'Actions'];
    }
    ListCaseDefsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.isLoading = true;
        this.isErrorLoadOccured = false;
        this.subscription = this.store.pipe(select('caseDefs')).subscribe(function (st) {
            if (!st) {
                return;
            }
            _this.isLoading = st.isLoading;
            _this.isErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                _this.caseDefinitions = st.content.Content;
                _this.length = st.content.TotalLength;
            }
        });
        this.refresh();
    };
    ListCaseDefsComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListCaseDefsComponent.prototype.refresh = function () {
        var request = {
            type: ActionTypes.CASEDEFSLOAD,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize
        };
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            request['startIndex'] = this.paginator.pageIndex * this.paginator.pageSize;
        }
        else {
            request['startIndex'] = 0;
        }
        if (this.paginator.pageSize) {
            request['count'] = this.paginator.pageSize;
        }
        else {
            request['count'] = 5;
        }
        this.isLoading = true;
        this.store.dispatch(request);
    };
    ListCaseDefsComponent.prototype.ngOnDestroy = function () {
        this.subscription.unsubscribe();
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListCaseDefsComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListCaseDefsComponent.prototype, "sort", void 0);
    ListCaseDefsComponent = __decorate([
        Component({
            selector: 'list-case-defs',
            templateUrl: './list-case-defs.component.html',
            styleUrls: ['./list-case-defs.component.scss']
        }),
        __metadata("design:paramtypes", [Store])
    ], ListCaseDefsComponent);
    return ListCaseDefsComponent;
}());
export { ListCaseDefsComponent };
//# sourceMappingURL=list-case-defs.component.js.map