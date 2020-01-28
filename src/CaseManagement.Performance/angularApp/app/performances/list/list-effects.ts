import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { StatisticService } from '../services/statistic.service';
import { ActionTypes } from './list-actions';

@Injectable()
export class ListPerformancesEffects {
    constructor(
        private actions$: Actions,
        private statisticService: StatisticService
    ) { }

    @Effect()
    loadPerformances$ = this.actions$
        .pipe(
            ofType(ActionTypes.PERFORMANCESLOAD),
            mergeMap((evt: any) => {
                return this.statisticService.searchPerformances(evt.startIndex, evt.count, evt.order, evt.direction, evt.startDateTime)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.PERFORMANCESLOADED, result: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADPERFORMANCES }))
                    );
                }
            )
        );
}