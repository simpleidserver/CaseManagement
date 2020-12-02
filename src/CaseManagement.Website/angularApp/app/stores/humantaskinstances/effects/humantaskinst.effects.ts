import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromHumanTask from '../actions/humantaskinst.actions';
import { HumanTaskInstService } from '../services/humantaskinst.service';

@Injectable()
export class HumanTaskInstEffects {
    constructor(
        private actions$: Actions,
        private humanTaskInstService: HumanTaskInstService
    ) { }

    @Effect()
    createHumanTaskInstance = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.CREATE_HUMANTASKINSTANCE),
            mergeMap((evt: fromHumanTask.CreateHumanTaskInstanceOperation) => {
                return this.humanTaskInstService.create(evt.cmd)
                    .pipe(
                        map(id => { return { type: fromHumanTask.ActionTypes.COMPLETE_CREATE_HUMANTASKINSTANCE, content: id }; }),
                        catchError((e) => of({ type: fromHumanTask.ActionTypes.ERROR_CREATE_HUMANTASKINSTANCE, error: e }))
                    );
            }
            )
    );

    @Effect()
    createMeHumanTaskInstance = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.CREATE_ME_HUMANTASKINSTANCE),
            mergeMap((evt: fromHumanTask.CreateMeHumanTaskInstanceOperation) => {
                return this.humanTaskInstService.createMe(evt.cmd)
                    .pipe(
                        map(id => { return { type: fromHumanTask.ActionTypes.COMPLETE_ME_CREATE_HUMANTASKINSTANCE, content: id }; }),
                        catchError((e) => of({ type: fromHumanTask.ActionTypes.ERROR_ME_CREATE_HUMANTASKINSTANCE, error: e }))
                    );
            }
            )
        );
}