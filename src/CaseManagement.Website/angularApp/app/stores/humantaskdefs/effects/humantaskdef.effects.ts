import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromHumanTask from '../actions/humantaskdef.actions';
import { HumanTaskDefService } from '../services/humantaskdef.service';

@Injectable()
export class HumanTaskDefEffects {
    constructor(
        private actions$: Actions,
        private humanTaskDefService: HumanTaskDefService
    ) { }

    @Effect()
    getHumanTaskDef = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.START_GET_HUMANTASKDEF),
            mergeMap((evt: fromHumanTask.GetHumanTaskDef) => {
                return this.humanTaskDefService.get(evt.id)
                    .pipe(
                        map(humanTaskDef => { return { type: fromHumanTask.ActionTypes.COMPLETE_GET_HUMANTASKDEF, content: humanTaskDef }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_GET_HUMANTASKDEF }))
                    );
            }
            )
        );
}