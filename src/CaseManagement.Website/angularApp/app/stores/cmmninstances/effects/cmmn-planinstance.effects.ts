import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromCmmnPlanInstance from '../actions/cmmn-instances.actions';
import { CmmnPlanInstanceResult } from '../models/cmmn-planinstance.model';
import { CmmnPlanInstanceService } from '../services/cmmn-planinstance.service';

@Injectable()
export class CmmnPlanInstanceEffects {
    constructor(
        private actions$: Actions,
        private cmmnPlanInstanceService: CmmnPlanInstanceService
    ) { }

    @Effect()
    launchCasePlanInstance = this.actions$
        .pipe(
            ofType(fromCmmnPlanInstance.ActionTypes.LAUNCH_CMMN_PLANINSTANCE),
            mergeMap((evt: fromCmmnPlanInstance.LaunchCmmnPlanInstance) => {
                return this.cmmnPlanInstanceService.createCasePlanInstance(evt.cmmnPlanId)
                    .pipe(
                        mergeMap((r: CmmnPlanInstanceResult) => {
                            return this.cmmnPlanInstanceService.launchCasePlanInstance(r.id)
                                .pipe(
                                    map(() => { return { type: fromCmmnPlanInstance.ActionTypes.COMPLETE_LAUNCH_CMMN_PLANINSTANCE, cmmnPlanInstance: r  } } ),
                                    catchError(() => of({ type: fromCmmnPlanInstance.ActionTypes.ERROR_LAUNCH_CMMN_PLANINSTANCE }))
                                )
                        }),
                        catchError(() => of({ type: fromCmmnPlanInstance.ActionTypes.ERROR_LAUNCH_CMMN_PLANINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    searchCasePlanInstances = this.actions$
        .pipe(
            ofType(fromCmmnPlanInstance.ActionTypes.SEARCH_CMMN_PLANINSTANCE),
            mergeMap((evt: fromCmmnPlanInstance.SearchCmmnPlanInstance) => {
                return this.cmmnPlanInstanceService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.casePlanId)
                    .pipe(
                        map(cmmnPlanInstances => { return { type: fromCmmnPlanInstance.ActionTypes.COMPLETE_SEARCH_CMMN_PLANINSTANCE, content: cmmnPlanInstances }; }),
                        catchError(() => of({ type: fromCmmnPlanInstance.ActionTypes.ERROR_SEARCH_CMMN_PLANINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    getCasePlanInstance = this.actions$
        .pipe(
            ofType(fromCmmnPlanInstance.ActionTypes.GET_CMMN_PLANINSTANCE),
            mergeMap((evt: fromCmmnPlanInstance.GetCmmnPlanInstance) => {
                return this.cmmnPlanInstanceService.get(evt.cmmnPlanInstanceId)
                    .pipe(
                        map(cmmnPlanInstance => { return { type: fromCmmnPlanInstance.ActionTypes.COMPLETE_GET_CMMN_PLANINSTANCE, content: cmmnPlanInstance }; }),
                        catchError(() => of({ type: fromCmmnPlanInstance.ActionTypes.ERROR_GET_CMMN_PLANINSTANCE }))
                    );
            }
            )
        );
}