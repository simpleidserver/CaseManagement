import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, SearchCases } from '../actions/cases.actions';
import { CasesService } from '../services/cases.service';

@Injectable()
export class CasesEffects {
    constructor(
        private actions$: Actions,
        private casesService: CasesService
    ) { }

    @Effect()
    searchCaseFiles$ = this.actions$
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
}