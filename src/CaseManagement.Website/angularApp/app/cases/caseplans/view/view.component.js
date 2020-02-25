var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import * as fromCaseInstance from '../actions/caseinstance';
import * as fromCasePlan from '../actions/caseplan';
import * as fromCaseWorker from '../actions/caseworker';
import * as fromFormInstance from '../actions/forminstance';
import { CasePlan } from '../models/caseplan.model';
import * as fromCasePlanDefinitions from '../reducers';
import { CasePlanService } from '../services/caseplan.service';
import { TranslateService } from '@ngx-translate/core';
var ViewCaseDefinitionComponent = (function () {
    function ViewCaseDefinitionComponent(casePlanStore, route, casePlanService, translateService, snackBar) {
        this.casePlanStore = casePlanStore;
        this.route = route;
        this.casePlanService = casePlanService;
        this.translateService = translateService;
        this.snackBar = snackBar;
        this.selectedTimer = "4000";
        this.casePlan$ = new CasePlan();
        this.casePlanInstances$ = new Array();
        this.formInstances$ = new Array();
        this.workerTasks$ = new Array();
        this.displayedColumns = ['id', 'state', 'create_datetime', 'actions'];
        this.formInstanceDisplayedColumns = ['form_id', 'performer', 'status', 'update_datetime', 'create_datetime'];
        this.workerTaskDisplayedColumns = ['type', 'status', 'performer', 'create_datetime', 'update_datetime'];
    }
    ViewCaseDefinitionComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectGetResult)).subscribe(function (casePlan) {
            if (!casePlan) {
                return;
            }
            _this.casePlan$ = casePlan;
        });
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectSearchInstanceResult)).subscribe(function (searchCasePlanInstanceResult) {
            if (!searchCasePlanInstanceResult) {
                return;
            }
            _this.casePlanInstances$ = searchCasePlanInstanceResult.Content;
            _this.casePlanInstancesLength = searchCasePlanInstanceResult.TotalLength;
        });
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectSearchFormInstancesResult)).subscribe(function (searchFormInstanceResult) {
            if (!searchFormInstanceResult) {
                return;
            }
            _this.formInstances$ = searchFormInstanceResult.Content;
            _this.formInstancesLength = searchFormInstanceResult.TotalLength;
        });
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectSearchCaseWorkerResult)).subscribe(function (searchWorkerTaskResult) {
            if (!searchWorkerTaskResult) {
                return;
            }
            _this.workerTasks$ = searchWorkerTaskResult.Content;
            _this.workerTasksLength = searchWorkerTaskResult.TotalLength;
        });
        this.interval = setInterval(function () {
            _this.refresh();
        }, 4000);
        this.refresh();
    };
    ViewCaseDefinitionComponent.prototype.selectTimer = function (evt) {
        var _this = this;
        clearInterval(this.interval);
        this.interval = setInterval(function () {
            _this.refresh();
        }, evt.value);
    };
    ViewCaseDefinitionComponent.prototype.launchCaseInstance = function () {
        var _this = this;
        this.casePlanService.launchCasePlanInstance(this.route.snapshot.params['id']).subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_LAUNCHED'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_LAUNCH_CASE_PLAN_INSTANCE'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    };
    ViewCaseDefinitionComponent.prototype.reactivateCaseInstance = function (caseInstance) {
        var _this = this;
        this.casePlanService.reactivateCasePlanInstance(this.route.snapshot.params['id'], caseInstance.Id).subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_REACTIVATED'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_REACTIVATE_CASE_PLAN_INSTANCE'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    };
    ViewCaseDefinitionComponent.prototype.suspendCaseInstance = function (caseInstance) {
        var _this = this;
        this.casePlanService.suspendCasePlanInstance(this.route.snapshot.params['id'], caseInstance.Id).subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_SUSPENDED'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_SUSPEND_CASE_PLAN_INSTANCE'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    };
    ViewCaseDefinitionComponent.prototype.resumeCaseInstance = function (caseInstance) {
        var _this = this;
        this.casePlanService.resumeCasePlanInstance(this.route.snapshot.params['id'], caseInstance.Id).subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_RESUMED'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_RESUME_CASE_PLAN_INSTANCE'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    };
    ViewCaseDefinitionComponent.prototype.closeCaseInstance = function (caseInstance) {
        var _this = this;
        this.casePlanService.closeCasePlanInstance(this.route.snapshot.params['id'], caseInstance.Id).subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_CLOSED'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_CLOSE_CASE_PLAN_INSTANCE'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    };
    ViewCaseDefinitionComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.casePlanInstanceSort.sortChange, this.casePlanInstancePaginator.page).subscribe(function () { return _this.refreshCaseInstances(); });
        merge(this.formInstanceSort.sortChange, this.formInstancePaginator.page).subscribe(function () { return _this.refreshFormInstances(); });
        merge(this.workerTaskSort.sortChange, this.caseWorkerPaginator.page).subscribe(function () { return _this.refreshWorkerTasks(); });
    };
    ViewCaseDefinitionComponent.prototype.refresh = function () {
        this.refreshCaseDefinition();
        this.refreshCaseInstances();
        this.refreshFormInstances();
        this.refreshWorkerTasks();
    };
    ViewCaseDefinitionComponent.prototype.refreshCaseDefinition = function () {
        var id = this.route.snapshot.params['id'];
        var loadCaseDefinition = new fromCasePlan.StartGet(id);
        this.casePlanStore.dispatch(loadCaseDefinition);
    };
    ViewCaseDefinitionComponent.prototype.refreshCaseInstances = function () {
        var startIndex = 0;
        var count = 5;
        if (this.casePlanInstancePaginator.pageIndex && this.casePlanInstancePaginator.pageSize) {
            startIndex = this.casePlanInstancePaginator.pageIndex * this.casePlanInstancePaginator.pageSize;
        }
        if (this.casePlanInstancePaginator.pageSize) {
            count = this.casePlanInstancePaginator.pageSize;
        }
        var loadCaseInstances = new fromCaseInstance.StartSearch(this.route.snapshot.params['id'], startIndex, count, this.casePlanInstanceSort.active, this.casePlanInstanceSort.direction);
        this.casePlanStore.dispatch(loadCaseInstances);
    };
    ViewCaseDefinitionComponent.prototype.refreshFormInstances = function () {
        var startIndex = 0;
        var count = 5;
        if (this.formInstancePaginator.pageSize) {
            count = this.formInstancePaginator.pageSize;
        }
        if (this.formInstancePaginator.pageIndex && this.formInstancePaginator.pageSize) {
            startIndex = this.formInstancePaginator.pageIndex * this.formInstancePaginator.pageSize;
        }
        var loadFormInstances = new fromFormInstance.StartSearch(this.route.snapshot.params['id'], this.formInstanceSort.active, this.formInstanceSort.direction, count, startIndex);
        this.casePlanStore.dispatch(loadFormInstances);
    };
    ViewCaseDefinitionComponent.prototype.refreshWorkerTasks = function () {
        var count = 5;
        var startIndex = 0;
        if (this.caseWorkerPaginator.pageSize) {
            count = this.caseWorkerPaginator.pageSize;
        }
        if (this.caseWorkerPaginator.pageIndex && this.caseWorkerPaginator.pageSize) {
            startIndex = this.caseWorkerPaginator.pageIndex * this.caseWorkerPaginator.pageSize;
        }
        var loadCaseWorker = new fromCaseWorker.StartSearch(this.route.snapshot.params['id'], this.workerTaskSort.active, this.workerTaskSort.direction, count, startIndex);
        this.casePlanStore.dispatch(loadCaseWorker);
    };
    ViewCaseDefinitionComponent.prototype.ngOnDestroy = function () {
        clearInterval(this.interval);
    };
    __decorate([
        ViewChild('casePlanInstanceSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "casePlanInstanceSort", void 0);
    __decorate([
        ViewChild('formInstanceSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "formInstanceSort", void 0);
    __decorate([
        ViewChild('workerTaskSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "workerTaskSort", void 0);
    __decorate([
        ViewChild('casePlanInstancePaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "casePlanInstancePaginator", void 0);
    __decorate([
        ViewChild('formInstancePaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "formInstancePaginator", void 0);
    __decorate([
        ViewChild('caseWorkerPaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "caseWorkerPaginator", void 0);
    ViewCaseDefinitionComponent = __decorate([
        Component({
            selector: 'view-case-files',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store, ActivatedRoute, CasePlanService, TranslateService, MatSnackBar])
    ], ViewCaseDefinitionComponent);
    return ViewCaseDefinitionComponent;
}());
export { ViewCaseDefinitionComponent };
//# sourceMappingURL=view.component.js.map