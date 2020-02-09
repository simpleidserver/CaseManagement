import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, StartFetch, StartGetFileItems, StartGet } from '../actions/case-instances';
import { CaseInstancesService } from '../services/caseinstances.service';

@Injectable()
export class CaseInstancesEffects {
    constructor(
        private actions$: Actions,
        private caseInstancesService: CaseInstancesService
    ) { }

    @Effect()
    loadCaseInstances$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH),
            mergeMap((evt: StartFetch) => {
                return this.caseInstancesService.search(evt.id, evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(casefiles => { return { type: ActionTypes.COMPLETE_SEARCH, content: casefiles }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );

    @Effect()
    loadCaseFileItems$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_GET_FILE_ITEMS),
            mergeMap((evt: StartGetFileItems) => {
                return this.caseInstancesService.getCaseFileItems(evt.id)
                    .pipe(
                        map(caseFileItems => { return { type: ActionTypes.COMPLETE_SEARCH, content: caseFileItems }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_GET_FILE_ITEMS }))
                    );
            }
            )
    );

    @Effect()
    loadCaseInstance$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_GET),
            mergeMap((evt: StartGet) => {
                return this.caseInstancesService.get(evt.id)
                    .pipe(
                        map(caseInstance => { return { type: ActionTypes.COMPLETE_GET, content: caseInstance }; }),
                        catchError(() => of({ type: ActionTypes.COMPLETE_GET }))
                    );
            }
            )
        );

}