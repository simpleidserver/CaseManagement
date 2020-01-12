import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { Observable } from 'rxjs/Rx';
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { CaseFilesService } from '../services/casefiles.service';
import { CaseInstancesService } from '../services/caseinstances.service';
import { ActionTypes } from './view-actions';

@Injectable()
export class ViewCaseDefinitionEffects {
    constructor(
        private actions$: Actions,
        private caseDefinitionsService: CaseDefinitionsService,
        private caseFileService: CaseFilesService,
        private caseInstancesService: CaseInstancesService
    ) { }

    @Effect()
    loadCaseDefinition$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEDEFINITIONLOAD),
            mergeMap((evt:any) => {
                return Observable.forkJoin([this.caseDefinitionsService.get(evt.id), this.caseDefinitionsService.getHistory(evt.id)])
                    .pipe(
                        mergeMap((responses: any) => {
                            return this.caseFileService.get(responses[0].CaseFile).pipe(
                                map(caseFile => { return { type: ActionTypes.CASEDEFINITIONLOADED, caseDefinition: responses[0], caseFile: caseFile, caseDefinitionHistory: responses[1] }; }),
                                catchError(() => of({ type: ActionTypes.ERRORLOADCASEDEFINITION }))
                            );
                        }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEDEFINITION }))
                    );
                }
            )
    );

    @Effect()
    loadCaseInstances$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEINSTANCESLOAD),
            mergeMap((evt: any) => {
                return this.caseInstancesService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction).pipe(
                    map(caseInstances => { return { type: ActionTypes.CASEINSTANCESLOADED, result: caseInstances }; }),
                    catchError(() => of({ type: ActionTypes.ERRORLOADCASEDEFINITION }))
                );
            })
        );
}