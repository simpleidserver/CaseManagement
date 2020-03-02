import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromRole from '../actions/role';
import { RolesService } from '../services/role.service';

@Injectable()
export class RoleEffects {
    constructor(
        private actions$: Actions,
        private roleService: RolesService
    ) { }

    @Effect()
    searchRoles$ = this.actions$
        .pipe(
            ofType(fromRole.ActionTypes.START_SEARCH),
            mergeMap((evt: fromRole.StartSearch) => {
                return this.roleService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map(casePlans => { return { type: fromRole.ActionTypes.COMPLETE_SEARCH, content: casePlans }; }),
                        catchError(() => of({ type: fromRole.ActionTypes.COMPLETE_SEARCH }))
                    );
            }
            )
    );

    @Effect()
    loadRole$ = this.actions$
        .pipe(
            ofType(fromRole.ActionTypes.START_GET),
            mergeMap((evt: fromRole.StartGet) => {
                return this.roleService.get(evt.role)
                    .pipe(
                        map(role => { return { type: fromRole.ActionTypes.COMPLETE_GET, content: role }; }),
                        catchError(() => of({ type: fromRole.ActionTypes.COMPLETE_GET }))
                    );
            }
            )
        );
}