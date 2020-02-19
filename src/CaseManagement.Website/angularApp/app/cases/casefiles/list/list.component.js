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
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { StartSearch } from '../actions/case-files';
import * as fromCaseFiles from '../reducers';
import { AddCaseFileDialog } from './add-case-file-dialog';
var ListCaseFilesComponent = (function () {
    function ListCaseFilesComponent(store, formBuilder, dialog) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.dialog = dialog;
        this.displayedColumns = ['name', 'version', 'status', 'create_datetime', 'update_datetime', 'actions'];
        this.caseFiles$ = [];
        this.searchForm = this.formBuilder.group({
            text: ''
        });
    }
    ListCaseFilesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromCaseFiles.selectSearchResults)).subscribe(function (searchCaseFilesResult) {
            if (!searchCaseFilesResult) {
                return;
            }
            _this.caseFiles$ = searchCaseFilesResult.Content;
            _this.length = searchCaseFilesResult.TotalLength;
        });
        this.refresh();
    };
    ListCaseFilesComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    ListCaseFilesComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListCaseFilesComponent.prototype.addCaseFile = function () {
        var _this = this;
        var dialogRef = this.dialog.open(AddCaseFileDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe(function () {
            _this.refresh();
        });
    };
    ListCaseFilesComponent.prototype.refresh = function () {
        var startIndex = 0;
        var count = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }
        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }
        var request = new StartSearch(this.sort.active, this.sort.direction, count, startIndex, this.searchForm.get('text').value);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListCaseFilesComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListCaseFilesComponent.prototype, "sort", void 0);
    ListCaseFilesComponent = __decorate([
        Component({
            selector: 'list-case-files',
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss']
        }),
        __metadata("design:paramtypes", [Store, FormBuilder, MatDialog])
    ], ListCaseFilesComponent);
    return ListCaseFilesComponent;
}());
export { ListCaseFilesComponent };
//# sourceMappingURL=list.component.js.map