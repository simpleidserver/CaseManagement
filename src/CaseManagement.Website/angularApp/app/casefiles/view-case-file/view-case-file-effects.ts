import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { CaseFilesService } from '../services/casefiles.service';
import { ActionTypes } from './view-case-file-actions';

@Injectable()
export class ViewCaseFileEffects {
    constructor(
        private actions$: Actions,
        private caseFilesService: CaseFilesService,
        private caseDefinitionsService: CaseDefinitionsService
    ) { }

    @Effect()
    loadCaseFile = this.actions$
       .pipe(
            ofType(ActionTypes.LOADCASEFILE),
            mergeMap((evt:any) => {
                return this.caseFilesService.get(evt.caseFileId)
                    .pipe(
                        map(casefile => { return { type: ActionTypes.CASEFILELOADED, caseFile: casefile }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEFILE }))
                    );
                }
            )
    );
    @Effect()
    loadCaseDefinitions = this.actions$
        .pipe(
            ofType(ActionTypes.LOADCASEDEFINITIONS),
            mergeMap((evt: any) => {
                return this.caseDefinitionsService.search(evt.caseFileId, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.CASEDEFINITIONSLOADED, caseDefinitions: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEDEFINITIONS }))
                    );
            }
            )
        );
}