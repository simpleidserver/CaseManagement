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
import { MatDialog, MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefsActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { merge } from 'rxjs';
import { AddHumanTaskDefDialog } from './add-humantaskdef-dialog.component';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ListHumanTaskFilesComponent = (function () {
    function ListHumanTaskFilesComponent(store, dialog, actions$, translateService, snackBar) {
        this.store = store;
        this.dialog = dialog;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.snackBar = snackBar;
        this.displayedColumns = ['name', 'version', 'nbInstances', 'create_datetime', 'update_datetime'];
        this.humanTaskFiles$ = [];
    }
    ListHumanTaskFilesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefsActions.ActionTypes.COMPLETE_ADD_HUMANTASKDEF; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('HUMANTASK.MESSAGES.HUMANTASK_CREATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefsActions.ActionTypes.ERROR_ADD_HUMANTASKDEF; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_HUMANTASKDEF'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.humanTaskDefsListener = this.store.pipe(select(fromAppState.selectHumanTasksResult)).subscribe(function (searchHumanTaskFilesResult) {
            if (!searchHumanTaskFilesResult) {
                return;
            }
            _this.humanTaskFiles$ = searchHumanTaskFilesResult.content;
            _this.length = searchHumanTaskFilesResult.totalLength;
        });
        this.refresh();
    };
    ListHumanTaskFilesComponent.prototype.ngOnDestroy = function () {
        this.humanTaskDefsListener.unsubscribe();
    };
    ListHumanTaskFilesComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    ListHumanTaskFilesComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListHumanTaskFilesComponent.prototype.addHumanTask = function () {
        var _this = this;
        var dialogRef = this.dialog.open(AddHumanTaskDefDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe(function (e) {
            if (!e) {
                return;
            }
            var request = new fromHumanTaskDefsActions.AddHumanTaskDefOperation(e.name);
            _this.store.dispatch(request);
        });
    };
    ListHumanTaskFilesComponent.prototype.refresh = function () {
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
        var request = new fromHumanTaskDefsActions.SearchHumanTaskDefOperation(active, direction, count, startIndex);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListHumanTaskFilesComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListHumanTaskFilesComponent.prototype, "sort", void 0);
    ListHumanTaskFilesComponent = __decorate([
        Component({
            selector: 'list-humantask-files',
            templateUrl: './listfiles.component.html',
            styleUrls: ['./listfiles.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            MatDialog,
            ScannedActionsSubject,
            TranslateService,
            MatSnackBar])
    ], ListHumanTaskFilesComponent);
    return ListHumanTaskFilesComponent;
}());
export { ListHumanTaskFilesComponent };
//# sourceMappingURL=listfiles.component.js.map