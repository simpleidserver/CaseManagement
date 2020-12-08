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
import * as fromCasePlan from '../actions/caseplan.actions';
import { CasePlanService } from '../services/caseplan.service';
var CasePlanEffects = (function () {
    function CasePlanEffects(actions$, casePlanService) {
        var _this = this;
        this.actions$ = actions$;
        this.casePlanService = casePlanService;
        this.searchCasePlans$ = this.actions$
            .pipe(ofType(fromCasePlan.ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.casePlanService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text, evt.caseFileId)
                .pipe(map(function (casePlans) { return { type: fromCasePlan.ActionTypes.COMPLETE_SEARCH, content: casePlans }; }), catchError(function () { return of({ type: fromCasePlan.ActionTypes.COMPLETE_SEARCH }); }));
        }));
        this.getCasePlan$ = this.actions$
            .pipe(ofType(fromCasePlan.ActionTypes.START_GET), mergeMap(function (evt) {
            return _this.casePlanService.get(evt.id)
                .pipe(map(function (casefiles) { return { type: fromCasePlan.ActionTypes.COMPLETE_GET, content: casefiles }; }), catchError(function () { return of({ type: fromCasePlan.ActionTypes.COMPLETE_GET }); }));
        }));
        this.searchCasePlanHistory = this.actions$
            .pipe(ofType(fromCasePlan.ActionTypes.START_SEARCH_HISTORY), mergeMap(function (evt) {
            return _this.casePlanService.searchHistory(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (caseInstances) { return { type: fromCasePlan.ActionTypes.COMPLETE_SEARCH_HISTORY, content: caseInstances }; }), catchError(function () { return of({ type: fromCasePlan.ActionTypes.COMPLETE_SEARCH_HISTORY }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanEffects.prototype, "searchCasePlans$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanEffects.prototype, "getCasePlan$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanEffects.prototype, "searchCasePlanHistory", void 0);
    CasePlanEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CasePlanService])
    ], CasePlanEffects);
    return CasePlanEffects;
}());
export { CasePlanEffects };
//# sourceMappingURL=caseplan.effects.js.map