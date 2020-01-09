import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { CaseInstancesService } from '../services/caseinstances.service';
import { ActionTypes } from './view-case-definition-actions';

@Injectable()
export class ViewCaseDefinitionEffects {
    constructor(
        private actions$: Actions,
        private caseDefinitionsService: CaseDefinitionsService,
        private caseInstancesService : CaseInstancesService
    ) { }

    @Effect()
    loadCaseDefinition = this.actions$
        .pipe(
            ofType(ActionTypes.LOADCASEDEFINITION),
            mergeMap((evt:any) => {
                return this.caseDefinitionsService.get(evt.caseDefinitionId)
                    .pipe(
                        map(caseDefinition => { return { type: ActionTypes.CASEDEFINITIONLOADED, caseDefinition: caseDefinition }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEDEFINITION }))
                    );
                }
            )
    );
    @Effect()
    loadCaseInstances = this.actions$
        .pipe(
            ofType(ActionTypes.LOADCASEINSTANCES),
            mergeMap((evt: any) => {
                return this.caseInstancesService.search(evt.caseDefinitionId, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(caseInstances => { return { type: ActionTypes.CASEINSTANCESLOADED, caseInstances: caseInstances }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEINSTANCES }))
                    );
            }
            )
    );
}