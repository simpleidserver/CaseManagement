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
import { MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnPlanActions from '@app/stores/cmmnplans/actions/cmmn-plans.actions';
import * as fromCmmnInstanceActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
var ListCmmnPlansComponent = (function () {
    function ListCmmnPlansComponent(store, route, actions$, snackBar, translateService) {
        this.store = store;
        this.route = route;
        this.actions$ = actions$;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.displayedColumns = ['name', 'version', 'create_datetime', 'update_datetime', 'nb_instances', 'actions'];
        this.cmmnPlans$ = [];
    }
    ListCmmnPlansComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnInstanceActions.ActionTypes.COMPLETE_LAUNCH_CMMN_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.PLAN_INSTANCE_LAUNCHED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectCmmnPlanLstResult)).subscribe(function (searchCmmnPlanResult) {
            if (!searchCmmnPlanResult) {
                return;
            }
            _this.cmmnPlans$ = searchCmmnPlanResult.content;
            _this.length = searchCmmnPlanResult.totalLength;
        });
        this.refresh();
    };
    ListCmmnPlansComponent.prototype.onSubmit = function () {
        this.refresh();
    };
    ListCmmnPlansComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListCmmnPlansComponent.prototype.refresh = function () {
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
        var id = this.route.parent.snapshot.params['id'];
        var request = new fromCmmnPlanActions.SearchCmmnPlans(active, direction, count, startIndex, id);
        this.store.dispatch(request);
    };
    ListCmmnPlansComponent.prototype.launch = function (cmmnPlan) {
        var request = new fromCmmnInstanceActions.LaunchCmmnPlanInstance(cmmnPlan.id);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListCmmnPlansComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListCmmnPlansComponent.prototype, "sort", void 0);
    ListCmmnPlansComponent = __decorate([
        Component({
            selector: 'list-cmmn-plans',
            templateUrl: './plans.component.html',
            styleUrls: ['./plans.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            ScannedActionsSubject,
            MatSnackBar,
            TranslateService])
    ], ListCmmnPlansComponent);
    return ListCmmnPlansComponent;
}());
export { ListCmmnPlansComponent };
//# sourceMappingURL=plans.component.js.map