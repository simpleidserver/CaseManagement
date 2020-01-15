import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { CaseInstancesService } from '../../casedefinitions/services/caseinstances.service';
import { ActionTypes } from './view-actions';
import { CaseInstance } from '../../casedefinitions/models/case-instance.model';
import { CaseDefinitionsService } from '../../casedefinitions/services/casedefinitions.service';
import { CaseFilesService } from '../../casedefinitions/services/casefiles.service';
import { CaseDefinition } from '../../casedefinitions/models/case-definition.model';

@Injectable()
export class ViewCaseInstanceEffects {
    constructor(
        private actions$: Actions,
        private caseInstancesService: CaseInstancesService,
        private caseDefinitionService: CaseDefinitionsService,
        private caseFileService: CaseFilesService
    ) { }

    @Effect()
    loadCaseInstance$ = this.actions$
        .pipe(
            ofType(ActionTypes.CASEINSTANCELOAD),
            mergeMap((evt: any) => {
                let self = this;
                return this.caseInstancesService.get(evt.id).pipe(
                    mergeMap((caseInstance: CaseInstance) => {
                        return self.caseDefinitionService.get(caseInstance.DefinitionId).pipe(
                            mergeMap((caseDefinition: CaseDefinition) => {
                                return self.caseFileService.get(caseDefinition.CaseFile).pipe(
                                    map(caseFile => { return { type: ActionTypes.CASEINSTANCELOADED, caseInstance: caseInstance, caseDefinition: caseDefinition, caseFile: caseFile }; }),
                                    catchError(() => of({ type: ActionTypes.ERRORLOADCASEINSTANCE }))
                                );
                            }),
                            catchError(() => of({ type: ActionTypes.ERRORLOADCASEINSTANCE }))
                        );
                    }),
                    catchError(() => of({ type: ActionTypes.ERRORLOADCASEINSTANCE }))
                );
            })
        );
}