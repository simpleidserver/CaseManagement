import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, GetBpmnFile, SearchBpmnFiles, UpdateBpmnFile, UpdateBpmnFilePayload } from '../actions/bpmn-files.actions';
import { BpmnFilesService } from '../services/bpmnfiles.service';

@Injectable()
export class BpmnFilesEffects {
    constructor(
        private actions$: Actions,
        private bpmnFilesService: BpmnFilesService
    ) { }

    @Effect()
    searchBpmnFiles$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_SEARCH_BPMNFILES),
            mergeMap((evt: SearchBpmnFiles) => {
                return this.bpmnFilesService.search(evt.startIndex, evt.count, evt.order, evt.direction, true)
                    .pipe(
                        map(bpmnFiles => { return { type: ActionTypes.COMPLETE_SEARCH_BPMNFILES, content: bpmnFiles }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_SEARCH_BPMNFILES }))
                    );
            }
            )
    );

    @Effect()
    getBpmnFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_GET_BPMNFILE),
            mergeMap((evt: GetBpmnFile) => {
                return this.bpmnFilesService.get(evt.id)
                    .pipe(
                        map(bpmnFile => { return { type: ActionTypes.COMPLETE_GET_BPMNFILE, bpmnFile: bpmnFile }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_GET_BPMNFILE }))
                    );
            }
            )
    );

    @Effect()
    updateBpmnFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.UPDATE_BPMNFILE),
            mergeMap((evt: UpdateBpmnFile) => {
                return this.bpmnFilesService.update(evt.id, evt.name, evt.description)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_UPDATE_BPMNFILE, id: evt.id, name: evt.name, description: evt.description}; }),
                        catchError(() => of({ type: ActionTypes.ERROR_UPDATE_BPMNFILE }))
                    );
            }
            )
    );

    @Effect()
    updateBpmnFilePayload$ = this.actions$
        .pipe(
            ofType(ActionTypes.UPDATE_BPMNFILE_PAYLOAD),
            mergeMap((evt: UpdateBpmnFilePayload) => {
                return this.bpmnFilesService.updatePayload(evt.id, evt.payload)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD, id: evt.id, payload: evt.payload }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_UPDATE_BPMNFILE_PAYLOAD }))
                    );
            }
            )
        );

    @Effect()
    publishBpmnFile$ = this.actions$
        .pipe(
            ofType(ActionTypes.PUBLISH_BPMNFILE),
            mergeMap((evt: UpdateBpmnFile) => {
                return this.bpmnFilesService.publish(evt.id)
                    .pipe(
                        map(str => { return { type: ActionTypes.COMPLETE_PUBLISH_BPMNFILE, id: str}; }),
                        catchError(() => of({ type: ActionTypes.ERROR_PUBLISH_BPMNFILE }))
                    );
            }
            )
        );
}