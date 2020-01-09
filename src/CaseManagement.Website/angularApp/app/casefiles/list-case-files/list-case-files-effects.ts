import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { CaseFilesService } from '../services/casefiles.service';
import { ActionTypes } from './list-case-files-actions';

@Injectable()
export class ListCaseFilesEffects {
    constructor(
        private actions$: Actions,
        private caseFilesService: CaseFilesService
    ) { }

    @Effect()
    loadCaseFiles$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEFILESLOAD),
            mergeMap((evt:any) => {
                return this.caseFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.CASEFILESLOADED, result: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEFILES }))
                    );
                }
            )
        );
}