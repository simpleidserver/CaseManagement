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
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import * as fromCmmnPlanActions from '@app/stores/cmmnplans/actions/cmmn-plans.actions';
import { ScannedActionsSubject, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ViewCmmnPlanComponent = (function () {
    function ViewCmmnPlanComponent(store, route, snackBar, actions$, translateService) {
        this.store = store;
        this.route = route;
        this.snackBar = snackBar;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.cmmnFiles$ = [];
        this.cmmnFile = new CmmnFile();
    }
    ViewCmmnPlanComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnPlanActions.ActionTypes.ERROR_GET_CMMN_PLAN; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_GET_CMMN_PLAN'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.refresh();
    };
    ViewCmmnPlanComponent.prototype.refresh = function () {
        this.id = this.route.snapshot.params['id'];
        var request = new fromCmmnPlanActions.GetCmmnPlan(this.id);
        this.store.dispatch(request);
    };
    ViewCmmnPlanComponent = __decorate([
        Component({
            selector: 'view-cmmnplan',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            MatSnackBar,
            ScannedActionsSubject,
            TranslateService])
    ], ViewCmmnPlanComponent);
    return ViewCmmnPlanComponent;
}());
export { ViewCmmnPlanComponent };
//# sourceMappingURL=view.component.js.map