import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { catchError, mergeMap, map } from 'rxjs/operators';
import { ActionTypes } from './case-instance-actions';
import { CaseDefinitionsService } from '../../casedefinitions/services/casedefinitions.service'
import { CaseInstancesService } from '../../casedefinitions/services/caseinstances.service';
import { CaseInstance } from '../../casedefinitions/models/search-case-instances-result.model';
import { of } from 'rxjs';

@Injectable()
export class CaseInstanceEffects {
    constructor(
        private actions$: Actions,
        private caseDefinitionsService: CaseDefinitionsService,
        private caseInstancesService : CaseInstancesService
    ) { }

    @Effect()
    loadCaseDef$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEINSTANCELOAD),
            mergeMap((evt: any) => {
                return this.caseInstancesService.get(evt.id)
                    .pipe(
                        mergeMap((caseInstance : CaseInstance) => {
                            return this.caseDefinitionsService.get(caseInstance.TemplateId).pipe(
                                map(caseDefinition => { return { type: ActionTypes.CASEINSTANCELOADED, caseInstance: caseInstance, caseDefinition: caseDefinition}; }),
                                catchError(() => of({ type: ActionTypes.ERRORLOADCASEINSTANCE }))
                            );
                        }),                        
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    loadExecutionSteps = this.actions$
        .pipe(
            ofType(ActionTypes.CASEEXECUTIONSSTEPSLOAD),
            mergeMap((evt: any) => {
                return this.caseInstancesService.searchExecutionSteps(evt.startIndex, evt.count, evt.id, evt.order, evt.direction)
                    .pipe(
                        map(r => { return { type: ActionTypes.CASEEXECUTIONSTEPSLOADED, result: r }; }),
                        catchError(() => of({ type: ActionTypes.ERRORLOADCASEEXECUTIONSTEPS }))
                    );
            }
            )
        );
}