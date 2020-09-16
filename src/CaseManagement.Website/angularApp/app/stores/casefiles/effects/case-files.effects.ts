import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, AddCaseFile, GetCaseFile, PublishCaseFile, SearchCaseFiles, SearchCaseFilesHistory, UpdateCaseFile } from '../actions/case-files.actions';
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
            ofType(ActionTypes.START_SEARCH_CASEFILES),
            mergeMap((evt: SearchCaseFiles) => {
                return this.caseFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text, null, true)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_SEARCH_CASEFILES, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_SEARCH_CASEFILES }))
                    );
            }
            )
    );

    @Effect()
    searchCaseFileHistories$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH_CASEFILES_HISTORY),
            mergeMap((evt: SearchCaseFilesHistory) => {
                return this.caseFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, null, evt.caseFileId, false)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_SEARCH_CASEFILES_HISTORY, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_SEARCH_CASEFILES_HISTORY }))
                    );
            }
            )
    ); 

    @Effect()
    getCaseFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_GET_CASEFILE),
            mergeMap((evt: GetCaseFile) => {
                return this.caseFilesService.get(evt.id)
                    .pipe(
                        map(casefile => { return { type: ActionTypes.COMPLETE_GET_CASEFILE, content: casefile }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_GET_CASEFILE }))
                    );
            }
            )
    );

    @Effect()
    addCaseFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.ADD_CASEFILE),
            mergeMap((evt: AddCaseFile) => {
                return this.caseFilesService.add(evt.name, evt.description)
                    .pipe(
                        map(str => { return { type: ActionTypes.COMPLETE_ADD_CASEFILE, id: str }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_ADD_CASEFILE }))
                    );
            }
            )
    );

    @Effect()
    publishCaseFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.PUBLISH_CASEFILE),
            mergeMap((evt: PublishCaseFile) => {
                return this.caseFilesService.publish(evt.id)
                    .pipe(
                        map(str => { return { type: ActionTypes.COMPLETE_PUBLISH_CASEFILE, id: str }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_PUBLISH_CASEFILE }))
                    );
            }
            )
    );

    @Effect()
    updateCaseFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.UPDATE_CASEFILE),
            mergeMap((evt: UpdateCaseFile) => {
                return this.caseFilesService.update(evt.id, evt.name, evt.description, evt.payload)
                    .pipe(
                        map(str => { return { type: ActionTypes.COMPLETE_UPDATE_CASEFILE, id: str }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_UPDATE_CASEFILE }))
                    );
            }
            )
        );

}