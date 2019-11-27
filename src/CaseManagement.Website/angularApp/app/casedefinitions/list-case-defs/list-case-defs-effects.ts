import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { ActionTypes } from './list-case-defs-actions';

@Injectable()
export class ListCaseDefsEffects {
    constructor(
        private actions$: Actions,
        private caseDefinitionsService: CaseDefinitionsService
    ) { }

    @Effect()
    loadCaseDefs$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEDEFSLOAD),
            mergeMap((evt:any) => {
                return this.caseDefinitionsService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(casedefs => { return { type: ActionTypes.CASEDEFSLOADED, result: casedefs }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEDEFS }))
                    );
                }
            )
        );
}