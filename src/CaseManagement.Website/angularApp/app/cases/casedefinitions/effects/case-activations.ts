import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, StartFetch } from '../actions/case-activations';
import { CaseActivationsService } from '../services/caseactivations.service';

@Injectable()
export class CaseActivationsEffects {
    constructor(
        private actions$: Actions,
        private caseActivationsService: CaseActivationsService
    ) { }

    @Effect()
    loadCaseInstances$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH),
            mergeMap((evt: StartFetch) => {
                return this.caseActivationsService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_SEARCH, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );
}