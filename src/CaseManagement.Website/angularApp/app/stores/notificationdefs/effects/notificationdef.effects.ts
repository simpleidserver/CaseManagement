import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromNotification from '../actions/notificationdef.actions';
import { NotificationDefService } from '../services/notificationdef.service';

@Injectable()
export class NotificationDefEffects {
    constructor(
        private actions$: Actions,
        private notificationDefService: NotificationDefService
    ) { }

    @Effect()
    search = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.SEARCH_NOTIFICATIONDEFS),
            mergeMap((evt: fromNotification.SearchNotificationDefOperation) => {
                return this.notificationDefService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map((notificationDefsResult) => { return { type: fromNotification.ActionTypes.COMPLETE_SEARCH_NOTIFICATIONDEFS, notificationDefsResult: notificationDefsResult }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_SEARCH_NOTIFICATIONDEFS }))
                    );
            }
            )
    );

    @Effect()
    addNotificationDef = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.ADD_NOTIFICATIONDEF),
            mergeMap((evt: fromNotification.AddNotificationDefOperation) => {
                return this.notificationDefService.addNotification(evt.name)
                    .pipe(
                        map((notificationDef) => { return { type: fromNotification.ActionTypes.COMPLETE_ADD_NOTIFICATIONDEF, notificationDef: notificationDef }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_ADD_NOTIFICATIONDEF }))
                    );
            }
            )
    );

    @Effect()
    getNotificationDef = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.GET_NOTIFICATIONDEF),
            mergeMap((evt: fromNotification.GetNotificationOperation) => {
                return this.notificationDefService.getNotification(evt.id)
                    .pipe(
                        map((notification) => { return { type: fromNotification.ActionTypes.COMPLETE_GET_NOTIFICATIONDEF, notification: notification }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_GET_NOTIFICATIONDEF }))
                    );
            }
            )
    );

    @Effect()
    updateNotificationDefInfo = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.UPDATE_NOTIFICATION_INFO),
            mergeMap((evt: fromNotification.UpdateNotificationInfo) => {
                return this.notificationDefService.updateInfo(evt.id, evt.name, evt.priority)
                    .pipe(
                        map(() => { return { type: fromNotification.ActionTypes.COMPLETE_UPDATE_NOTIFICATION_INFO, name: evt.name, priority: evt.priority }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_UPDATE_NOTIFICATION_INFO }))
                    );
            }
            )
        );

    @Effect()
    addParameterAction = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.ADD_OPERATION_PARAMETER),
            mergeMap((evt: fromNotification.AddOperationParameterOperation) => {
                return this.notificationDefService.addParameter(evt.id, evt.parameter)
                    .pipe(
                        map((str) => { return { type: fromNotification.ActionTypes.COMPLETE_ADD_OPERATION_PARAMETER, id: str, parameter: evt.parameter }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_DELETE_OPERATION_PARAMETER }))
                    );
            }
            )
        );

    @Effect()
    deleteParameterAction = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.DELETE_OPERATION_PARAMETER),
            mergeMap((evt: fromNotification.DeleteOperationParameterOperation) => {
                return this.notificationDefService.deleteParameter(evt.id, evt.parameterId)
                    .pipe(
                        map(() => { return { type: fromNotification.ActionTypes.COMPLETE_DELETE_OPERATION_PARAMETER, parameterId: evt.parameterId }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_DELETE_OPERATION_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    addPresentationParameter = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.ADD_PRESENTATION_PARAMETER),
            mergeMap((evt: fromNotification.AddPresentationParameter) => {
                return this.notificationDefService.addPresentationParameter(evt.id, evt.presentationParameter)
                    .pipe(
                        map(() => { return { type: fromNotification.ActionTypes.COMPLETE_ADD_PRESENTATION_PARAMETER, presentationParameter: evt.presentationParameter }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_ADD_PRESENTATION_PARAMETER }))
                    );
            }
            )
        );

    @Effect()
    deletePresentationParameter = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.DELETE_PRESENTATION_PARAMETER),
            mergeMap((evt: fromNotification.AddPresentationParameter) => {
                return this.notificationDefService.deletePresentationParameter(evt.id, evt.presentationParameter)
                    .pipe(
                        map(() => { return { type: fromNotification.ActionTypes.COMPLETE_DELETE_PRESENTATION_PARAMETER, presentationParameter: evt.presentationParameter }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_DELETE_PRESENTATION_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    addPresentationElt = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.ADD_PRESENTATION_ELT),
            mergeMap((evt: fromNotification.AddPresentationElt) => {
                return this.notificationDefService.addPresentationElt(evt.id, evt.presentationElt)
                    .pipe(
                        map(() => { return { type: fromNotification.ActionTypes.COMPLETE_ADD_PRESENTATION_ELT, presentationElt: evt.presentationElt }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_ADD_PRESENTATION_ELT }))
                    );
            }
            )
        );

    @Effect()
    deletePresentationElt = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.DELETE_PRESENTATION_ELT),
            mergeMap((evt: fromNotification.DeletePresentationElt) => {
                return this.notificationDefService.deletePresentationElt(evt.id, evt.presentationElt)
                    .pipe(
                        map(() => { return { type: fromNotification.ActionTypes.COMPLETE_DELETE_PRESENTATION_ELT, presentationElt: evt.presentationElt }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_DELETE_PRESENTATION_ELT }))
                    );
            }
            )
        );

    @Effect()
    addPeopleAssignment = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.ADD_PEOPLE_ASSIGNMENT),
            mergeMap((evt: fromNotification.AddPeopleAssignment) => {
                return this.notificationDefService.addPeopleAssignment(evt.id, evt.peopleAssignment)
                    .pipe(
                        map((id: string) => { return { type: fromNotification.ActionTypes.COMPLETE_ADD_PEOPLE_ASSIGNMENT, peopleAssignment: evt.peopleAssignment, assignmentId: id }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_ADD_PEOPLE_ASSIGNMENT }))
                    );
            }
            )
        );

    @Effect()
    deletePeopleAssignment = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.DELETE_PEOPLE_ASSIGNMENT),
            mergeMap((evt: fromNotification.DeletePeopleAssignment) => {
                return this.notificationDefService.deletePeopleAssignment(evt.id, evt.peopleAssignment)
                    .pipe(
                        map(() => { return { type: fromNotification.ActionTypes.COMPLETE_DELETE_PEOPLE_ASSIGNMENT, peopleAssignment: evt.peopleAssignment }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_DELETE_PEOPLE_ASSIGNMENT }))
                    );
            }
            )
    );

    @Effect()
    getAll = this.actions$
        .pipe(
            ofType(fromNotification.ActionTypes.GET_ALL),
            mergeMap(() => {
                return this.notificationDefService.getAll()
                    .pipe(
                        map((notificationDefs) => { return { type: fromNotification.ActionTypes.COMPLETE_GET_ALL, notificationDefs: notificationDefs }; }),
                        catchError(() => of({ type: fromNotification.ActionTypes.ERROR_GET_ALL }))
                    );
            }
            )
        );

}