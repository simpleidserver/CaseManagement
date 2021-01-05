import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, Activate, GetCase, Reenable, SearchCases } from '../actions/cases.actions';
import { CasesService } from '../services/cases.service';

@Injectable()
export class CasesEffects {
    constructor(
        private actions$: Actions,
        private casesService: CasesService
    ) { }

    @Effect()
    searchCaseInstances$ = this.actions$
        .pipe(
            ofType(ActionTypes.SEARCH_CASES),
            mergeMap((evt: SearchCases) => {
                return this.casesService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(cases => { return { type: ActionTypes.COMPLETE_SEARCH_CASES, content: cases }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_SEARCH_CASES }))
                    );
            }
            )
    );

    @Effect()
    getCase$ = this.actions$
        .pipe(
            ofType(ActionTypes.GET_CASE),
            mergeMap((evt: GetCase) => {
                return this.casesService.get(evt.id)
                    .pipe(
                        map(cs => { return { type: ActionTypes.COMPLETE_GET_CASE, content: cs }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_GET_CASE }))
                    );
            }
            )
    );

    @Effect()
    activate$ = this.actions$
        .pipe(
            ofType(ActionTypes.ACTIVATE),
            mergeMap((evt: Activate) => {
                return this.casesService.activate(evt.id, evt.elt)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_ACTIVATE }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_ACTIVATE }))
                    );
            }
            )
    );

    @Effect()
    disable$ = this.actions$
        .pipe(
            ofType(ActionTypes.DISABLE),
            mergeMap((evt: Activate) => {
                return this.casesService.disable(evt.id, evt.elt)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_DISABLE }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_DISABLE }))
                    );
            }
            )
    );

    @Effect()
    reenable$ = this.actions$
        .pipe(
            ofType(ActionTypes.REENABLE),
            mergeMap((evt: Reenable) => {
                return this.casesService.reenable(evt.id, evt.elt)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_REENABLE }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_REENABLE }))
                    );
            }
            )
    );

    @Effect()
    complete$ = this.actions$
        .pipe(
            ofType(ActionTypes.COMPLETE),
            mergeMap((evt: Reenable) => {
                return this.casesService.complete(evt.id, evt.elt)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETED }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_COMPLETE }))
                    );
            }
            )
        );

}