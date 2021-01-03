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
import { forkJoin, of } from 'rxjs';
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
            return _this.tasksService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.owner, evt.status)
                .pipe(map(function (tasks) { return { type: ActionTypes.COMPLETE_SEARCH_TASKS, content: tasks }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SEARCH_TASKS }); }));
        }));
        this.startTask$ = this.actions$
            .pipe(ofType(ActionTypes.START_TASK), mergeMap(function (evt) {
            return _this.tasksService.start(evt.humanTaskInstanceId)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_START_TASK }; }), catchError(function () { return of({ type: ActionTypes.ERROR_START_TASK }); }));
        }));
        this.nominateTask$ = this.actions$
            .pipe(ofType(ActionTypes.NOMINATE_TASK), mergeMap(function (evt) {
            return _this.tasksService.nominate(evt.humanTaskInstanceId, evt.parameter)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_NOMINATE_TASK }; }), catchError(function () { return of({ type: ActionTypes.ERROR_NOMINATE_TASK }); }));
        }));
        this.claimTask$ = this.actions$
            .pipe(ofType(ActionTypes.CLAIM_TASK), mergeMap(function (evt) {
            return _this.tasksService.claim(evt.humanTaskInstanceId)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_CLAIM_TASK }; }), catchError(function () { return of({ type: ActionTypes.ERROR_CLAIM_TASK }); }));
        }));
        this.getTask$ = this.actions$
            .pipe(ofType(ActionTypes.GET_TASK), mergeMap(function (evt) {
            var renderingCall = _this.tasksService.getRendering(evt.humanTaskInstanceId);
            var detailsCall = _this.tasksService.getDetails(evt.humanTaskInstanceId);
            var descriptionCall = _this.tasksService.getDescription(evt.humanTaskInstanceId);
            var searchTaskHistoryCall = _this.tasksService.searchTaskHistory(evt.humanTaskInstanceId, 0, 200, evt.order, evt.direction);
            return forkJoin([renderingCall, detailsCall, descriptionCall, searchTaskHistoryCall]).pipe(map(function (results) { return { type: ActionTypes.COMPLETE_GET_TASK, renderingElts: results[0], task: results[1], description: results[2], searchTaskHistory: results[3] }; }), catchError(function () { return of({ type: ActionTypes.ERROR_GET_TASK }); }));
        }));
        this.completeTask$ = this.actions$
            .pipe(ofType(ActionTypes.SUBMIT_TASK), mergeMap(function (evt) {
            return _this.tasksService.completeTask(evt.humanTaskInstanceId, evt.operationParameters)
                .pipe(map(function () { return { type: ActionTypes.COMPLETE_SUBMIT_TASK }; }), catchError(function () { return of({ type: ActionTypes.ERROR_SUBMIT_TASK }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], TasksEffects.prototype, "searchCaseFiles$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], TasksEffects.prototype, "startTask$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], TasksEffects.prototype, "nominateTask$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], TasksEffects.prototype, "claimTask$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], TasksEffects.prototype, "getTask$", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], TasksEffects.prototype, "completeTask$", void 0);
    TasksEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            TasksService])
    ], TasksEffects);
    return TasksEffects;
}());
export { TasksEffects };
//# sourceMappingURL=tasks.effects.js.map