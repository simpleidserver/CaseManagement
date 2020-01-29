import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, StartFetch, StartGet } from '../actions/case-files';
import { CaseFilesService } from '../services/casefiles.service';

@Injectable()
export class CaseFilesEffects {
    constructor(
        private actions$: Actions,
        private caseFilesService: CaseFilesService
    ) { }

    @Effect()
    loadCaseFiles$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH),
            mergeMap((evt: StartFetch) => {
                return this.caseFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text, evt.user)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_SEARCH, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );

    @Effect()
    loadCaseFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_GET),
            mergeMap((evt: StartGet) => {
                return this.caseFilesService.get(evt.id)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_GET, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_GET }))
                    );
            }
            )
        );

}