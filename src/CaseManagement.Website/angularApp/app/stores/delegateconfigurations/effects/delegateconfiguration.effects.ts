import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromCasePlan from '../actions/delegateconfiguration.actions';
import { DelegateConfigurationService } from '../services/delegateconfiguration.service';

@Injectable()
export class DelegateConfigurationEffects {
    constructor(
        private actions$: Actions,
        private delegateConfigurationService: DelegateConfigurationService
    ) { }

    @Effect()
    searchDelegateConfiguration$ = this.actions$
        .pipe(
            ofType(fromCasePlan.ActionTypes.SEARCH_DELEGATE_CONFIGURATION),
            mergeMap((evt: fromCasePlan.SearchDelegateConfiguration) => {
                return this.delegateConfigurationService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(content => { return { type: fromCasePlan.ActionTypes.COMPLETE_SEARCH_DELEGATE_CONFIGURATION, content: content }; }),
                        catchError(() => of({ type: fromCasePlan.ActionTypes.ERROR_SEARCH_DELEGATE_CONFIGURATION }))
                    );
            }
            )
    );

    @Effect()
    getDelegateConfiguration$ = this.actions$
        .pipe(
            ofType(fromCasePlan.ActionTypes.GET_DELEGATE_CONFIGURATION),
            mergeMap((evt: fromCasePlan.GetDelegateConfiguration) => {
                return this.delegateConfigurationService.get(evt.id)
                    .pipe(
                        map(content => { return { type: fromCasePlan.ActionTypes.COMPLETE_GET_DELEGATE_CONFIGURATION, content: content }; }),
                        catchError(() => of({ type: fromCasePlan.ActionTypes.ERROR_GET_DELEGATE_CONFIGURATION }))
                    );
            }
            )
    );

    @Effect()
    getAllDelegateConfigurations$ = this.actions$
        .pipe(
            ofType(fromCasePlan.ActionTypes.START_GET_ALL_DELEGATE_CONFIGURATION),
            mergeMap(() => {
                return this.delegateConfigurationService.getAll()
                    .pipe(
                        map(content => { return { type: fromCasePlan.ActionTypes.COMPLETE_GET_ALL_DELEGATE_CONFIGURATION, content: content }; }),
                        catchError(() => of({ type: fromCasePlan.ActionTypes.ERROR_GET_ALL_DELEGATE_CONFIGURATION }))
                    );
            }
            )
        );

    @Effect()
    updateDelegateConfiguration$ = this.actions$
        .pipe(
            ofType(fromCasePlan.ActionTypes.START_UPDATE_DELEGATE_CONFIGURATION),
            mergeMap((evt: fromCasePlan.UpdateDelegateConfiguration) => {
                return this.delegateConfigurationService.update(evt.id, evt.records)
                    .pipe(
                        map(() => { return { type: fromCasePlan.ActionTypes.COMPLETE_UPDATE_DELEGATE_CONFIGURATION }; }),
                        catchError(() => of({ type: fromCasePlan.ActionTypes.ERROR_UPDATE_DELEGATE_CONFIGURATION }))
                    );
            }
            )
        );
}