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
import { ActionTypes } from './list-actions';
import { FormBuilder } from '@angular/forms';
var ListCaseDefinitionsComponent = (function () {
    function ListCaseDefinitionsComponent(store, formBuilder) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.caseDefinitions = [];
        this.displayedColumns = ['name', 'create_datetime'];
        this.searchForm = this.formBuilder.group({
            text: ''
        });
    }
    ListCaseDefinitionsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.isLoading = true;
        this.isErrorLoadOccured = false;
        this.subscription = this.store.pipe(select('caseDefinitions')).subscribe(function (st) {
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
    ListCaseDefinitionsComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListCaseDefinitionsComponent.prototype.onSubmit = function (evt) {
        if (!evt) {
            return;
        }
        this.refresh();
    };
    ListCaseDefinitionsComponent.prototype.refresh = function () {
        var request = {
            type: ActionTypes.CASEDEFINITIONSLOAD,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize,
            text: this.searchForm.get('text').value
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
    ListCaseDefinitionsComponent.prototype.ngOnDestroy = function () {
        this.subscription.unsubscribe();
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListCaseDefinitionsComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListCaseDefinitionsComponent.prototype, "sort", void 0);
    ListCaseDefinitionsComponent = __decorate([
        Component({
            selector: 'list-case-files',
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss']
        }),
        __metadata("design:paramtypes", [Store, FormBuilder])
    ], ListCaseDefinitionsComponent);
    return ListCaseDefinitionsComponent;
}());
export { ListCaseDefinitionsComponent };
//# sourceMappingURL=list.component.js.map