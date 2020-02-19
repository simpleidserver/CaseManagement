import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromCaseInstance from '../actions/caseinstance';
import * as fromCasePlan from '../actions/caseplan';
import * as fromCaseWorker from '../actions/caseworker';
import * as fromFormInstance from '../actions/forminstance';
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
                return this.casePlanService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text)
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

    @Effect()
    searchCaseInstance$ = this.actions$
        .pipe(
            ofType(fromCaseInstance.ActionTypes.START_SEARCH),
            mergeMap((evt: fromCaseInstance.StartSearch) => {
                return this.casePlanService.searchCasePlanInstance(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(caseInstances => { return { type: fromCaseInstance.ActionTypes.COMPLETE_SEARCH, content: caseInstances }; }),
                        catchError(() => of({ type: fromCaseInstance.ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );

    @Effect()
    searchWorkerTask$ = this.actions$
        .pipe(
            ofType(fromCaseWorker.ActionTypes.START_SEARCH),
            mergeMap((evt: fromCaseWorker.StartSearch) => {
                return this.casePlanService.searchWorkerTask(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(workerTask => { return { type: fromCaseWorker.ActionTypes.COMPLETE_SEARCH, content: workerTask }; }),
                        catchError(() => of({ type: fromCaseWorker.ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );

    @Effect()
    searchFormInstance$ = this.actions$
        .pipe(
            ofType(fromFormInstance.ActionTypes.START_SEARCH),
            mergeMap((evt: fromFormInstance.StartSearch) => {
                return this.casePlanService.searchFormInstance(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(formInstance => { return { type: fromFormInstance.ActionTypes.COMPLETE_SEARCH, content: formInstance }; }),
                        catchError(() => of({ type: fromFormInstance.ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
        );
}