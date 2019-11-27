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
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { ActionTypes } from './case-def-actions';
var CaseDefEffects = (function () {
    function CaseDefEffects(actions$, caseDefinitionsService) {
        var _this = this;
        this.actions$ = actions$;
        this.caseDefinitionsService = caseDefinitionsService;
        this.loadCaseDef$ = this.actions$
            .pipe(ofType(ActionTypes.CASEDEFLOAD), mergeMap(function (evt) {
            return _this.caseDefinitionsService.get(evt.id)
                .pipe(map(function (casedef) { return { type: ActionTypes.CASEDEFLOADED, result: casedef }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEDEF }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseDefEffects.prototype, "loadCaseDef$", void 0);
    CaseDefEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CaseDefinitionsService])
    ], CaseDefEffects);
    return CaseDefEffects;
}());
export { CaseDefEffects };
//# sourceMappingURL=case-def-effects.js.map