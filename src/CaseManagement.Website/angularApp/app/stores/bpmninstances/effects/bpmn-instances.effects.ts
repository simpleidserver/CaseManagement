import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { ActionTypes, CreateBpmnInstance, StartBpmnInstance, SearchBpmnInstances, GetBpmnInstance } from '../actions/bpmn-instances.actions';
import { BpmnInstancesService } from '../services/bpmninstances.service';

@Injectable()
export class BpmnInstancesEffects {
    constructor(
        private actions$: Actions,
        private bpmnInstancesService: BpmnInstancesService
    ) { }

    @Effect()
    getBpmnInstance$ = this.actions$
        .pipe(
            ofType(ActionTypes.GET_BPMNINSTANCE),
            mergeMap((evt: GetBpmnInstance) => {
                return this.bpmnInstancesService.get(evt.id)
                    .pipe(
                        map(content => { return { type: ActionTypes.COMPLETE_GET_BPMNINSTANCE, content: content }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_GET_BPMNINSTANCE }))
                    );
            }
            )
        );

    @Effect()
    createBpmnInstance$ = this.actions$
        .pipe(
            ofType(ActionTypes.CREATE_BPMNINSTANCE),
            mergeMap((evt: CreateBpmnInstance) => {
                return this.bpmnInstancesService.create(evt.processFileId)
                    .pipe(
                        map(content => { return { type: ActionTypes.COMPLETE_CREATE_BPMN_INSTANCE, content: content }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_CREATE_BPMNINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    startBpmnInstance$ = this.actions$
        .pipe(
            ofType(ActionTypes.START_BPMNINSTANCE),
            mergeMap((evt: StartBpmnInstance) => {
                return this.bpmnInstancesService.start(evt.id)
                    .pipe(
                        map(() => { return { type: ActionTypes.COMPLETE_START_BPMNINSTANCE }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_START_BPMNINSTANCE }))
                    );
            }
            )
    );

    @Effect()
    searchBpmnInstances$ = this.actions$
        .pipe(
            ofType(ActionTypes.SEARCH_BPMNINSTANCES),
            mergeMap((evt: SearchBpmnInstances) => {
                return this.bpmnInstancesService.search(evt.startIndex, evt.count, evt.order, evt.direction, evt.processFileId)
                    .pipe(
                        map((content) => { return { type: ActionTypes.COMPLETE_SEARCH_BPMNINSTANCES, content: content }; }),
                        catchError(() => of({ type: ActionTypes.ERROR_SEARCH_BPMNINSTANCES }))
                    );
            }
            )
        );
}