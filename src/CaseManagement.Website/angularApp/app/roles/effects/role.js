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
import * as fromRole from '../actions/role';
import { RolesService } from '../services/role.service';
var RoleEffects = (function () {
    function RoleEffects(actions$, roleService) {
        var _this = this;
        this.actions$ = actions$;
        this.roleService = roleService;
        this.searchRoles$ = this.actions$
            .pipe(ofType(fromRole.ActionTypes.START_SEARCH), mergeMap(function (evt) {
            return _this.roleService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (casePlans) { return { type: fromRole.ActionTypes.COMPLETE_SEARCH, content: casePlans }; }), catchError(function () { return of({ type: fromRole.ActionTypes.COMPLETE_SEARCH }); }));
        }));
        this.loadRole$ = this.actions$
            .pipe(ofType(fromRole.ActionTypes.START_GET), mergeMap(function (evt) {
            return _this.roleService.get(evt.role)
                .pipe(map(function (role) { return { type: fromRole.ActionTypes.COMPLETE_GET, content: role }; }), catchError(function () { return of({ type: fromRole.ActionTypes.COMPLETE_GET }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], RoleEffects.prototype, "searchRoles$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], RoleEffects.prototype, "loadRole$", void 0);
    RoleEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            RolesService])
    ], RoleEffects);
    return RoleEffects;
}());
export { RoleEffects };
//# sourceMappingURL=role.js.map