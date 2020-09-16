import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromCasePlanInstance from '../actions/caseplaninstance.actions';
import { CasePlanInstanceResult } from '../models/caseplaninstance.model';
import { CasePlanInstanceService } from '../services/caseplaninstance.service';

@Injectable()
export class CasePlanInstanceEffects {
    constructor(
        private actions$: Actions,
        private casePlanInstanceService: CasePlanInstanceService
    ) { }

    @Effect()
    searchCasePlanInstances = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.START_SEARCH_CASE_PLANINSTANCES),
            mergeMap((evt: fromCasePlanInstance.SearchCasePlanInstances) => {
                return this.casePlanInstanceService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.casePlanId)
                    .pipe(
                        map(casePlanInstances => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_SEARCH_CASE_PLANINSTANCES, content: casePlanInstances }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.ERROR_SEARCH_CASE_PLANINSTANCES }))
                    );
            }
            )
    );

    @Effect()
    getCasePlanInstance = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.START_GET_CASE_PLANINSTANCE),
            mergeMap((evt: fromCasePlanInstance.GetCasePlanInstance) => {
                return this.casePlanInstanceService.get(evt.id)
                    .pipe(
                        map(casePlanInstance => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_GET_CASE_PLANINSTANCE, content: casePlanInstance }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.ERROR_GET_CASE_PLANINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    launchCasePlanInstance = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.LAUNCH_CASE_PLANINSTANCE),
            mergeMap((evt: fromCasePlanInstance.LaunchCasePlanInstance) => {
                return this.casePlanInstanceService.createCasePlanInstance(evt.casePlanId)
                    .pipe(
                        mergeMap((r: CasePlanInstanceResult) => {
                            return this.casePlanInstanceService.launchCasePlanInstance(r.id)
                                .pipe(
                                    map(() => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_LAUNCH_CASE_PLANINSTANCE } } ),
                                    catchError(() => of({ type: fromCasePlanInstance.ActionTypes.ERROR_LAUNCH_CASE_PLANINSTANCE }))
                                )
                        }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.ERROR_LAUNCH_CASE_PLANINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    reactivateCasePlanInstance = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.REACTIVATE_CASE_PLANINSTANCE),
            mergeMap((evt: fromCasePlanInstance.ReactivateCasePlanInstance) => {
                return this.casePlanInstanceService.reactivateCasePlanInstance(evt.casePlanInstanceId)
                    .pipe(
                        map(() => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_REACTIVATE_CASE_PLANINSTANCE }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.ERROR_REACTIVATE_CASE_PLANINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    suspendCasePlanInstance = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.SUSPEND_CASE_PLANINSTANCE),
            mergeMap((evt: fromCasePlanInstance.SuspendCasePlanInstance) => {
                return this.casePlanInstanceService.suspendCasePlanInstance(evt.casePlanInstanceId)
                    .pipe(
                        map(() => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_SUSPEND_CASE_PLANINSTANCE }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.ERROR_SUSPEND_CASE_PLANINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    resumeCasePlanInstance = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.RESUME_CASE_PLANINSTANCE),
            mergeMap((evt: fromCasePlanInstance.ResumeCasePlanInstance) => {
                return this.casePlanInstanceService.resumeCasePlanInstance(evt.casePlanInstanceId)
                    .pipe(
                        map(() => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_RESUME_CASE_PLANINSTANCE }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.ERROR_RESUME_CASE_PLANINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    closeCasePlanInstance = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.CLOSE_CASE_PLANINSTANCE),
            mergeMap((evt: fromCasePlanInstance.CloseCasePlanInstance) => {
                return this.casePlanInstanceService.closeCasePlanInstance(evt.casePlanInstanceId)
                    .pipe(
                        map(() => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_CLOSE_CASE_PLANINSTANCE }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.ERROR_CLOSE_CASE_PLANINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    enableCasePlanInstanceElt = this.actions$
        .pipe(
            ofType(fromCasePlanInstance.ActionTypes.ENABLE_CASE_PLANINSTANCE_ELT),
            mergeMap((evt: fromCasePlanInstance.EnableCasePlanInstanceElt) => {
                return this.casePlanInstanceService.enable(evt.casePlanInstanceId, evt.casePlanInstanceEltId)
                    .pipe(
                        map(() => { return { type: fromCasePlanInstance.ActionTypes.COMPLETE_ENABLE_CASE_PLANINSTANCE_ELT }; }),
                        catchError(() => of({ type: fromCasePlanInstance.ActionTypes.ERROR_ENABLE_CASE_PLANINSTANCE_ELT }))
                    );
            }
            )
        );
}