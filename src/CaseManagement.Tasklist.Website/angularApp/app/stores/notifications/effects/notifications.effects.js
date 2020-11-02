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
import { ActionTypes } from '../actions/tasks.actions';
import { TasksService } from '../services/tasks.service';
var TasksEffects = (function () {
    function TasksEffects(actions$, tasksService) {
        var _this = this;
        this.actions$ = actions$;
        this.tasksService = tasksService;
        this.searchCaseFiles$ = this.actions$
            .pipe(ofType(ActionTypes.SEARCH_TASKS), mergeMap(function (evt) {
            return _this.tasksService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (tasks) { return { type: ActionTypes.COMPLETE_SEARCH_TASKS, content: tasks }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_TASKS }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], TasksEffects.prototype, "searchCaseFiles$", void 0);
    TasksEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            TasksService])
    ], TasksEffects);
    return TasksEffects;
}());
export { TasksEffects };
//# sourceMappingURL=tasks.effects.js.map