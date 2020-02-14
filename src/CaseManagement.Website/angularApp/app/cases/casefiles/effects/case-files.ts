import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, StartGet, StartSearch, StartSearchHistory } from '../actions/case-files';
import { CaseFilesService } from '../services/casefiles.service';

@Injectable()
export class CaseFilesEffects {
    constructor(
        private actions$: Actions,
        private caseFilesService: CaseFilesService
    ) { }

    @Effect()
    searchCaseFiles$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH),
            mergeMap((evt: StartSearch) => {
                return this.caseFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_SEARCH, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );

    @Effect()
    searchCaseFileHistories$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH_HISTORY),
            mergeMap((evt: StartSearchHistory) => {
                return this.caseFilesService.searchCaseFileHistories(evt.caseFileId, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_SEARCH_HISTORY, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_SEARCH_HISTORY }))
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