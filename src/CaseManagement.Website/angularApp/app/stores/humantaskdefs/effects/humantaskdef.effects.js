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
import * as fromHumanTask from '../actions/humantaskdef.actions';
import { HumanTaskDefService } from '../services/humantaskdef.service';
var HumanTaskDefEffects = (function () {
    function HumanTaskDefEffects(actions$, humanTaskDefService) {
        var _this = this;
        this.actions$ = actions$;
        this.humanTaskDefService = humanTaskDefService;
        this.getHumanTaskDef = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.START_GET_HUMANTASKDEF), mergeMap(function (evt) {
            return _this.humanTaskDefService.get(evt.id)
                .pipe(map(function (humanTaskDef) { return { type: fromHumanTask.ActionTypes.COMPLETE_GET_HUMANTASKDEF, content: humanTaskDef }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_GET_HUMANTASKDEF }); }));
        }));
        this.updateHumanTaskDef = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_HUMANASKDEF), mergeMap(function (evt) {
            return _this.humanTaskDefService.update(evt.humanTaskDef)
                .pipe(map(function (humanTaskDef) { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_HUMANASKDEF, content: humanTaskDef }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_HUMANASKDEF }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "getHumanTaskDef", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updateHumanTaskDef", void 0);
    HumanTaskDefEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            HumanTaskDefService])
    ], HumanTaskDefEffects);
    return HumanTaskDefEffects;
}());
export { HumanTaskDefEffects };
//# sourceMappingURL=humantaskdef.effects.js.map