var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromCasePlan from '../actions/cmmn-plans.actions';
import { CmmnPlanService } from '../services/cmmn-plan.service';
var CmmnPlanEffects = (function () {
    function CmmnPlanEffects(actions$, cmmnPlanService) {
        var _this = this;
        this.actions$ = actions$;
        this.cmmnPlanService = cmmnPlanService;
        this.searchCmmnPlans$ = this.actions$
            .pipe(ofType(fromCasePlan.ActionTypes.SEARCH_CMMN_PLANS), mergeMap(function (evt) {
            return _this.cmmnPlanService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.caseFileId)
                .pipe(map(function (cmmnPlans) { return { type: fromCasePlan.ActionTypes.COMPLETE_SEARCH_CMMN_PLANS, content: cmmnPlans }; }), catchError(function () { return of({ type: fromCasePlan.ActionTypes.ERROR_SEARCH_CMMN_PLANS }); }));
        }));
        this.getCmmnPlan$ = this.actions$
            .pipe(ofType(fromCasePlan.ActionTypes.GET_CMMN_PLAN), mergeMap(function (evt) {
            return _this.cmmnPlanService.get(evt.id)
                .pipe(map(function (cmmnPlan) { return { type: fromCasePlan.ActionTypes.COMPLETE_GET_CMMN_PLAN, content: cmmnPlan }; }), catchError(function () { return of({ type: fromCasePlan.ActionTypes.ERROR_GET_CMMN_PLAN }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnPlanEffects.prototype, "searchCmmnPlans$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnPlanEffects.prototype, "getCmmnPlan$", void 0);
    CmmnPlanEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CmmnPlanService])
    ], CmmnPlanEffects);
    return CmmnPlanEffects;
}());
export { CmmnPlanEffects };
//# sourceMappingURL=cmmn-plans.effects.js.map