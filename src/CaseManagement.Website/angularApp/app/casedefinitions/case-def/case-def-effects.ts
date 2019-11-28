import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { CaseDefinitionsService } from '../services/casedefinitions.service';
import { CaseInstancesService } from '../services/caseinstances.service';
import { ActionTypes } from './case-def-actions';

@Injectable()
export class CaseDefEffects {
    constructor(
        private actions$: Actions,
        private caseDefinitionsService: CaseDefinitionsService,
        private caseInstancesService: CaseInstancesService
    ) { }

    @Effect()
    loadCaseDef$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEDEFLOAD),
            mergeMap((evt: any) => {
                return this.caseDefinitionsService.get(evt.id)
                    .pipe(
                        map(casedef => { return { type: ActionTypes.CASEDEFLOADED, result: casedef }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEDEF }))
                    );
            }
            )
        );

    @Effect()
    loadCaseInstances = this.actions$
        .pipe(
            ofType(ActionTypes.CASEINSTANCESLOAD),
            mergeMap((evt: any) => {
                return this.caseInstancesService.search(evt.startIndex, evt.count, evt.id, evt.order, evt.direction)
                    .pipe(
                        map(caseInstances => { return { type: ActionTypes.CASEINSTANCESLOADED, result: caseInstances }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEINSTANCES }))
                    );
            }
            )
    );        
}