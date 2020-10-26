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
import * as fromHumanTask from '../actions/humantaskinst.actions';
import { HumanTaskInstService } from '../services/humantaskinst.service';
var HumanTaskInstEffects = (function () {
    function HumanTaskInstEffects(actions$, humanTaskInstService) {
        var _this = this;
        this.actions$ = actions$;
        this.humanTaskInstService = humanTaskInstService;
        this.createHumanTaskInstance = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.CREATE_HUMANTASKINSTANCE), mergeMap(function (evt) {
            return _this.humanTaskInstService.create(evt.cmd)
                .pipe(map(function (id) { return { type: fromHumanTask.ActionTypes.COMPLETE_CREATE_HUMANTASKINSTANCE, content: id }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_CREATE_HUMANTASKINSTANCE }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskInstEffects.prototype, "createHumanTaskInstance", void 0);
    HumanTaskInstEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            HumanTaskInstService])
    ], HumanTaskInstEffects);
    return HumanTaskInstEffects;
}());
export { HumanTaskInstEffects };
//# sourceMappingURL=humantaskinst.effects.js.map