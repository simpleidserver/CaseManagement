import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { forkJoin, of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, ClaimTask, NominateTask, RenderingTask, SearchTasks, StartTask, SubmitTask } from '../actions/tasks.actions';
import { TasksService } from '../services/tasks.service';

@Injectable()
export class TasksEffects {
    constructor(
        private actions$: Actions,
        private tasksService: TasksService
    ) { }

    @Effect()
    searchCaseFiles$ = this.actions$
        .pipe(
            ofType(ActionTypes.SEARCH_TASKS),
            mergeMap((evt: SearchTasks) => {
                return this.tasksService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.owner, evt.status)
                    .pipe(
                        map(tasks => { return { type: ActionTypes.COMPLETE_SEARCH_TASKS, content: tasks }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_SEARCH_TASKS }))
                    );
            }
            )
    );

    @Effect()
    startTask$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_TASK),
            mergeMap((evt: StartTask) => {
                return this.tasksService.start(evt.humanTaskInstanceId)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_START_TASK }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_START_TASK }))
                    );
            }
            )
    );

    @Effect()
    nominateTask$ = this.actions$
        .pipe(
            ofType(ActionTypes.NOMINATE_TASK),
            mergeMap((evt: NominateTask) => {
                return this.tasksService.nominate(evt.humanTaskInstanceId, evt.parameter)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_NOMINATE_TASK }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_NOMINATE_TASK }))
                    );
            }
            )
    );

    @Effect()
    claimTask$ = this.actions$
        .pipe(
            ofType(ActionTypes.CLAIM_TASK),
            mergeMap((evt: ClaimTask) => {
                return this.tasksService.claim(evt.humanTaskInstanceId)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_CLAIM_TASK }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_CLAIM_TASK }))
                    );
            }
            )
        );

    @Effect()
    getTask$= this.actions$
        .pipe(
            ofType(ActionTypes.GET_TASK),
            mergeMap((evt: RenderingTask) => {
                let renderingCall = this.tasksService.getRendering(evt.humanTaskInstanceId);
                let detailsCall = this.tasksService.getDetails(evt.humanTaskInstanceId);
                let descriptionCall = this.tasksService.getDescription(evt.humanTaskInstanceId);
                let searchTaskHistoryCall = this.tasksService.searchTaskHistory(evt.humanTaskInstanceId, 0, 200, evt.order, evt.direction);
                return forkJoin([renderingCall, detailsCall, descriptionCall, searchTaskHistoryCall]).pipe(
                    map((results) => { return { type: ActionTypes.COMPLETE_GET_TASK, rendering: results[0], task: results[1], description: results[2], searchTaskHistory: results[3] }; }),
                    catchError(() => of({ type: ActionTypes.ERROR_GET_TASK }))
                )
            }
            )
    );

    @Effect()
    completeTask$ = this.actions$
        .pipe(
            ofType(ActionTypes.SUBMIT_TASK),
            mergeMap((evt: SubmitTask) => {
                return this.tasksService.completeTask(evt.humanTaskInstanceId, evt.operationParameters)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_SUBMIT_TASK }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_SUBMIT_TASK }))
                    );
            }
            )
        );
}