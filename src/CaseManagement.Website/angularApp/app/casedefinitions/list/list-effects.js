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
import { ActionTypes } from './list-actions';
var ListCaseDefinitionsEffects = (function () {
    function ListCaseDefinitionsEffects(actions$, caseDefinitionsService) {
        var _this = this;
        this.actions$ = actions$;
        this.caseDefinitionsService = caseDefinitionsService;
        this.loadCaseFiles$ = this.actions$
            .pipe(ofType(ActionTypes.CASEDEFINITIONSLOAD), mergeMap(function (evt) {
            return _this.caseDefinitionsService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text)
                .pipe(map(function (casefiles) { return { type: ActionTypes.CASEDEFINITIONSLOADED, result: casefiles }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEDEFINITIONS }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], ListCaseDefinitionsEffects.prototype, "loadCaseFiles$", void 0);
    ListCaseDefinitionsEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CaseDefinitionsService])
    ], ListCaseDefinitionsEffects);
    return ListCaseDefinitionsEffects;
}());
export { ListCaseDefinitionsEffects };
//# sourceMappingURL=list-effects.js.map