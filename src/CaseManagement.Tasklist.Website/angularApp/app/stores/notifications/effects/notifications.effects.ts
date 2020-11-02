import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, SearchNotifications } from '../actions/notifications.actions';
import { NotificationsService } from '../services/notifications.service';

@Injectable()
export class NotificationsEffects {
    constructor(
        private actions$: Actions,
        private notificationsService: NotificationsService
    ) { }

    @Effect()
    searchNotifications$ = this.actions$
        .pipe(
            ofType(ActionTypes.SEARCH_NOTIFICATIONS),
            mergeMap((evt: SearchNotifications) => {
                return this.notificationsService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(tasks => { return { type: ActionTypes.COMPLETE_SEARCH_NOTIFICATIONS, content: tasks }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_SEARCH_NOTIFICATIONS }))
                    );
            }
            )
    );
}