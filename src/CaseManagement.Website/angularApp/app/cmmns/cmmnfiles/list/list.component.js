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
import { MatDialog, MatPaginator, MatSnackBar, MatSort } from '@angular/material';
import { Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { AddCmmnFileDialog } from './add-cmmn-file-dialog';
var ListCmmnFilesComponent = (function () {
    function ListCmmnFilesComponent(store, formBuilder, dialog, actions$, translateService, snackBar, route) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.dialog = dialog;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.snackBar = snackBar;
        this.route = route;
        this.displayedColumns = ['name', 'version', 'status', 'create_datetime', 'update_datetime'];
        this.cmmnFiles$ = [];
        this.searchForm = this.formBuilder.group({
            text: ''
        });
    }
    ListCmmnFilesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.COMPLETE_ADD_CMMNFILE; }))
            .subscribe(function (evt) {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.CASES_FILE_ADDED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.route.navigate(["/cmmns/cmmnfiles/" + evt.id]);
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.ERROR_ADD_CMMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_ADD_CASE_FILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectCmmnFileLstResult)).subscribe(function (searchCmmnFilesResult) {
            if (!searchCmmnFilesResult) {
                return;
            }
            _this.cmmnFiles$ = searchCmmnFilesResult.content;
            _this.length = searchCmmnFilesResult.totalLength;
        });
        this.refresh();
    };
    ListCmmnFilesComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    ListCmmnFilesComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListCmmnFilesComponent.prototype.addCaseFile = function () {
        var _this = this;
        var dialogRef = this.dialog.open(AddCmmnFileDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe(function (e) {
            if (!e) {
                return;
            }
            var request = new fromCmmnFileActions.AddCmmnFile(e.name, e.description);
            _this.store.dispatch(request);
        });
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
        var request = new fromCmmnFileActions.SearchCmmnFiles(active, direction, count, startIndex, this.searchForm.get('text').value, null, true);
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
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            FormBuilder,
            MatDialog,
            ScannedActionsSubject,
            TranslateService,
            MatSnackBar,
            Router])
    ], ListCmmnFilesComponent);
    return ListCmmnFilesComponent;
}());
export { ListCmmnFilesComponent };
//# sourceMappingURL=list.component.js.map