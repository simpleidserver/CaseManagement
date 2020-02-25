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
import * as fromCasePlanInstance from '../actions/caseplaninstance';
import { CasePlanInstanceService } from '../services/caseplaninstance.service';
var CasePlanInstanceEffects = (function () {
    function CasePlanInstanceEffects(actions$, casePlanInstanceService) {
        var _this = this;
        this.actions$ = actions$;
        this.casePlanInstanceService = casePlanInstanceService;
        this.searchCasePlanInstance$ = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.casePlanInstanceService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.casePlanId)
                .pipe(map(function (casePlanInstances) { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH, content: casePlanInstances }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH }); }));
        }));
        this.searchMyCasePlanInstance$ = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.START_SEARCH_ME), mergeMap(function (evt) {
            return _this.casePlanInstanceService.searchMe(evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (casePlanInstances) { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH_ME, content: casePlanInstances }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH_ME }); }));
        }));
        this.selectGetCasePlanInstance = this.actions$
            .pipe(ofType(fromCasePlanInstance.ActionTypes.START_GET), mergeMap(function (evt) {
            return _this.casePlanInstanceService.get(evt.id)
                .pipe(map(function (casePlanInstance) { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_GET, content: casePlanInstance }; }), catchError(function () { return of({ type: fromCasePlanInstance.ActionTypes.COMPLETE_GET }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "searchCasePlanInstance$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "searchMyCasePlanInstance$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasePlanInstanceEffects.prototype, "selectGetCasePlanInstance", void 0);
    CasePlanInstanceEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CasePlanInstanceService])
    ], CasePlanInstanceEffects);
    return CasePlanInstanceEffects;
}());
export { CasePlanInstanceEffects };
//# sourceMappingURL=caseplaninstance.js.map