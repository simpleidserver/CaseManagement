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
import { ActionTypes } from '../actions/notifications.actions';
import { NotificationsService } from '../services/notifications.service';
var NotificationsEffects = (function () {
    function NotificationsEffects(actions$, notificationsService) {
        var _this = this;
        this.actions$ = actions$;
        this.notificationsService = notificationsService;
        this.searchNotifications$ = this.actions$
            .pipe(ofType(ActionTypes.SEARCH_NOTIFICATIONS), mergeMap(function (evt) {
            return _this.notificationsService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (tasks) { return { type: ActionTypes.COMPLETE_SEARCH_NOTIFICATIONS, content: tasks }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_NOTIFICATIONS }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], NotificationsEffects.prototype, "searchNotifications$", void 0);
    NotificationsEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            NotificationsService])
    ], NotificationsEffects);
    return NotificationsEffects;
}());
export { NotificationsEffects };
//# sourceMappingURL=notifications.effects.js.map