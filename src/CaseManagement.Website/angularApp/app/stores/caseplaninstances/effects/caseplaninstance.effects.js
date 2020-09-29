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
import * as fromCasePlanInstance from '../actions/caseplaninstance.actions';
import { CasePlanInstanceService } from '../services/caseplaninstance.service';
var CasePlanInstanceEffects = (function () {
    function CasePlanInstanceEffects(actions$, casePlanInstanceService) {
        var _this = this;
        this.actions$ = actions$;
        this.casePlanInstanceService = casePlanInstanceService;
        this.searchCasePlanInstances = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.START_SEARCH_CASE_PLANINSTANCES), mergeMap(function (evt) {
            return _this.casePlanInstanceService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.casePlanId)
                .pipe(map(function (casePlanInstances) { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH_CASE_PLANINSTANCES, content: casePlanInstances }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.ERROR_SEARCH_CASE_PLANINSTANCES }); }));
        }));
        this.getCasePlanInstance = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.START_GET_CASE_PLANINSTANCE), mergeMap(function (evt) {
            return _this.casePlanInstanceService.get(evt.id)
                .pipe(map(function (casePlanInstance) { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_GET_CASE_PLANINSTANCE, content: casePlanInstance }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.ERROR_GET_CASE_PLANINSTANCE }); }));
        }));
        this.launchCasePlanInstance = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.LAUNCH_CASE_PLANINSTANCE), mergeMap(function (evt) {
            return _this.casePlanInstanceService.createCasePlanInstance(evt.casePlanId)
                .pipe(mergeMap(function (r) {
                return _this.casePlanInstanceService.launchCasePlanInstance(r.id)
                    .pipe(map(function () { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_LAUNCH_CASE_PLANINSTANCE }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.ERROR_LAUNCH_CASE_PLANINSTANCE }); }));
            }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.ERROR_LAUNCH_CASE_PLANINSTANCE }); }));
        }));
        this.reactivateCasePlanInstance = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.REACTIVATE_CASE_PLANINSTANCE), mergeMap(function (evt) {
            return _this.casePlanInstanceService.reactivateCasePlanInstance(evt.casePlanInstanceId)
                .pipe(map(function () { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_REACTIVATE_CASE_PLANINSTANCE }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.ERROR_REACTIVATE_CASE_PLANINSTANCE }); }));
        }));
        this.suspendCasePlanInstance = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.SUSPEND_CASE_PLANINSTANCE), mergeMap(function (evt) {
            return _this.casePlanInstanceService.suspendCasePlanInstance(evt.casePlanInstanceId)
                .pipe(map(function () { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_SUSPEND_CASE_PLANINSTANCE }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.ERROR_SUSPEND_CASE_PLANINSTANCE }); }));
        }));
        this.resumeCasePlanInstance = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.RESUME_CASE_PLANINSTANCE), mergeMap(function (evt) {
            return _this.casePlanInstanceService.resumeCasePlanInstance(evt.casePlanInstanceId)
                .pipe(map(function () { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_RESUME_CASE_PLANINSTANCE }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.ERROR_RESUME_CASE_PLANINSTANCE }); }));
        }));
        this.closeCasePlanInstance = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.CLOSE_CASE_PLANINSTANCE), mergeMap(function (evt) {
            return _this.casePlanInstanceService.closeCasePlanInstance(evt.casePlanInstanceId)
                .pipe(map(function () { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_CLOSE_CASE_PLANINSTANCE }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.ERROR_CLOSE_CASE_PLANINSTANCE }); }));
        }));
        this.enableCasePlanInstanceElt = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.ENABLE_CASE_PLANINSTANCE_ELT), mergeMap(function (evt) {
            return _this.casePlanInstanceService.enable(evt.casePlanInstanceId, evt.casePlanInstanceEltId)
                .pipe(map(function () { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_ENABLE_CASE_PLANINSTANCE_ELT }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.ERROR_ENABLE_CASE_PLANINSTANCE_ELT }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "searchCasePlanInstances", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "getCasePlanInstance", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "launchCasePlanInstance", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "reactivateCasePlanInstance", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "suspendCasePlanInstance", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "resumeCasePlanInstance", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "closeCasePlanInstance", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "enableCasePlanInstanceElt", void 0);
    CasePlanInstanceEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CasePlanInstanceService])
    ], CasePlanInstanceEffects);
    return CasePlanInstanceEffects;
}());
export { CasePlanInstanceEffects };
//# sourceMappingURL=caseplaninstance.effects.js.map