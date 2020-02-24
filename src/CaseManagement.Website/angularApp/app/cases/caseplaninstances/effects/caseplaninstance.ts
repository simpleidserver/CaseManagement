import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromCasePlanInstance from '../actions/caseplaninstance';
import { CasePlanInstanceService } from '../services/caseplaninstance.service';

@Injectable()
export class CasePlanInstanceEffects {
    constructor(
        private actions$: Actions,
        private casePlanInstanceService: CasePlanInstanceService
    ) { }

    @Effect()
    searchCasePlanInstance$ = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.START_SEARCH),
            mergeMap((evt: fromCasePlanInstance.StartSearch) => {
                return this.casePlanInstanceService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.casePlanId)
                    .pipe(
                        map(casePlanInstances => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH, content: casePlanInstances }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );

    @Effect()
    searchMyCasePlanInstance$ = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.START_SEARCH_ME),
            mergeMap((evt: fromCasePlanInstance.StartSearchMe) => {
                return this.casePlanInstanceService.searchMe(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(casePlanInstances => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH_ME, content: casePlanInstances }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH_ME }))
                    );
            }
            )
    );

    @Effect()
    selectGetCasePlanInstance = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.START_GET),
            mergeMap((evt: fromCasePlanInstance.StartGet) => {
                return this.casePlanInstanceService.get(evt.id)
                    .pipe(
                        map(casePlanInstance => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_GET, content: casePlanInstance }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.COMPLETE_GET }))
                    );
            }
            )
        );
}