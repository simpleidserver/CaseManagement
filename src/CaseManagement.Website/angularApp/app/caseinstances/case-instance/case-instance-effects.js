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
import { catchError, mergeMap, map } from 'rxjs/operators';
import { ActionTypes } from './case-instance-actions';
import { CaseDefinitionsService } from '../../casedefinitions/services/casedefinitions.service';
import { CaseInstancesService } from '../../casedefinitions/services/caseinstances.service';
import { of } from 'rxjs';
var CaseInstanceEffects = (function () {
    function CaseInstanceEffects(actions$, caseDefinitionsService, caseInstancesService) {
        var _this = this;
        this.actions$ = actions$;
        this.caseDefinitionsService = caseDefinitionsService;
        this.caseInstancesService = caseInstancesService;
        this.loadCaseDef$ = this.actions$
            .pipe(ofType(ActionTypes.CASEINSTANCELOAD), mergeMap(function (evt) {
            return _this.caseInstancesService.get(evt.id)
                .pipe(mergeMap(function (caseInstance) {
                return _this.caseDefinitionsService.get(caseInstance.TemplateId).pipe(map(function (caseDefinition) { return { type: ActionTypes.CASEINSTANCELOADED, caseInstance: caseInstance, caseDefinition: caseDefinition }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEINSTANCE }); }));
            }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEINSTANCE }); }));
        }));
        this.loadExecutionSteps = this.actions$
            .pipe(ofType(ActionTypes.CASEEXECUTIONSSTEPSLOAD), mergeMap(function (evt) {
            return _this.caseInstancesService.searchExecutionSteps(evt.startIndex, evt.count, evt.id, evt.order, evt.direction)
                .pipe(map(function (r) { return { type: ActionTypes.CASEEXECUTIONSTEPSLOADED, result: r }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEEXECUTIONSTEPS }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseInstanceEffects.prototype, "loadCaseDef$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseInstanceEffects.prototype, "loadExecutionSteps", void 0);
    CaseInstanceEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CaseDefinitionsService,
            CaseInstancesService])
    ], CaseInstanceEffects);
    return CaseInstanceEffects;
}());
export { CaseInstanceEffects };
//# sourceMappingURL=case-instance-effects.js.map