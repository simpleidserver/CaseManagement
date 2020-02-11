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
import { ActionTypes } from '../actions/case-instances';
import { CaseInstancesService } from '../services/caseinstances.service';
var CaseInstancesEffects = (function () {
    function CaseInstancesEffects(actions$, caseInstancesService) {
        var _this = this;
        this.actions$ = actions$;
        this.caseInstancesService = caseInstancesService;
        this.loadCaseInstances$ = this.actions$
            .pipe(ofType(ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.caseInstancesService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (casefiles) { return { type: ActionTypes.COMPLETE_SEARCH, content: casefiles }; }), catchError(function () { return of({ type: ActionTypes.COMPLETE_SEARCH }); }));
        }));
        this.loadCaseFileItems$ = this.actions$
            .pipe(ofType(ActionTypes.START_GET_FILE_ITEMS), mergeMap(function (evt) {
            return _this.caseInstancesService.getCaseFileItems(evt.id)
                .pipe(map(function (caseFileItems) { return { type: ActionTypes.COMPLETE_SEARCH, content: caseFileItems }; }), catchError(function () { return of({ type: ActionTypes.COMPLETE_GET_FILE_ITEMS }); }));
        }));
        this.loadCaseInstance$ = this.actions$
            .pipe(ofType(ActionTypes.START_GET), mergeMap(function (evt) {
            return _this.caseInstancesService.get(evt.id)
                .pipe(map(function (caseInstance) { return { type: ActionTypes.COMPLETE_GET, content: caseInstance }; }), catchError(function () { return of({ type: ActionTypes.COMPLETE_GET }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseInstancesEffects.prototype, "loadCaseInstances$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseInstancesEffects.prototype, "loadCaseFileItems$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseInstancesEffects.prototype, "loadCaseInstance$", void 0);
    CaseInstancesEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CaseInstancesService])
    ], CaseInstancesEffects);
    return CaseInstancesEffects;
}());
export { CaseInstancesEffects };
//# sourceMappingURL=case-instances.js.map