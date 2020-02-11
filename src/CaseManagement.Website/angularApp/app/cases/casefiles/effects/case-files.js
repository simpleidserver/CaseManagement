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
import { ActionTypes } from '../actions/case-files';
import { CaseFilesService } from '../services/casefiles.service';
var CaseFilesEffects = (function () {
    function CaseFilesEffects(actions$, caseFilesService) {
        var _this = this;
        this.actions$ = actions$;
        this.caseFilesService = caseFilesService;
        this.loadCaseFiles$ = this.actions$
            .pipe(ofType(ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.caseFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text, evt.user)
                .pipe(map(function (casefiles) { return { type: ActionTypes.COMPLETE_SEARCH, content: casefiles }; }), catchError(function () { return of({ type: ActionTypes.COMPLETE_SEARCH }); }));
        }));
        this.loadCaseFile$ = this.actions$
            .pipe(ofType(ActionTypes.START_GET), mergeMap(function (evt) {
            return _this.caseFilesService.get(evt.id)
                .pipe(map(function (casefiles) { return { type: ActionTypes.COMPLETE_GET, content: casefiles }; }), catchError(function () { return of({ type: ActionTypes.COMPLETE_GET }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseFilesEffects.prototype, "loadCaseFiles$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseFilesEffects.prototype, "loadCaseFile$", void 0);
    CaseFilesEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CaseFilesService])
    ], CaseFilesEffects);
    return CaseFilesEffects;
}());
export { CaseFilesEffects };
//# sourceMappingURL=case-files.js.map