var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCasePlanInstanceActions from '@app/stores/caseplaninstances/actions/caseplaninstance.actions';
import { CasePlanInstanceResult } from '@app/stores/caseplaninstances/models/caseplaninstance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ViewCasePlanInstanceComponent = (function () {
    function ViewCasePlanInstanceComponent(store, route, actions$, translateService, snackBar) {
        this.store = store;
        this.route = route;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.snackBar = snackBar;
        this.casePlanInstance = new CasePlanInstanceResult();
        this.activeActivities = [];
        this.enableActivities = [];
        this.completeActivities = [];
        this.milestones = [];
    }
    ViewCasePlanInstanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_ENABLE_CASE_PLANINSTANCE_ELT; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('ACTIVITY_IS_ENABLED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_ENABLE_CASE_PLANINSTANCE_ELT; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_ENABLE_ACTIVITY'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.store.pipe(select(fromAppState.selectCasePlanInstanceResult)).subscribe(function (casePlanInstance) {
            if (!casePlanInstance) {
                return;
            }
            _this.casePlanInstance = casePlanInstance;
            _this.activeActivities = casePlanInstance.children.filter(function (cp) {
                return _this.isActivity(cp) && cp.state === "Active";
            });
            _this.enableActivities = casePlanInstance.children.filter(function (cp) {
                return _this.isActivity(cp) && cp.state === "Enabled";
            });
            _this.completeActivities = casePlanInstance.children.filter(function (cp) {
                return _this.isActivity(cp) && cp.state === "Completed";
            });
            _this.milestones = casePlanInstance.children.filter(function (cp) {
                return cp.type === "MILESTONE";
            });
        });
        this.refresh();
    };
    ViewCasePlanInstanceComponent.prototype.enable = function (casePlanElementInstance) {
        var suspendCasePlanInstance = new fromCasePlanInstanceActions.EnableCasePlanInstanceElt(this.casePlanInstance.id, casePlanElementInstance.id);
        this.store.dispatch(suspendCasePlanInstance);
    };
    ViewCasePlanInstanceComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var request = new fromCasePlanInstanceActions.GetCasePlanInstance(id);
        this.store.dispatch(request);
    };
    ViewCasePlanInstanceComponent.prototype.isActivity = function (casePlanElementInstance) {
        return casePlanElementInstance.type === "TASK" ||
            casePlanElementInstance.type === "HUMANTASK" ||
            casePlanElementInstance.type === "PROCESSTASK" ||
            casePlanElementInstance.type === "EMPTYTASK";
    };
    ViewCasePlanInstanceComponent = __decorate([
        Component({
            selector: 'view-case-instances',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            ScannedActionsSubject,
            TranslateService,
            MatSnackBar])
    ], ViewCasePlanInstanceComponent);
    return ViewCasePlanInstanceComponent;
}());
export { ViewCasePlanInstanceComponent };
//# sourceMappingURL=view.component.js.map