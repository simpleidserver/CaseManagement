import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, GetCmmnFile, SearchCmmnFiles, AddCmmnFile, PublishCmmnFile, UpdateCmmnFile, UpdateCmmnFilePayload } from '../actions/cmmn-files.actions';
import { CmmnFilesService } from '../services/cmmnfiles.service';
import { HumanTaskDefService } from '@app/stores/humantaskdefs/services/humantaskdef.service';
import { of, forkJoin } from 'rxjs';

@Injectable()
export class CmmnFilesEffects {
    constructor(
        private actions$: Actions,
        private cmmnFilesService: CmmnFilesService,
        private humanTaskDefService: HumanTaskDefService
    ) { }

    @Effect()
    searchCmmnFiles$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH_CMMNFILES),
            mergeMap((evt: SearchCmmnFiles) => {
                return this.cmmnFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.text, evt.caseFileId, evt.takeLatest)
                    .pipe(
                        map(cmmnfiles => { return { type: ActionTypes.COMPLETE_SEARCH_CMMNFILES, content: cmmnfiles }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_SEARCH_CMMNFILES }))
                    );
            }
            )
    );

    @Effect()
    getCmmnFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_GET_CMMNFILE),
            mergeMap((evt: GetCmmnFile) => {
                const getCmmnFileCall = this.cmmnFilesService.get(evt.id);
                const getHumanTaskDefsCall = this.humanTaskDefService.getAll();
                return forkJoin([getCmmnFileCall, getHumanTaskDefsCall]).pipe(
                    map((results: any[]) => { return { type: ActionTypes.COMPLETE_GET_CMMNFILE, humanTaskDefs: results[1], content: results[0] }; }),
                    catchError(() => of({ type: ActionTypes.ERROR_GET_CMMNFILE }))
                );
            }
            )
    );

    @Effect()
    addCmmnFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.ADD_CMMNFILE),
            mergeMap((evt: AddCmmnFile) => {
                return this.cmmnFilesService.add(evt.name, evt.description)
                    .pipe(
                        map(str => { return { type: ActionTypes.COMPLETE_ADD_CMMNFILE, id: str }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_ADD_CMMNFILE }))
                    );
            }
            )
    );

    @Effect()
    publishCmmnFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.PUBLISH_CMMNFILE),
            mergeMap((evt: PublishCmmnFile) => {
                return this.cmmnFilesService.publish(evt.id)
                    .pipe(
                        map(str => { return { type: ActionTypes.COMPLETE_PUBLISH_CMMNFILE, id: str }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_PUBLISH_CMMNFILE }))
                    );
            }
            )
    );

    @Effect()
    updateCmmnFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.UPDATE_CMMNFILE),
            mergeMap((evt: UpdateCmmnFile) => {
                return this.cmmnFilesService.update(evt.id, evt.name, evt.description)
                    .pipe(
                        mergeMap(() => {
                            return this.cmmnFilesService.updatePayload(evt.id, evt.xml)
                                .pipe(
                                    map(() => { return { type: ActionTypes.COMPLETE_UPDATE_CMMNFILE, id: evt.id, name: evt.name, description: evt.description, payload: evt.xml }; }),
                                    catchError(() => of({ type: ActionTypes.ERROR_UPDATE_CMMNFILE }))
                                )
                        }),
                        catchError(() => of({ type: ActionTypes.ERROR_UPDATE_CMMNFILE }))
                    );
            }
            )
    );

    @Effect()
    updateCmmnFilePayload$ = this.actions$
        .pipe(
            ofType(ActionTypes.UPDATE_CMMNFILE_PAYLOAD),
            mergeMap((evt: UpdateCmmnFilePayload) => {
                return this.cmmnFilesService.updatePayload(evt.id, evt.payload)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_UPDATE_CMMNFILE_PAYLOAD, payload: evt.payload }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_UPDATE_CMMNFILE_PAYLOAD }))
                    );
            }
            )
        );
}