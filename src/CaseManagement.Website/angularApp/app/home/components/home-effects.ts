import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { StatisticService } from '../services/statistic.service';
import { ActionTypes } from './home-actions';

function getFirstDayOfMonth() {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    return getDate(new Date(y, m, 1));
}

function getCurrentMonday() {
    let d = new Date();
    var day = d.getDay(),
        diff = d.getDate() - day + (day == 0 ? -6 : 1);
    return getDate(new Date(d.setDate(diff)));
}

function getDate(d : Date) {
    return d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
}

@Injectable()
export class HomeEffects {
    constructor(
        private actions$: Actions,
        private statisticService: StatisticService
    ) { }

    @Effect()
    loadStatistic = this.actions$
        .pipe(
            ofType(ActionTypes.STATISTICLOAD),
            mergeMap(() => {
                return this.statisticService.get()
                    .pipe(
                        map(statistic => { return { type: ActionTypes.STATISTICLOADED, result: statistic }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADSTATISTIC }))
                    );
                }
            )
    );

    @Effect()
    searchWeekStatistics = this.actions$
        .pipe(
            ofType(ActionTypes.SEARCHWEEKSTATISTICS),
            mergeMap((evt: any) => {
                var date = getCurrentMonday();
                return this.statisticService.search(evt.startIndex, evt.count, evt.order, evt.direction, date, null)
                    .pipe(
                        map(statistic => { return { type: ActionTypes.WEEKSTATISTICSLOADED, result: statistic }; }),
                        catchError(() => of({ type: ActionTypes.ERRORWEEKSTATISTICS }))
                    );
            }
            )
    );

    @Effect()
    searchMonthStatistics = this.actions$
        .pipe(
            ofType(ActionTypes.SEARCHMONTHSTATISTICS),
            mergeMap((evt: any) => {
                var date = getFirstDayOfMonth();
                return this.statisticService.search(evt.startIndex, evt.count, evt.order, evt.direction, date, null)
                    .pipe(
                        map(statistic => { return { type: ActionTypes.MONTHSTATISTICSLOADED, result: statistic }; }),
                        catchError(() => of({ type: ActionTypes.ERRORMONTHSTATISTICS }))
                    );
            }
            )
        );
}