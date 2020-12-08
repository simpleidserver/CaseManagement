import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromCasePlan from '../actions/cmmn-plans.actions';
import { CmmnPlanService } from '../services/cmmn-plan.service';

@Injectable()
export class CmmnPlanEffects {
    constructor(
        private actions$: Actions,
        private cmmnPlanService: CmmnPlanService
    ) { }

    @Effect()
    searchCmmnPlans$ = this.actions$
        .pipe(
            ofType(fromCasePlan.ActionTypes.SEARCH_CMMN_PLANS),
            mergeMap((evt: fromCasePlan.SearchCmmnPlans) => {
                return this.cmmnPlanService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.caseFileId)
                    .pipe(
                        map(cmmnPlans => { return { type: fromCasePlan.ActionTypes.COMPLETE_SEARCH_CMMN_PLANS, content: cmmnPlans }; }),
                        catchError(() => of({ type: fromCasePlan.ActionTypes.ERROR_SEARCH_CMMN_PLANS }))
                    );
            }
            )
    );

    @Effect()
    getCmmnPlan$ = this.actions$
        .pipe(
            ofType(fromCasePlan.ActionTypes.GET_CMMN_PLAN),
            mergeMap((evt: fromCasePlan.GetCmmnPlan) => {
                return this.cmmnPlanService.get(evt.id)
                    .pipe(
                        map(cmmnPlan => { return { type: fromCasePlan.ActionTypes.COMPLETE_GET_CMMN_PLAN, content: cmmnPlan }; }),
                        catchError(() => of({ type: fromCasePlan.ActionTypes.ERROR_GET_CMMN_PLAN }))
                    );
            }
            )
    );
}