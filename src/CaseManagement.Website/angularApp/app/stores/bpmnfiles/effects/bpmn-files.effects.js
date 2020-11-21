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
import { ActionTypes } from '../actions/case-files.actions';
import { CaseFilesService } from '../services/casefiles.service';
var CaseFilesEffects = (function () {
    function CaseFilesEffects(actions$, caseFilesService) {
        var _this = this;
        this.actions$ = actions$;
        this.caseFilesService = caseFilesService;
        this.searchCaseFiles$ = this.actions$
            .pipe(ofType(ActionTypes.START_SEARCH_CASEFILES), mergeMap(function (evt) {
            return _this.caseFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text, null, true)
                .pipe(map(function (casefiles) { return { type: ActionTypes.COMPLETE_SEARCH_CASEFILES, content: casefiles }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_CASEFILES }); }));
        }));
        this.searchCaseFileHistories$ = this.actions$
            .pipe(ofType(ActionTypes.START_SEARCH_CASEFILES_HISTORY), mergeMap(function (evt) {
            return _this.caseFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, null, evt.caseFileId, false)
                .pipe(map(function (casefiles) { return { type: ActionTypes.COMPLETE_SEARCH_CASEFILES_HISTORY, content: casefiles }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_CASEFILES_HISTORY }); }));
        }));
        this.getCaseFile$ = this.actions$
            .pipe(ofType(ActionTypes.START_GET_CASEFILE), mergeMap(function (evt) {
            return _this.caseFilesService.get(evt.id)
                .pipe(map(function (casefile) { return { type: ActionTypes.COMPLETE_GET_CASEFILE, content: casefile }; }), catchError(function () { return of({ type: ActionTypes.ERROR_GET_CASEFILE }); }));
        }));
        this.addCaseFile$ = this.actions$
            .pipe(ofType(ActionTypes.ADD_CASEFILE), mergeMap(function (evt) {
            return _this.caseFilesService.add(evt.name, evt.description)
                .pipe(map(function (str) { return { type: ActionTypes.COMPLETE_ADD_CASEFILE, id: str }; }), catchError(function () { return of({ type: ActionTypes.ERROR_ADD_CASEFILE }); }));
        }));
        this.publishCaseFile$ = this.actions$
            .pipe(ofType(ActionTypes.PUBLISH_CASEFILE), mergeMap(function (evt) {
            return _this.caseFilesService.publish(evt.id)
                .pipe(map(function (str) { return { type: ActionTypes.COMPLETE_PUBLISH_CASEFILE, id: str }; }), catchError(function () { return of({ type: ActionTypes.ERROR_PUBLISH_CASEFILE }); }));
        }));
        this.updateCaseFile$ = this.actions$
            .pipe(ofType(ActionTypes.UPDATE_CASEFILE), mergeMap(function (evt) {
            return _this.caseFilesService.update(evt.id, evt.name, evt.description, evt.payload)
                .pipe(map(function (str) { return { type: ActionTypes.COMPLETE_UPDATE_CASEFILE, id: str }; }), catchError(function () { return of({ type: ActionTypes.ERROR_UPDATE_CASEFILE }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseFilesEffects.prototype, "searchCaseFiles$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseFilesEffects.prototype, "searchCaseFileHistories$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseFilesEffects.prototype, "getCaseFile$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseFilesEffects.prototype, "addCaseFile$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseFilesEffects.prototype, "publishCaseFile$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseFilesEffects.prototype, "updateCaseFile$", void 0);
    CaseFilesEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CaseFilesService])
    ], CaseFilesEffects);
    return CaseFilesEffects;
}());
export { CaseFilesEffects };
//# sourceMappingURL=case-files.effects.js.map