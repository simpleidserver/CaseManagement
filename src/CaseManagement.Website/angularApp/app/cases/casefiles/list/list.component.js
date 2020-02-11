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
import { OAuthService } from 'angular-oauth2-oidc';
import { merge } from 'rxjs';
import { StartFetch } from '../actions/case-files';
import * as fromCaseFiles from '../reducers';
import { AddCaseFileDialog } from './add-case-file-dialog';
var ListCaseFilesComponent = (function () {
    function ListCaseFilesComponent(store, formBuilder, oauthService, dialog) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.oauthService = oauthService;
        this.dialog = dialog;
        this.displayedColumns = ['name', 'create_datetime', 'update_datetime'];
        this.searchForm = this.formBuilder.group({
            text: ''
        });
    }
    ListCaseFilesComponent.prototype.ngOnInit = function () {
        this.caseFiles$ = this.store.pipe(select(fromCaseFiles.selectSearchResults));
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
        var claims = this.oauthService.getIdentityClaims();
        var request = new StartFetch(this.sort.active, this.sort.direction, count, startIndex, this.searchForm.get('text').value, claims.sub);
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
        __metadata("design:paramtypes", [Store, FormBuilder, OAuthService, MatDialog])
    ], ListCaseFilesComponent);
    return ListCaseFilesComponent;
}());
export { ListCaseFilesComponent };
//# sourceMappingURL=list.component.js.map