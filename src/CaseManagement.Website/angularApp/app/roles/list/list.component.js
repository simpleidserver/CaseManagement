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
import * as fromRole from '../reducers';
import { Store, select } from '@ngrx/store';
import { MatSort, MatPaginator } from '@angular/material';
import { StartSearch } from '../actions/role';
import { merge } from 'rxjs';
var ListRolesComponent = (function () {
    function ListRolesComponent(store) {
        this.store = store;
        this.displayedColumns = ['name', 'create_datetime', 'update_datetime', 'actions'];
        this.roles$ = [];
    }
    ListRolesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromRole.selectSearchResults)).subscribe(function (l) {
            if (!l || !l.Content) {
                return;
            }
            _this.roles$ = l.Content;
            _this.length = l.TotalLength;
        });
        this.refresh();
    };
    ListRolesComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListRolesComponent.prototype.refresh = function () {
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
        var request = new StartSearch(startIndex, count, active, direction);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListRolesComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListRolesComponent.prototype, "sort", void 0);
    ListRolesComponent = __decorate([
        Component({
            selector: 'list-roles',
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss']
        }),
        __metadata("design:paramtypes", [Store])
    ], ListRolesComponent);
    return ListRolesComponent;
}());
export { ListRolesComponent };
//# sourceMappingURL=list.component.js.map