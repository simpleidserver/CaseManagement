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
import * as fromCmmnPlanInstance from '../actions/cmmn-instances.actions';
import { CmmnPlanInstanceService } from '../services/cmmn-planinstance.service';
var CmmnPlanInstanceEffects = (function () {
    function CmmnPlanInstanceEffects(actions$, cmmnPlanInstanceService) {
        var _this = this;
        this.actions$ = actions$;
        this.cmmnPlanInstanceService = cmmnPlanInstanceService;
        this.launchCasePlanInstance = this.actions$
            .pipe(ofType(fromCmmnPlanInstance.ActionTypes.LAUNCH_CMMN_PLANINSTANCE), mergeMap(function (evt) {
            return _this.cmmnPlanInstanceService.createCasePlanInstance(evt.cmmnPlanId)
                .pipe(mergeMap(function (r) {
                return _this.cmmnPlanInstanceService.launchCasePlanInstance(r.id)
                    .pipe(map(function () { return { type: fromCmmnPlanInstance.ActionTypes.COMPLETE_LAUNCH_CMMN_PLANINSTANCE, cmmnPlanInstance: r }; }), catchError(function () { return of({ type: fromCmmnPlanInstance.ActionTypes.ERROR_LAUNCH_CMMN_PLANINSTANCE }); }));
            }), catchError(function () { return of({ type: fromCmmnPlanInstance.ActionTypes.ERROR_LAUNCH_CMMN_PLANINSTANCE }); }));
        }));
        this.searchCasePlanInstances = this.actions$
            .pipe(ofType(fromCmmnPlanInstance.ActionTypes.SEARCH_CMMN_PLANINSTANCE), mergeMap(function (evt) {
            return _this.cmmnPlanInstanceService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.casePlanId, evt.caseFileId)
                .pipe(map(function (cmmnPlanInstances) { return { type: fromCmmnPlanInstance.ActionTypes.COMPLETE_SEARCH_CMMN_PLANINSTANCE, content: cmmnPlanInstances }; }), catchError(function () { return of({ type: fromCmmnPlanInstance.ActionTypes.ERROR_SEARCH_CMMN_PLANINSTANCE }); }));
        }));
        this.getCasePlanInstance = this.actions$
            .pipe(ofType(fromCmmnPlanInstance.ActionTypes.GET_CMMN_PLANINSTANCE), mergeMap(function (evt) {
            return _this.cmmnPlanInstanceService.get(evt.cmmnPlanInstanceId)
                .pipe(map(function (cmmnPlanInstance) { return { type: fromCmmnPlanInstance.ActionTypes.COMPLETE_GET_CMMN_PLANINSTANCE, content: cmmnPlanInstance }; }), catchError(function () { return of({ type: fromCmmnPlanInstance.ActionTypes.ERROR_GET_CMMN_PLANINSTANCE }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnPlanInstanceEffects.prototype, "launchCasePlanInstance", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnPlanInstanceEffects.prototype, "searchCasePlanInstances", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnPlanInstanceEffects.prototype, "getCasePlanInstance", void 0);
    CmmnPlanInstanceEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CmmnPlanInstanceService])
    ], CmmnPlanInstanceEffects);
    return CmmnPlanInstanceEffects;
}());
export { CmmnPlanInstanceEffects };
//# sourceMappingURL=cmmn-planinstance.effects.js.map