import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromCasePlan from '../actions/caseplan.actions';
import { CasePlanService } from '../services/caseplan.service';

@Injectable()
export class CasePlanEffects {
    constructor(
        private actions$: Actions,
        private casePlanService: CasePlanService
    ) { }

    @Effect()
    searchCasePlans$ = this.actions$
        .pipe(
            ofType(fromCasePlan.ActionTypes.START_SEARCH),
            mergeMap((evt: fromCasePlan.StartSearch) => {
                return this.casePlanService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text, evt.caseFileId)
                    .pipe(
                        map(casePlans => { return { type: fromCasePlan.ActionTypes.COMPLETE_SEARCH, content: casePlans }; }),
                        catchError(() => of({ type: fromCasePlan.ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );

    @Effect()
    getCasePlan$ = this.actions$
        .pipe(
            ofType(fromCasePlan.ActionTypes.START_GET),
            mergeMap((evt: fromCasePlan.StartGet) => {
                return this.casePlanService.get(evt.id)
                    .pipe(
                        map(casefiles => { return { type: fromCasePlan.ActionTypes.COMPLETE_GET, content: casefiles }; }),
                        catchError(() => of({ type: fromCasePlan.ActionTypes.COMPLETE_GET }))
                    );
            }
            )
    );

    @Effect()
    searchCasePlanHistory = this.actions$
        .pipe(
            ofType(fromCasePlan.ActionTypes.START_SEARCH_HISTORY),
            mergeMap((evt: fromCasePlan.StartSearchHistory) => {
                return this.casePlanService.searchHistory(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(caseInstances => { return { type: fromCasePlan.ActionTypes.COMPLETE_SEARCH_HISTORY, content: caseInstances }; }),
                        catchError(() => of({ type: fromCasePlan.ActionTypes.COMPLETE_SEARCH_HISTORY }))
                    );
            }
            )
        );
}