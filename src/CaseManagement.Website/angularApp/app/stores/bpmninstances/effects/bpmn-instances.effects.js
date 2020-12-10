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
import { ActionTypes } from '../actions/bpmn-instances.actions';
import { BpmnInstancesService } from '../services/bpmninstances.service';
var BpmnInstancesEffects = (function () {
    function BpmnInstancesEffects(actions$, bpmnInstancesService) {
        var _this = this;
        this.actions$ = actions$;
        this.bpmnInstancesService = bpmnInstancesService;
        this.getBpmnInstance$ = this.actions$
            .pipe(ofType(ActionTypes.GET_BPMNINSTANCE), mergeMap(function (evt) {
            return _this.bpmnInstancesService.get(evt.id)
                .pipe(map(function (content) { return { type: ActionTypes.COMPLETE_GET_BPMNINSTANCE, content: content }; }), catchError(function () { return of({ type: ActionTypes.ERROR_GET_BPMNINSTANCE }); }));
        }));
        this.createBpmnInstance$ = this.actions$
            .pipe(ofType(ActionTypes.CREATE_BPMNINSTANCE), mergeMap(function (evt) {
            return _this.bpmnInstancesService.create(evt.processFileId)
                .pipe(map(function (content) { return { type: ActionTypes.COMPLETE_CREATE_BPMN_INSTANCE, content: content }; }), catchError(function () { return of({ type: ActionTypes.ERROR_CREATE_BPMNINSTANCE }); }));
        }));
        this.startBpmnInstance$ = this.actions$
            .pipe(ofType(ActionTypes.START_BPMNINSTANCE), mergeMap(function (evt) {
            return _this.bpmnInstancesService.start(evt.id)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_START_BPMNINSTANCE }; }), catchError(function () { return of({ type: ActionTypes.ERROR_START_BPMNINSTANCE }); }));
        }));
        this.searchBpmnInstances$ = this.actions$
            .pipe(ofType(ActionTypes.SEARCH_BPMNINSTANCES), mergeMap(function (evt) {
            return _this.bpmnInstancesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.processFileId)
                .pipe(map(function (content) { return { type: ActionTypes.COMPLETE_SEARCH_BPMNINSTANCES, content: content }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_BPMNINSTANCES }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], BpmnInstancesEffects.prototype, "getBpmnInstance$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], BpmnInstancesEffects.prototype, "createBpmnInstance$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], BpmnInstancesEffects.prototype, "startBpmnInstance$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], BpmnInstancesEffects.prototype, "searchBpmnInstances$", void 0);
    BpmnInstancesEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            BpmnInstancesService])
    ], BpmnInstancesEffects);
    return BpmnInstancesEffects;
}());
export { BpmnInstancesEffects };
//# sourceMappingURL=bpmn-instances.effects.js.map