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
import { ActionTypes } from '../actions/bpmn-files.actions';
import { BpmnFilesService } from '../services/bpmnfiles.service';
var BpmnFilesEffects = (function () {
    function BpmnFilesEffects(actions$, bpmnFilesService) {
        var _this = this;
        this.actions$ = actions$;
        this.bpmnFilesService = bpmnFilesService;
        this.searchBpmnFiles$ = this.actions$
            .pipe(ofType(ActionTypes.START_SEARCH_BPMNFILES), mergeMap(function (evt) {
            return _this.bpmnFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.takeLatest, evt.fileId)
                .pipe(map(function (bpmnFiles) { return { type: ActionTypes.COMPLETE_SEARCH_BPMNFILES, content: bpmnFiles }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_BPMNFILES }); }));
        }));
        this.getBpmnFile$ = this.actions$
            .pipe(ofType(ActionTypes.START_GET_BPMNFILE), mergeMap(function (evt) {
            return _this.bpmnFilesService.get(evt.id)
                .pipe(map(function (bpmnFile) { return { type: ActionTypes.COMPLETE_GET_BPMNFILE, bpmnFile: bpmnFile }; }), catchError(function () { return of({ type: ActionTypes.ERROR_GET_BPMNFILE }); }));
        }));
        this.updateBpmnFile$ = this.actions$
            .pipe(ofType(ActionTypes.UPDATE_BPMNFILE), mergeMap(function (evt) {
            return _this.bpmnFilesService.update(evt.id, evt.name, evt.description)
                .pipe(mergeMap(function () {
                return _this.bpmnFilesService.updatePayload(evt.id, evt.payload)
                    .pipe(map(function () { return { type: ActionTypes.COMPLETE_UPDATE_BPMNFILE, id: evt.id, name: evt.name, description: evt.description, payload: evt.payload }; }), catchError(function () { return of({ type: ActionTypes.ERROR_UPDATE_BPMNFILE }); }));
            }), catchError(function () { return of({ type: ActionTypes.ERROR_UPDATE_BPMNFILE }); }));
        }));
        this.updateBpmnFilePayload$ = this.actions$
            .pipe(ofType(ActionTypes.UPDATE_BPMNFILE_PAYLOAD), mergeMap(function (evt) {
            return _this.bpmnFilesService.updatePayload(evt.id, evt.payload)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD, id: evt.id, payload: evt.payload }; }), catchError(function () { return of({ type: ActionTypes.ERROR_UPDATE_BPMNFILE_PAYLOAD }); }));
        }));
        this.publishBpmnFile$ = this.actions$
            .pipe(ofType(ActionTypes.PUBLISH_BPMNFILE), mergeMap(function (evt) {
            return _this.bpmnFilesService.publish(evt.id)
                .pipe(map(function (str) { return { type: ActionTypes.COMPLETE_PUBLISH_BPMNFILE, id: str }; }), catchError(function () { return of({ type: ActionTypes.ERROR_PUBLISH_BPMNFILE }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], BpmnFilesEffects.prototype, "searchBpmnFiles$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], BpmnFilesEffects.prototype, "getBpmnFile$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], BpmnFilesEffects.prototype, "updateBpmnFile$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], BpmnFilesEffects.prototype, "updateBpmnFilePayload$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], BpmnFilesEffects.prototype, "publishBpmnFile$", void 0);
    BpmnFilesEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            BpmnFilesService])
    ], BpmnFilesEffects);
    return BpmnFilesEffects;
}());
export { BpmnFilesEffects };
//# sourceMappingURL=bpmn-files.effects.js.map