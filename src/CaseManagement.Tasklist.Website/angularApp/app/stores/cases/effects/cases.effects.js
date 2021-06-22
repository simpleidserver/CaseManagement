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
import { ActionTypes } from '../actions/cases.actions';
import { CasesService } from '../services/cases.service';
var CasesEffects = (function () {
    function CasesEffects(actions$, casesService) {
        var _this = this;
        this.actions$ = actions$;
        this.casesService = casesService;
        this.searchCaseInstances$ = this.actions$
            .pipe(ofType(ActionTypes.SEARCH_CASES), mergeMap(function (evt) {
            return _this.casesService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (cases) { return { type: ActionTypes.COMPLETE_SEARCH_CASES, content: cases }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_CASES }); }));
        }));
        this.getCase$ = this.actions$
            .pipe(ofType(ActionTypes.GET_CASE), mergeMap(function (evt) {
            return _this.casesService.get(evt.id)
                .pipe(map(function (cs) { return { type: ActionTypes.COMPLETE_GET_CASE, content: cs }; }), catchError(function () { return of({ type: ActionTypes.ERROR_GET_CASE }); }));
        }));
        this.activate$ = this.actions$
            .pipe(ofType(ActionTypes.ACTIVATE), mergeMap(function (evt) {
            return _this.casesService.activate(evt.id, evt.elt)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_ACTIVATE }; }), catchError(function () { return of({ type: ActionTypes.ERROR_ACTIVATE }); }));
        }));
        this.disable$ = this.actions$
            .pipe(ofType(ActionTypes.DISABLE), mergeMap(function (evt) {
            return _this.casesService.disable(evt.id, evt.elt)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_DISABLE }; }), catchError(function () { return of({ type: ActionTypes.ERROR_DISABLE }); }));
        }));
        this.reenable$ = this.actions$
            .pipe(ofType(ActionTypes.REENABLE), mergeMap(function (evt) {
            return _this.casesService.reenable(evt.id, evt.elt)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_REENABLE }; }), catchError(function () { return of({ type: ActionTypes.ERROR_REENABLE }); }));
        }));
        this.complete$ = this.actions$
            .pipe(ofType(ActionTypes.COMPLETE), mergeMap(function (evt) {
            return _this.casesService.complete(evt.id, evt.elt)
                .pipe(map(function () { return { type: ActionTypes.COMPLETED }; }), catchError(function () { return of({ type: ActionTypes.ERROR_COMPLETE }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasesEffects.prototype, "searchCaseInstances$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasesEffects.prototype, "getCase$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasesEffects.prototype, "activate$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasesEffects.prototype, "disable$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasesEffects.prototype, "reenable$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CasesEffects.prototype, "complete$", void 0);
    CasesEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CasesService])
    ], CasesEffects);
    return CasesEffects;
}());
export { CasesEffects };
//# sourceMappingURL=cases.effects.js.map