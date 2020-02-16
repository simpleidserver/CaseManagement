import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, StartSearch, StartGet } from '../actions/case-plans';
import { CasePlansService } from '../services/caseplans.service';

@Injectable()
export class CasePlansEffects {
    constructor(
        private actions$: Actions,
        private casePlansService: CasePlansService
    ) { }

    @Effect()
    searchCasePlans$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH),
            mergeMap((evt: StartSearch) => {
                return this.casePlansService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_SEARCH, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );

    @Effect()
    loadCaseDefinition$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_GET),
            mergeMap((evt: StartGet) => {
                return this.casePlansService.get(evt.id)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_GET, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_GET }))
                    );
            }
            )
    );

    @Effect()
    loadCaseDefinitionHistory$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_GET_HISTORY),
            mergeMap((evt: StartGet) => {
                return this.casePlansService.getHistory(evt.id)
                    .pipe(
                        map(caseDefinitionHistory => { return { type: ActionTypes.COMPLETE_GET_HISTORY, content: caseDefinitionHistory }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_GET_HISTORY }))
                    );
            }
            )
        );
}