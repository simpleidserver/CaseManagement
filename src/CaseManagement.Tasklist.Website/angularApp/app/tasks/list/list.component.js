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
import { FormBuilder, FormControl } from '@angular/forms';
import { MatDialog, MatPaginator, MatSnackBar, MatSort } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { SearchTasks } from '../../stores/tasks/actions/tasks.actions';
import { NominateTaskDialogComponent } from './nominate-task-dialog.component';
var ListTasksComponent = (function () {
    function ListTasksComponent(store, translate, activatedRoute, router, snackBar, actions$, dialog, formBuilder) {
        this.store = store;
        this.translate = translate;
        this.activatedRoute = activatedRoute;
        this.router = router;
        this.snackBar = snackBar;
        this.actions$ = actions$;
        this.dialog = dialog;
        this.formBuilder = formBuilder;
        this.displayedColumns = ['priority', 'presentationName', 'presentationSubject', 'actualOwner', 'status', 'createdTime', 'actions'];
        this.baseTranslationKey = "TASKS.LIST";
        this.tasks$ = [];
        this.searchTasksForm = this.formBuilder.group({
            actualOwner: new FormControl(''),
            status: new FormControl('')
        });
    }
    ListTasksComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromTaskActions.ActionTypes.COMPLETE_START_TASK; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant(_this.baseTranslationKey + '.TASK_STARTED'), _this.translate.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromTaskActions.ActionTypes.ERROR_START_TASK; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant(_this.baseTranslationKey + '.ERROR_START_TASK'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromTaskActions.ActionTypes.COMPLETE_NOMINATE_TASK; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant(_this.baseTranslationKey + '.TASK_NOMINATED'), _this.translate.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromTaskActions.ActionTypes.ERROR_NOMINATE_TASK; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant(_this.baseTranslationKey + '.ERROR_NOMINATE_TASK'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromTaskActions.ActionTypes.COMPLETE_CLAIM_TASK; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant(_this.baseTranslationKey + '.TASK_CLAIMED'), _this.translate.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromTaskActions.ActionTypes.ERROR_CLAIM_TASK; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant(_this.baseTranslationKey + '.ERROR_CLAIM_TASK'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectTaskLstResult)).subscribe(function (l) {
            if (!l || !l.content) {
                return;
            }
            _this.tasks$ = l.content;
            _this.length = l.totalLength;
        });
        this.activatedRoute.queryParamMap.subscribe(function (p) {
            _this.sort.active = p.get('active');
            _this.sort.direction = p.get('direction');
            _this.paginator.pageSize = p.get('pageSize');
            _this.paginator.pageIndex = p.get('pageIndex');
            var actualOwner = p.get('actualOwner');
            if (actualOwner) {
                _this.searchTasksForm.get('actualOwner').setValue(actualOwner);
            }
            var status = p.get('status');
            if (status) {
                _this.searchTasksForm.get('status').setValue(status.split(','));
            }
            _this.refresh();
        });
        this.translate.onLangChange.subscribe(function () {
            _this.refresh();
        });
    };
    ListTasksComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () {
            _this.refreshUrl();
        });
    };
    ListTasksComponent.prototype.onSearchTasks = function () {
        this.refreshUrl();
    };
    ListTasksComponent.prototype.refreshUrl = function () {
        var queryParams = {
            pageIndex: this.paginator.pageIndex,
            pageSize: this.paginator.pageSize,
            active: this.sort.active,
            direction: this.sort.direction
        };
        var actualOwner = this.searchTasksForm.get('actualOwner').value;
        var status = this.searchTasksForm.get('status').value;
        if (actualOwner) {
            queryParams['actualOwner'] = actualOwner;
        }
        if (status) {
            queryParams['status'] = status.join(',');
        }
        this.router.navigate(['.'], {
            relativeTo: this.activatedRoute,
            queryParams: queryParams
        });
    };
    ListTasksComponent.prototype.refresh = function () {
        var startIndex = 0;
        var count = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }
        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }
        var active = this.getOrder();
        var direction = this.getDirection();
        var request = new SearchTasks(active, direction, count, startIndex, this.searchTasksForm.get('actualOwner').value, this.searchTasksForm.get('status').value);
        this.store.dispatch(request);
    };
    ListTasksComponent.prototype.executeAction = function (act, task) {
        var _this = this;
        switch (act) {
            case 'START':
                {
                    var request = new fromTaskActions.StartTask(task.id);
                    this.store.dispatch(request);
                }
                break;
            case 'NOMINATE':
                var dialogRef = this.dialog.open(NominateTaskDialogComponent);
                dialogRef.afterClosed().subscribe(function (result) {
                    if (!result) {
                        return;
                    }
                    var act = new fromTaskActions.NominateTask(task.id, result);
                    _this.store.dispatch(act);
                });
                break;
            case 'CLAIM':
                {
                    var act_1 = new fromTaskActions.ClaimTask(task.id);
                    this.store.dispatch(act_1);
                }
                break;
            case 'COMPLETE':
                {
                    this.router.navigate(['/tasks/' + task.id]);
                }
                break;
        }
    };
    ListTasksComponent.prototype.getOrder = function () {
        var active = "createdTime";
        if (this.sort.active) {
            active = this.sort.active;
        }
        return active;
    };
    ListTasksComponent.prototype.getDirection = function () {
        var direction = "desc";
        if (this.sort.direction) {
            direction = this.sort.direction;
        }
        return direction;
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListTasksComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListTasksComponent.prototype, "sort", void 0);
    ListTasksComponent = __decorate([
        Component({
            selector: 'list-tasks-component',
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            TranslateService,
            ActivatedRoute,
            Router,
            MatSnackBar,
            ScannedActionsSubject,
            MatDialog,
            FormBuilder])
    ], ListTasksComponent);
    return ListTasksComponent;
}());
export { ListTasksComponent };
//# sourceMappingURL=list.component.js.map