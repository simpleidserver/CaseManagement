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
import { FormBuilder } from '@angular/forms';
import { MatPaginator, MatSort } from '@angular/material';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { StartSearch } from '../actions/caseplan';
import * as fromCaseFiles from '../reducers';
import { ActivatedRoute } from '@angular/router';
var ListCasePlansComponent = (function () {
    function ListCasePlansComponent(store, formBuilder, activatedRoute) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.activatedRoute = activatedRoute;
        this.displayedColumns = ['name', 'version', 'create_datetime', 'actions'];
        this.casePlans$ = [];
        this.searchForm = this.formBuilder.group({
            text: '',
            caseFileId: ''
        });
    }
    ListCasePlansComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromCaseFiles.selectSearchResult)).subscribe(function (l) {
            if (!l || !l.Content) {
                return;
            }
            _this.casePlans$ = l.Content;
            _this.length = l.TotalLength;
        });
        this.activatedRoute.queryParams.subscribe(function (params) {
            var caseFileId = params['caseFileId'];
            if (caseFileId) {
                _this.searchForm.controls['caseFileId'].setValue(caseFileId);
            }
            _this.refresh();
        });
    };
    ListCasePlansComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    ListCasePlansComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListCasePlansComponent.prototype.refresh = function () {
        var startIndex = 0;
        var count = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }
        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }
        var request = new StartSearch(this.sort.active, this.sort.direction, count, startIndex, this.searchForm.get('text').value, this.searchForm.get('caseFileId').value);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListCasePlansComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListCasePlansComponent.prototype, "sort", void 0);
    ListCasePlansComponent = __decorate([
        Component({
            selector: 'list-case-plans',
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss']
        }),
        __metadata("design:paramtypes", [Store, FormBuilder, ActivatedRoute])
    ], ListCasePlansComponent);
    return ListCasePlansComponent;
}());
export { ListCasePlansComponent };
//# sourceMappingURL=list.component.js.map