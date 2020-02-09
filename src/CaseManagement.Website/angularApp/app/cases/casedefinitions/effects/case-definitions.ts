import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, StartFetch, StartGet } from '../actions/case-definitions';
import { CaseDefinitionsService } from '../services/casedefinitions.service';

@Injectable()
export class CaseDefinitionsEffects {
    constructor(
        private actions$: Actions,
        private caseDefinitionsService: CaseDefinitionsService
    ) { }

    @Effect()
    loadCaseDefinitions$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH),
            mergeMap((evt: StartFetch) => {
                return this.caseDefinitionsService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text, evt.user)
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
                return this.caseDefinitionsService.get(evt.id)
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
                return this.caseDefinitionsService.getHistory(evt.id)
                    .pipe(
                        map(caseDefinitionHistory => { return { type: ActionTypes.COMPLETE_GET_HISTORY, content: caseDefinitionHistory }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_GET_HISTORY }))
                    );
            }
            )
        );
}