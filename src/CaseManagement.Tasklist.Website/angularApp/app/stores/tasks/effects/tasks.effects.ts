import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, SearchTasks, StartTask, NominateTask, ClaimTask } from '../actions/tasks.actions';
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

}