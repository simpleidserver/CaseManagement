import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { Observable } from 'rxjs/Rx';
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { CaseFilesService } from '../services/casefiles.service';
import { CaseInstancesService } from '../services/caseinstances.service';
import { ActionTypes } from './view-actions';
import { CaseFormInstancesService } from '../services/caseforminstances.service';
import { CaseActivationsService } from '../services/caseactivations.service';

@Injectable()
export class ViewCaseDefinitionEffects {
    constructor(
        private actions$: Actions,
        private caseDefinitionsService: CaseDefinitionsService,
        private caseFileService: CaseFilesService,
        private caseInstancesService: CaseInstancesService,
        private formInstancesService: CaseFormInstancesService,
        private caseActivationsService: CaseActivationsService
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

    @Effect()
    loadFormInstances$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEINSTANCESLOAD),
            mergeMap((evt: any) => {
                return this.formInstancesService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction).pipe(
                    map(formInstances => { return { type: ActionTypes.CASEFORMINSTANCESLOADED, result: formInstances }; }),
                    catchError(() => of({ type: ActionTypes.ERRORLOADCASEFORMINSTANCES }))
                );
            })
    );

    @Effect()
    loadCaseActivations$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEACTIVATIONSLOAD),
            mergeMap((evt: any) => {
                return this.caseActivationsService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction).pipe(
                    map(caseActivations => { return { type: ActionTypes.CASEACTIVATIONSLOADED, result: caseActivations }; }),
                    catchError(() => of({ type: ActionTypes.ERRORLOADCASEACTIVATIONS }))
                );
            })
        );
}