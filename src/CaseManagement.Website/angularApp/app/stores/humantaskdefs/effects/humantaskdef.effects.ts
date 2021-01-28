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

    @Effect()
    updateHumanTaskDef = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_HUMANASKDEF),
            mergeMap((evt: fromHumanTask.UpdateHumanTaskDef) => {
                return this.humanTaskDefService.update(evt.humanTaskDef)
                    .pipe(
                        map(humanTaskDef => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_HUMANASKDEF, content: humanTaskDef }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_HUMANASKDEF }))
                    );
            }
            )
    );

    @Effect()
    addDeadline = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_DEADLINE),
            mergeMap((evt: fromHumanTask.AddDeadline) => {
                return this.humanTaskDefService.addDeadline(evt.id, evt.deadLine)
                    .pipe(
                        map((deadLine) => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_DEADLINE, content: deadLine }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    updateHumanTaskDefInfo = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_HUMANTASKDEF_INFO),
            mergeMap((evt: fromHumanTask.UpdateHumanTaskInfo) => {
                return this.humanTaskDefService.updateInfo(evt.id, evt.name, evt.priority)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO, name: evt.name, priority: evt.priority }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_HUMANTASK_INFO }))
                    );
            }
            )
        );

    @Effect()
    addParameterAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_OPERATION_PARAMETER),
            mergeMap((evt: fromHumanTask.AddOperationParameterOperation) => {
                return this.humanTaskDefService.addParameter(evt.id, evt.parameter)
                    .pipe(
                        map((str) => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_OPERATION_PARAMETER, id: str, parameter: evt.parameter }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_OPERATION_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    deleteParameterAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_OPERATION_PARAMETER),
            mergeMap((evt: fromHumanTask.DeleteOperationParameterOperation) => {
                return this.humanTaskDefService.deleteParameter(evt.id, evt.parameterId)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_OPERATION_PARAMETER, parameterId: evt.parameterId }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_OPERATION_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    updateRenderingAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_RENDERING_PARAMETER),
            mergeMap((evt: fromHumanTask.UpdateRenderingOperation) => {
                return this.humanTaskDefService.updateRendering(evt.id, evt.renderingElements)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER, renderingElements: evt.renderingElements}; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_RENDERING_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    deleteDealineAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_DEADLINE),
            mergeMap((evt: fromHumanTask.DeleteDeadlineOperation) => {
                return this.humanTaskDefService.deleteDeadline(evt.id, evt.deadLineId)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_DEADLINE, deadLineId: evt.deadLineId}; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    updateDealine = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_DEADLINE),
            mergeMap((evt: fromHumanTask.UpdateDeadlineOperation) => {
                return this.humanTaskDefService.updateDealine(evt.id, evt.deadline)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_DEADLINE, deadline: evt.deadline }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    addEscalationDeadline = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_ESCALATION_DEADLINE),
            mergeMap((evt: fromHumanTask.AddEscalationDeadlineOperation) => {
                return this.humanTaskDefService.addEscalationDeadline(evt.id, evt.deadlineId, evt.condition, evt.notificationId)
                    .pipe(
                        map((escId) => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_ESCALATION_DEADLINE, deadlineId: evt.deadlineId, condition: evt.condition, escId: escId, notificationId: evt.notificationId }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_ESCALATION_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    updateEscalation = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_ESCALATION),
            mergeMap((evt: fromHumanTask.UpdateEscalationOperation) => {
                return this.humanTaskDefService.updateEscalation(evt.id, evt.deadLineId, evt.escalationId, evt.condition, evt.notificationId)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_ESCALATION, id: evt.id, deadLineId: evt.deadLineId, escalationId: evt.escalationId, condition: evt.condition, notificationId: evt.notificationId }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_ESCALATION }))
                    );
            }
            )
    );

    @Effect()
    deleteEscalation = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_ESCALATION),
            mergeMap((evt: fromHumanTask.DeleteEscalationOperation) => {
                return this.humanTaskDefService.deleteEscalation(evt.id, evt.deadLineId, evt.escalationId)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_ESCALATION, deadLineId: evt.deadLineId, escalationId: evt.escalationId }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    addHumanTaskDef = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_HUMANTASKEF),
            mergeMap((evt: fromHumanTask.AddHumanTaskDefOperation) => {
                return this.humanTaskDefService.addHumanTask(evt.name)
                    .pipe(
                        map((humanTaskDef) => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_HUMANTASKDEF, humanTaskDef: humanTaskDef }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_HUMANTASKDEF }))
                    );
            }
            )
    );

    @Effect()
    search = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.SEARCH_HUMANTASKDEFS),
            mergeMap((evt: fromHumanTask.SearchHumanTaskDefOperation) => {
                return this.humanTaskDefService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                    .pipe(
                        map((humanTaskDefsResult) => { return { type: fromHumanTask.ActionTypes.COMPLETE_SEARCH_HUMANTASKDEFS, humanTaskDefsResult: humanTaskDefsResult }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_SEARCH_HUMANTASKDEFS }))
                    );
            }
            )
    );

    @Effect()
    updatePresentationElement = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_PRESENTATIONELEMENT),
            mergeMap((evt: fromHumanTask.UpdatePresentationElementOperation) => {
                return this.humanTaskDefService.updatePresentationElement(evt.id, evt.presentationElements, evt.presentationParameters)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT, presentationElements: evt.presentationElements, presentationParameters: evt.presentationParameters }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_PRESENTATIONELEMENT }))
                    );
            }
            )
    );

    @Effect()
    addPresentationParameter = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_PRESENTATION_PARAMETER),
            mergeMap((evt: fromHumanTask.AddPresentationParameter) => {
                return this.humanTaskDefService.addPresentationParameter(evt.id, evt.presentationParameter)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_PRESENTATION_PARAMETER, presentationParameter: evt.presentationParameter }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_PRESENTATION_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    deletePresentationParameter = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_PRESENTATION_PARAMETER),
            mergeMap((evt: fromHumanTask.AddPresentationParameter) => {
                return this.humanTaskDefService.deletePresentationParameter(evt.id, evt.presentationParameter)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_PRESENTATION_PARAMETER, presentationParameter: evt.presentationParameter }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_PRESENTATION_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    addPresentationElt = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_PRESENTATION_ELT),
            mergeMap((evt: fromHumanTask.AddPresentationElt) => {
                return this.humanTaskDefService.addPresentationElt(evt.id, evt.presentationElt)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_PRESENTATION_ELT, presentationElt: evt.presentationElt }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_PRESENTATION_ELT }))
                    );
            }
            )
    );

    @Effect()
    deletePresentationElt = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_PRESENTATION_ELT),
            mergeMap((evt: fromHumanTask.DeletePresentationElt) => {
                return this.humanTaskDefService.deletePresentationElt(evt.id, evt.presentationElt)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_PRESENTATION_ELT, presentationElt: evt.presentationElt }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_PRESENTATION_ELT }))
                    );
            }
            )
    );

    @Effect()
    addPeopleAssignment = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_PEOPLE_ASSIGNMENT),
            mergeMap((evt: fromHumanTask.AddPeopleAssignment) => {
                return this.humanTaskDefService.addPeopleAssignment(evt.id, evt.peopleAssignment)
                    .pipe(
                        map((id: string) => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_PEOPLE_ASSIGNMENT, peopleAssignment: evt.peopleAssignment, assignmentId: id }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_PEOPLE_ASSIGNMENT }))
                    );
            }
            )
    );

    @Effect()
    deletePeopleAssignment = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_PEOPLE_ASSIGNMENT),
            mergeMap((evt: fromHumanTask.DeletePeopleAssignment) => {
                return this.humanTaskDefService.deletePeopleAssignment(evt.id, evt.peopleAssignment)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_PEOPLE_ASSIGNMENT, peopleAssignment: evt.peopleAssignment }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_PEOPLE_ASSIGNMENT }))
                    );
            }
            )
    );

    @Effect()
    addEscalationToPart = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_ESCALATION_TOPART),
            mergeMap((evt: fromHumanTask.AddToPartEscalation) => {
                return this.humanTaskDefService.addEscalationToPart(evt.id, evt.deadlineId, evt.escalationId, evt.toPart)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_ESCALATION_TOPART, id: evt.id, deadlineId: evt.deadlineId, escalationId: evt.escalationId, toPart: evt.toPart }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_ESCALATION_TOPART }))
                    );
            }
            )
    );

    @Effect()
    deleteEscalationToPart$ = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_ESCALATION_TOPART),
            mergeMap((evt: fromHumanTask.DeleteToPartEscalation) => {
                return this.humanTaskDefService.deleteEscalationToPart(evt.id, evt.deadlineId, evt.escalationId, evt.toPartName)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_ESCALATION_TOPART, id: evt.id, deadlineId: evt.deadlineId, escalationId: evt.escalationId, toPartName: evt.toPartName }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_ESCALATION_TOPART }))
                    );
            }
            )
        );
}