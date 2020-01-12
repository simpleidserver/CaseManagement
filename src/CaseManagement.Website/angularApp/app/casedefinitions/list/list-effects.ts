import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { ActionTypes } from './list-actions';

@Injectable()
export class ListCaseDefinitionsEffects {
    constructor(
        private actions$: Actions,
        private caseDefinitionsService: CaseDefinitionsService
    ) { }

    @Effect()
    loadCaseFiles$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEDEFINITIONSLOAD),
            mergeMap((evt:any) => {
                return this.caseDefinitionsService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.CASEDEFINITIONSLOADED, result: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEDEFINITIONS }))
                    );
                }
            )
        );
}