import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, StartFetch } from '../actions/case-form-instances';
import { CaseFormInstancesService } from '../services/caseforminstances.service';

@Injectable()
export class CaseFormInstancesEffects {
    constructor(
        private actions$: Actions,
        private caseFormInstancesService: CaseFormInstancesService
    ) { }

    @Effect()
    loadCaseInstances$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH),
            mergeMap((evt: StartFetch) => {
                return this.caseFormInstancesService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(formInstances => { return { type: ActionTypes.COMPLETE_SEARCH, content: formInstances }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );
}