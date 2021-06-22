var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCaseActions from '@app/stores/cases/actions/cases.actions';
import { GetCase } from '@app/stores/cases/actions/cases.actions';
import { CaseInstance } from '@app/stores/cases/models/caseinstance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ViewCaseComponent = (function () {
    function ViewCaseComponent(store, activatedRoute, translate, actions$, snackBar) {
        this.store = store;
        this.activatedRoute = activatedRoute;
        this.translate = translate;
        this.actions$ = actions$;
        this.snackBar = snackBar;
        this.caseInstance = new CaseInstance();
        this.interval = null;
    }
    Object.defineProperty(ViewCaseComponent.prototype, "activeHumanTasks", {
        get: function () {
            return this.caseInstance.children.filter(function (child) {
                return child.state === 'Active' && child.type === 'HUMANTASK';
            });
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(ViewCaseComponent.prototype, "enabledTasks", {
        get: function () {
            return this.caseInstance.children.filter(function (child) {
                return child.state === 'Enabled';
            });
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(ViewCaseComponent.prototype, "disabledTasks", {
        get: function () {
            return this.caseInstance.children.filter(function (child) {
                return child.state === 'Disabled';
            });
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(ViewCaseComponent.prototype, "completedTasks", {
        get: function () {
            return this.caseInstance.children.filter(function (child) {
                return child.state === 'Completed' && child.type === 'HUMANTASK';
            });
        },
        enumerable: false,
        configurable: true
    });
    ViewCaseComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseActions.ActionTypes.ERROR_COMPLETE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('CASES.MESSAGES.ERROR_COMPLETE_CASE'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseActions.ActionTypes.COMPLETED; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('CASES.MESSAGES.COMPLETED'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseActions.ActionTypes.ERROR_GET_CASE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('CASES.MESSAGES.ERROR_GET_CASE'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseActions.ActionTypes.COMPLETE_ACTIVATE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('CASES.MESSAGES.ACTIVATED'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseActions.ActionTypes.ERROR_ACTIVATE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('CASES.MESSAGES.ERROR_ACTIVATE'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseActions.ActionTypes.COMPLETE_DISABLE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('CASES.MESSAGES.DISABLED'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseActions.ActionTypes.ERROR_DISABLE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('CASES.MESSAGES.ERROR_DISABLE'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseActions.ActionTypes.COMPLETE_REENABLE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('CASES.MESSAGES.REENABLED'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseActions.ActionTypes.ERROR_REENABLE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('CASES.MESSAGES.ERROR_REENABLE'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectCaseResult)).subscribe(function (l) {
            if (!l) {
                return;
            }
            _this.caseInstance = l;
        });
        this.interval = setInterval(function () {
            _this.refresh();
        }, 2000);
        this.refresh();
    };
    ViewCaseComponent.prototype.ngOnDestroy = function () {
        if (this.interval) {
            clearInterval(this.interval);
        }
    };
    ViewCaseComponent.prototype.refresh = function () {
        var id = this.activatedRoute.snapshot.params['id'];
        var request = new GetCase(id);
        this.store.dispatch(request);
    };
    ViewCaseComponent.prototype.enableTask = function (task) {
        var id = this.activatedRoute.snapshot.params['id'];
        var request = new fromCaseActions.Activate(id, task.id);
        this.store.dispatch(request);
    };
    ViewCaseComponent.prototype.disableTask = function (task) {
        var id = this.activatedRoute.snapshot.params['id'];
        var request = new fromCaseActions.Disable(id, task.id);
        this.store.dispatch(request);
    };
    ViewCaseComponent.prototype.reenableTask = function (task) {
        var id = this.activatedRoute.snapshot.params['id'];
        var request = new fromCaseActions.Reenable(id, task.id);
        this.store.dispatch(request);
    };
    ViewCaseComponent.prototype.canConfirmTask = function (task) {
        var filtered = this.caseInstance.workerTasks.filter(function (workerTask) {
            return workerTask.casePlanElementInstanceId === task.id;
        });
        return filtered.length !== 1;
    };
    ViewCaseComponent.prototype.getFormUrl = function (task) {
        var filtered = this.caseInstance.workerTasks.filter(function (workerTask) {
            return workerTask.casePlanElementInstanceId === task.id;
        });
        if (filtered.length !== 1) {
            return '/cases/' + this.caseInstance.id;
        }
        return '/cases/' + this.caseInstance.id + '/' + filtered[0].externalId;
    };
    ViewCaseComponent.prototype.confirmTask = function (task) {
        var id = this.activatedRoute.snapshot.params['id'];
        var request = new fromCaseActions.Complete(id, task.id);
        this.store.dispatch(request);
    };
    ViewCaseComponent = __decorate([
        Component({
            selector: 'view-case-component',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            TranslateService,
            ScannedActionsSubject,
            MatSnackBar])
    ], ViewCaseComponent);
    return ViewCaseComponent;
}());
export { ViewCaseComponent };
//# sourceMappingURL=view.component.js.map