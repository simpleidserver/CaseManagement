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
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes } from '../actions/cmmn-files.actions';
import { CmmnFilesService } from '../services/cmmnfiles.service';
import { HumanTaskDefService } from '@app/stores/humantaskdefs/services/humantaskdef.service';
import { of, forkJoin } from 'rxjs';
var CmmnFilesEffects = (function () {
    function CmmnFilesEffects(actions$, cmmnFilesService, humanTaskDefService) {
        var _this = this;
        this.actions$ = actions$;
        this.cmmnFilesService = cmmnFilesService;
        this.humanTaskDefService = humanTaskDefService;
        this.searchCmmnFiles$ = this.actions$
            .pipe(ofType(ActionTypes.START_SEARCH_CMMNFILES), mergeMap(function (evt) {
            return _this.cmmnFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text, evt.caseFileId, evt.takeLatest)
                .pipe(map(function (cmmnfiles) { return { type: ActionTypes.COMPLETE_SEARCH_CMMNFILES, content: cmmnfiles }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_CMMNFILES }); }));
        }));
        this.getCmmnFile$ = this.actions$
            .pipe(ofType(ActionTypes.START_GET_CMMNFILE), mergeMap(function (evt) {
            var getCmmnFileCall = _this.cmmnFilesService.get(evt.id);
            var getHumanTaskDefsCall = _this.humanTaskDefService.getAll();
            return forkJoin([getCmmnFileCall, getHumanTaskDefsCall]).pipe(map(function (results) { return { type: ActionTypes.COMPLETE_GET_CMMNFILE, humanTaskDefs: results[1], content: results[0] }; }), catchError(function () { return of({ type: ActionTypes.ERROR_GET_CMMNFILE }); }));
        }));
        this.addCmmnFile$ = this.actions$
            .pipe(ofType(ActionTypes.ADD_CMMNFILE), mergeMap(function (evt) {
            return _this.cmmnFilesService.add(evt.name, evt.description)
                .pipe(map(function (str) { return { type: ActionTypes.COMPLETE_ADD_CMMNFILE, id: str }; }), catchError(function () { return of({ type: ActionTypes.ERROR_ADD_CMMNFILE }); }));
        }));
        this.publishCmmnFile$ = this.actions$
            .pipe(ofType(ActionTypes.PUBLISH_CMMNFILE), mergeMap(function (evt) {
            return _this.cmmnFilesService.publish(evt.id)
                .pipe(map(function (str) { return { type: ActionTypes.COMPLETE_PUBLISH_CMMNFILE, id: str }; }), catchError(function () { return of({ type: ActionTypes.ERROR_PUBLISH_CMMNFILE }); }));
        }));
        this.updateCmmnFile$ = this.actions$
            .pipe(ofType(ActionTypes.UPDATE_CMMNFILE), mergeMap(function (evt) {
            return _this.cmmnFilesService.update(evt.id, evt.name, evt.description)
                .pipe(mergeMap(function () {
                return _this.cmmnFilesService.updatePayload(evt.id, evt.xml)
                    .pipe(map(function () { return { type: ActionTypes.COMPLETE_UPDATE_CMMNFILE, id: evt.id, name: evt.name, description: evt.description, xml: evt.xml }; }), catchError(function () { return of({ type: ActionTypes.ERROR_UPDATE_CMMNFILE }); }));
            }), catchError(function () { return of({ type: ActionTypes.ERROR_UPDATE_CMMNFILE }); }));
        }));
        this.updateCmmnFilePayload$ = this.actions$
            .pipe(ofType(ActionTypes.UPDATE_CMMNFILE_PAYLOAD), mergeMap(function (evt) {
            return _this.cmmnFilesService.updatePayload(evt.id, evt.payload)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_UPDATE_CMMNFILE_PAYLOAD, payload: evt.payload }; }), catchError(function () { return of({ type: ActionTypes.ERROR_UPDATE_CMMNFILE_PAYLOAD }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnFilesEffects.prototype, "searchCmmnFiles$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnFilesEffects.prototype, "getCmmnFile$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnFilesEffects.prototype, "addCmmnFile$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnFilesEffects.prototype, "publishCmmnFile$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnFilesEffects.prototype, "updateCmmnFile$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CmmnFilesEffects.prototype, "updateCmmnFilePayload$", void 0);
    CmmnFilesEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CmmnFilesService,
            HumanTaskDefService])
    ], CmmnFilesEffects);
    return CmmnFilesEffects;
}());
export { CmmnFilesEffects };
//# sourceMappingURL=cmmn-files.effects.js.map