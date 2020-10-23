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
    addStartDeadline = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_START_DEADLINE),
            mergeMap((evt: fromHumanTask.AddStartDeadLine) => {
                return this.humanTaskDefService.addStartDeadline(evt.id, evt.deadLine)
                    .pipe(
                        map((deadLine) => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_START_DEADLINE, content: deadLine }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_START_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    addCompletionDeadline = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_COMPLETION_DEADLINE),
            mergeMap((evt: fromHumanTask.AddCompletionDeadLine) => {
                return this.humanTaskDefService.addCompletionDeadline(evt.id, evt.deadLine)
                    .pipe(
                        map((deadLine) => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE, content: deadLine }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_COMPLETION_DEADLINE }))
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
    addInputParameterAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_OPERATION_INPUT_PARAMETER),
            mergeMap((evt: fromHumanTask.AddInputParameterOperation) => {
                return this.humanTaskDefService.addInputParameter(evt.id, evt.parameter)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER, parameter: evt.parameter }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_OPERATION_INPUT_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    addOutputParameterAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_OPERATION_OUTPUT_PARAMETER),
            mergeMap((evt: fromHumanTask.AddOutputParameterOperation) => {
                return this.humanTaskDefService.addOutputParameter(evt.id, evt.parameter)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER, parameter: evt.parameter }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_OPERATION_OUTPUT_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    deleteInputParameterAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_OPERATION_INPUT_PARAMETER),
            mergeMap((evt: fromHumanTask.DeleteInputParameterOperation) => {
                return this.humanTaskDefService.deleteInputParameter(evt.id, evt.name)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER, name: evt.name }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_OPERATION_INPUT_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    deleteOutputParameterAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_OPERATION_OUTPUT_PARAMETER),
            mergeMap((evt: fromHumanTask.DeleteInputParameterOperation) => {
                return this.humanTaskDefService.deleteOutputParameter(evt.id, evt.name)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER, name: evt.name }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_OPERATION_OUTPUT_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    updateRenderingAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_RENDERING_PARAMETER),
            mergeMap((evt: fromHumanTask.UpdateRenderingOperation) => {
                return this.humanTaskDefService.updateRendering(evt.id, evt.rendering)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER, rendering: evt.rendering}; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_RENDERING_PARAMETER }))
                    );
            }
            )
    );

    @Effect()
    deleteStartDealineAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_START_DEADLINE),
            mergeMap((evt: fromHumanTask.DeleteStartDeadlineOperation) => {
                return this.humanTaskDefService.deleteStartDeadline(evt.id, evt.deadLineId)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_START_DEALINE, deadLineId: evt.deadLineId}; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_START_DEALINE }))
                    );
            }
            )
    );

    @Effect()
    deleteCompletionDeadlineAction = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_COMPLETION_DEADLINE),
            mergeMap((evt: fromHumanTask.DeleteStartDeadlineOperation) => {
                return this.humanTaskDefService.deleteCompletionDeadline(evt.id, evt.deadLineId)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_COMPLETION_DEADLINE, deadLineId: evt.deadLineId }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    updateStartDealine = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_START_DEADLINE),
            mergeMap((evt: fromHumanTask.UpdateStartDeadlineOperation) => {
                return this.humanTaskDefService.updateStartDealine(evt.id, evt.deadline)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_START_DEADLINE, deadline: evt.deadline }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_START_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    updateCompletionDeadline = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_COMPLETION_DEADLINE),
            mergeMap((evt: fromHumanTask.UpdateCompletionDeadlineOperation) => {
                return this.humanTaskDefService.updateCompletionDealine(evt.id, evt.deadline)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_COMPLETION_DEADLINE, deadline: evt.deadline }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_COMPLETION_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    addEscalationStartDeadline = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_ESCALATION_STARTDEADLINE),
            mergeMap((evt: fromHumanTask.AddEscalationStartDeadlineOperation) => {
                return this.humanTaskDefService.addEscalationStartDeadline(evt.id, evt.deadlineId, evt.condition)
                    .pipe(
                        map((escId) => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_ESCALATION_STARTDEADLINE, deadlineId: evt.deadlineId, condition: evt.condition, escId: escId }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_ESCALATION_STARTDEADLINE }))
                    );
            }
            )
    );

    @Effect()
    addEscalationCompletionDeadline = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.ADD_ESCALATION_COMPLETIONDEADLINE),
            mergeMap((evt: fromHumanTask.AddEscalationCompletionDeadlineOperation) => {
                return this.humanTaskDefService.addEscalationCompletionDeadline(evt.id, evt.deadlineId, evt.condition)
                    .pipe(
                        map((escId) => { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE, deadlineId: evt.deadlineId, condition: evt.condition, escId: escId }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_ADD_ESCALATION_COMPLETIONDEADLINE }))
                    );
            }
            )
    );

    @Effect()
    updatePeopleAssignment = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_PEOPLE_ASSIGNMENT),
            mergeMap((evt: fromHumanTask.UpdatePeopleAssignmentOperation) => {
                return this.humanTaskDefService.updatePeopleAssignment(evt.id, evt.assignment)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT, assignment: evt.assignment }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_PEOPLE_ASSIGNMENT }))
                    );
            }
            )
    );

    @Effect()
    updateStartEscalation = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_START_ESCALATION),
            mergeMap((evt: fromHumanTask.UpdateStartEscalationOperation) => {
                return this.humanTaskDefService.updateStartEscalation(evt.id, evt.deadLineId, evt.escalation)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_START_ESCALATION, deadLineId: evt.deadLineId, escalation: evt.escalation }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_START_ESCALATION }))
                    );
            }
            )
    );

    @Effect()
    updateCompletionEscalation = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.UPDATE_COMPLETION_ESCALATION),
            mergeMap((evt: fromHumanTask.UpdateCompletionEscalationOperation) => {
                return this.humanTaskDefService.updateCompletionEscalation(evt.id, evt.deadLineId, evt.escalation)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_COMPLETION_ESCALATION, deadLineId: evt.deadLineId, escalation: evt.escalation }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_COMPLETION_ESCALATION }))
                    );
            }
            )
    );

    @Effect()
    deleteCompletionEscalation = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_COMPLETION_ESCALATION),
            mergeMap((evt: fromHumanTask.DeleteCompletionEscalationOperation) => {
                return this.humanTaskDefService.deleteCompletionEscalation(evt.id, evt.deadLineId, evt.escalation)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_COMPLETION_ESCALATION, deadLineId: evt.deadLineId, escalation: evt.escalation }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE }))
                    );
            }
            )
    );

    @Effect()
    deleteStartEscalation = this.actions$
        .pipe(
            ofType(fromHumanTask.ActionTypes.DELETE_START_ESCALATION),
            mergeMap((evt: fromHumanTask.DeleteCompletionEscalationOperation) => {
                return this.humanTaskDefService.deleteStartEscalation(evt.id, evt.deadLineId, evt.escalation)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_START_ESCALATION, deadLineId: evt.deadLineId, escalation: evt.escalation }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_START_ESCALATION }))
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
                return this.humanTaskDefService.updatePresentationElement(evt.id, evt.presentationElement)
                    .pipe(
                        map(() => { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT, presentationElement: evt.presentationElement }; }),
                        catchError(() => of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_PRESENTATIONELEMENT }))
                    );
            }
            )
        );
}