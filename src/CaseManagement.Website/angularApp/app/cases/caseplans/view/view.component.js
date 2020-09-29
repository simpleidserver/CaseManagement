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
import { MatPaginator, MatSnackBar, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCasePlanInstanceActions from '@app/stores/caseplaninstances/actions/caseplaninstance.actions';
import * as fromCasePlanActions from '@app/stores/caseplans/actions/caseplan.actions';
import { CasePlan } from '@app/stores/caseplans/models/caseplan.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
var ViewCaseDefinitionComponent = (function () {
    function ViewCaseDefinitionComponent(casePlanStore, route, actions$, translateService, snackBar) {
        this.casePlanStore = casePlanStore;
        this.route = route;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.snackBar = snackBar;
        this.selectedTimer = "4000";
        this.casePlan$ = new CasePlan();
        this.casePlanInstances$ = new Array();
        this.displayedColumns = ['id', 'state', 'create_datetime', 'actions'];
    }
    ViewCaseDefinitionComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.casePlanStore.pipe(select(fromAppState.selectCasePlanResult)).subscribe(function (casePlan) {
            if (!casePlan) {
                return;
            }
            _this.casePlan$ = casePlan;
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_LAUNCH_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_LAUNCHED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_LAUNCH_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_LAUNCH_CASE_PLAN_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_REACTIVATE_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_REACTIVATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_REACTIVATE_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_REACTIVATE_CASE_PLAN_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_SUSPEND_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_SUSPENDED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_SUSPEND_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_SUSPEND_CASE_PLAN_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_RESUME_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_RESUMED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_RESUME_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_RESUME_CASE_PLAN_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_CLOSE_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_PLAN_INSTANCE_CLOSED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_CLOSE_CASE_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_CLOSE_CASE_PLAN_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.casePlanStore.pipe(select(fromAppState.selectCasePlanInstanceLstResult)).subscribe(function (searchCasePlanInstanceResult) {
            if (!searchCasePlanInstanceResult) {
                return;
            }
            _this.casePlanInstances$ = searchCasePlanInstanceResult.content;
            _this.casePlanInstancesLength = searchCasePlanInstanceResult.totalLength;
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
        var launchCasePlanInstance = new fromCasePlanInstanceActions.LaunchCasePlanInstance(this.casePlan$.id);
        this.casePlanStore.dispatch(launchCasePlanInstance);
    };
    ViewCaseDefinitionComponent.prototype.reactivateCaseInstance = function (caseInstance) {
        var reactivateCasePlanInstance = new fromCasePlanInstanceActions.ReactivateCasePlanInstance(caseInstance.id);
        this.casePlanStore.dispatch(reactivateCasePlanInstance);
    };
    ViewCaseDefinitionComponent.prototype.suspendCaseInstance = function (caseInstance) {
        var suspendCasePlanInstance = new fromCasePlanInstanceActions.SuspendCasePlanInstance(caseInstance.id);
        this.casePlanStore.dispatch(suspendCasePlanInstance);
    };
    ViewCaseDefinitionComponent.prototype.resumeCaseInstance = function (caseInstance) {
        var suspendCasePlanInstance = new fromCasePlanInstanceActions.ResumeCasePlanInstance(caseInstance.id);
        this.casePlanStore.dispatch(suspendCasePlanInstance);
    };
    ViewCaseDefinitionComponent.prototype.closeCaseInstance = function (caseInstance) {
        var suspendCasePlanInstance = new fromCasePlanInstanceActions.CloseCasePlanInstance(caseInstance.id);
        this.casePlanStore.dispatch(suspendCasePlanInstance);
    };
    ViewCaseDefinitionComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.casePlanInstanceSort.sortChange, this.casePlanInstancePaginator.page).subscribe(function () { return _this.refreshCaseInstances(); });
    };
    ViewCaseDefinitionComponent.prototype.refresh = function () {
        this.refreshCaseDefinition();
        this.refreshCaseInstances();
    };
    ViewCaseDefinitionComponent.prototype.refreshCaseDefinition = function () {
        var id = this.route.snapshot.params['id'];
        var loadCaseDefinition = new fromCasePlanActions.StartGet(id);
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
        var active = "create_datetime";
        var direction = "desc";
        if (this.casePlanInstanceSort.active) {
            active = this.casePlanInstanceSort.active;
        }
        if (this.casePlanInstanceSort.direction) {
            direction = this.casePlanInstanceSort.direction;
        }
        var loadCaseInstances = new fromCasePlanInstanceActions.SearchCasePlanInstances(startIndex, count, active, direction, this.route.snapshot.params['id']);
        this.casePlanStore.dispatch(loadCaseInstances);
    };
    ViewCaseDefinitionComponent.prototype.ngOnDestroy = function () {
        clearInterval(this.interval);
    };
    __decorate([
        ViewChild('casePlanInstanceSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "casePlanInstanceSort", void 0);
    __decorate([
        ViewChild('casePlanInstancePaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "casePlanInstancePaginator", void 0);
    ViewCaseDefinitionComponent = __decorate([
        Component({
            selector: 'view-case-files',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            ScannedActionsSubject,
            TranslateService,
            MatSnackBar])
    ], ViewCaseDefinitionComponent);
    return ViewCaseDefinitionComponent;
}());
export { ViewCaseDefinitionComponent };
//# sourceMappingURL=view.component.js.map