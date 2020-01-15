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
import { Observable } from 'rxjs/Rx';
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { CaseFilesService } from '../services/casefiles.service';
import { CaseInstancesService } from '../services/caseinstances.service';
import { ActionTypes } from './view-actions';
import { CaseFormInstancesService } from '../services/caseforminstances.service';
import { CaseActivationsService } from '../services/caseactivations.service';
var ViewCaseDefinitionEffects = (function () {
    function ViewCaseDefinitionEffects(actions$, caseDefinitionsService, caseFileService, caseInstancesService, formInstancesService, caseActivationsService) {
        var _this = this;
        this.actions$ = actions$;
        this.caseDefinitionsService = caseDefinitionsService;
        this.caseFileService = caseFileService;
        this.caseInstancesService = caseInstancesService;
        this.formInstancesService = formInstancesService;
        this.caseActivationsService = caseActivationsService;
        this.loadCaseDefinition$ = this.actions$
            .pipe(ofType(ActionTypes.CASEDEFINITIONLOAD), mergeMap(function (evt) {
            return Observable.forkJoin([_this.caseDefinitionsService.get(evt.id), _this.caseDefinitionsService.getHistory(evt.id)])
                .pipe(mergeMap(function (responses) {
                return _this.caseFileService.get(responses[0].CaseFile).pipe(map(function (caseFile) { return { type: ActionTypes.CASEDEFINITIONLOADED, caseDefinition: responses[0], caseFile: caseFile, caseDefinitionHistory: responses[1] }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEDEFINITION }); }));
            }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEDEFINITION }); }));
        }));
        this.loadCaseInstances$ = this.actions$
            .pipe(ofType(ActionTypes.CASEINSTANCESLOAD), mergeMap(function (evt) {
            return _this.caseInstancesService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction).pipe(map(function (caseInstances) { return { type: ActionTypes.CASEINSTANCESLOADED, result: caseInstances }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEDEFINITION }); }));
        }));
        this.loadFormInstances$ = this.actions$
            .pipe(ofType(ActionTypes.CASEINSTANCESLOAD), mergeMap(function (evt) {
            return _this.formInstancesService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction).pipe(map(function (formInstances) { return { type: ActionTypes.CASEFORMINSTANCESLOADED, result: formInstances }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEFORMINSTANCES }); }));
        }));
        this.loadCaseActivations$ = this.actions$
            .pipe(ofType(ActionTypes.CASEACTIVATIONSLOAD), mergeMap(function (evt) {
            return _this.caseActivationsService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction).pipe(map(function (caseActivations) { return { type: ActionTypes.CASEACTIVATIONSLOADED, result: caseActivations }; }), catchError(function () { return of({ type: ActionTypes.ERRORLOADCASEACTIVATIONS }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], ViewCaseDefinitionEffects.prototype, "loadCaseDefinition$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], ViewCaseDefinitionEffects.prototype, "loadCaseInstances$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], ViewCaseDefinitionEffects.prototype, "loadFormInstances$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], ViewCaseDefinitionEffects.prototype, "loadCaseActivations$", void 0);
    ViewCaseDefinitionEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CaseDefinitionsService,
            CaseFilesService,
            CaseInstancesService,
            CaseFormInstancesService,
            CaseActivationsService])
    ], ViewCaseDefinitionEffects);
    return ViewCaseDefinitionEffects;
}());
export { ViewCaseDefinitionEffects };
//# sourceMappingURL=view-effects.js.map