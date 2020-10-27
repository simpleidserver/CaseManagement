import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, SearchTasks } from '../actions/tasks.actions';
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
}