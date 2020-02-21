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
import * as fromCaseInstance from '../actions/caseinstance';
import * as fromCasePlan from '../actions/caseplan';
import * as fromCaseWorker from '../actions/caseworker';
import * as fromFormInstance from '../actions/forminstance';
import { CasePlanService } from '../services/caseplan.service';
var CasePlanEffects = (function () {
    function CasePlanEffects(actions$, casePlanService) {
        var _this = this;
        this.actions$ = actions$;
        this.casePlanService = casePlanService;
        this.searchCasePlans$ = this.actions$
            .pipe(ofType(fromCasePlan.ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.casePlanService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text)
                .pipe(map(function (casePlans) { return { type: fromCasePlan.ActionTypes.COMPLETE_SEARCH, content: casePlans }; }), catchError(function () { return of({ type: fromCasePlan.ActionTypes.COMPLETE_SEARCH }); }));
        }));
        this.getCasePlan$ = this.actions$
            .pipe(ofType(fromCasePlan.ActionTypes.START_GET), mergeMap(function (evt) {
            return _this.casePlanService.get(evt.id)
                .pipe(map(function (casefiles) { return { type: fromCasePlan.ActionTypes.COMPLETE_GET, content: casefiles }; }), catchError(function () { return of({ type: fromCasePlan.ActionTypes.COMPLETE_GET }); }));
        }));
        this.searchCaseInstance$ = this.actions$
            .pipe(ofType(fromCaseInstance.ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.casePlanService.searchCasePlanInstance(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (caseInstances) { return { type: fromCaseInstance.ActionTypes.COMPLETE_SEARCH, content: caseInstances }; }), catchError(function () { return of({ type: fromCaseInstance.ActionTypes.COMPLETE_SEARCH }); }));
        }));
        this.searchWorkerTask$ = this.actions$
            .pipe(ofType(fromCaseWorker.ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.casePlanService.searchWorkerTask(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (workerTask) { return { type: fromCaseWorker.ActionTypes.COMPLETE_SEARCH, content: workerTask }; }), catchError(function () { return of({ type: fromCaseWorker.ActionTypes.COMPLETE_SEARCH }); }));
        }));
        this.searchFormInstance$ = this.actions$
            .pipe(ofType(fromFormInstance.ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.casePlanService.searchFormInstance(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (formInstance) { return { type: fromFormInstance.ActionTypes.COMPLETE_SEARCH, content: formInstance }; }), catchError(function () { return of({ type: fromFormInstance.ActionTypes.COMPLETE_SEARCH }); }));
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
    ], CasePlanEffects.prototype, "searchCaseInstance$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanEffects.prototype, "searchWorkerTask$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanEffects.prototype, "searchFormInstance$", void 0);
    CasePlanEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CasePlanService])
    ], CasePlanEffects);
    return CasePlanEffects;
}());
export { CasePlanEffects };
//# sourceMappingURL=caseplan.js.map