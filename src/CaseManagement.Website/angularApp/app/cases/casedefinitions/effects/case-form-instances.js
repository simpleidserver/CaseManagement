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
import { ActionTypes } from '../actions/case-form-instances';
import { CaseFormInstancesService } from '../services/caseforminstances.service';
var CaseFormInstancesEffects = (function () {
    function CaseFormInstancesEffects(actions$, caseFormInstancesService) {
        var _this = this;
        this.actions$ = actions$;
        this.caseFormInstancesService = caseFormInstancesService;
        this.loadCaseInstances$ = this.actions$
            .pipe(ofType(ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.caseFormInstancesService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (formInstances) { return { type: ActionTypes.COMPLETE_SEARCH, content: formInstances }; }), catchError(function () { return of({ type: ActionTypes.COMPLETE_SEARCH }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], CaseFormInstancesEffects.prototype, "loadCaseInstances$", void 0);
    CaseFormInstancesEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            CaseFormInstancesService])
    ], CaseFormInstancesEffects);
    return CaseFormInstancesEffects;
}());
export { CaseFormInstancesEffects };
//# sourceMappingURL=case-form-instances.js.map