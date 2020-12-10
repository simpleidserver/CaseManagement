var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as fromHumanTask from '../actions/humantaskdef.actions';
import { HumanTaskDefService } from '../services/humantaskdef.service';
var HumanTaskDefEffects = (function () {
    function HumanTaskDefEffects(actions$, humanTaskDefService) {
        var _this = this;
        this.actions$ = actions$;
        this.humanTaskDefService = humanTaskDefService;
        this.getHumanTaskDef = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.START_GET_HUMANTASKDEF), mergeMap(function (evt) {
            return _this.humanTaskDefService.get(evt.id)
                .pipe(map(function (humanTaskDef) { return { type: fromHumanTask.ActionTypes.COMPLETE_GET_HUMANTASKDEF, content: humanTaskDef }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_GET_HUMANTASKDEF }); }));
        }));
        this.updateHumanTaskDef = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_HUMANASKDEF), mergeMap(function (evt) {
            return _this.humanTaskDefService.update(evt.humanTaskDef)
                .pipe(map(function (humanTaskDef) { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_HUMANASKDEF, content: humanTaskDef }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_HUMANASKDEF }); }));
        }));
        this.addStartDeadline = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.ADD_START_DEADLINE), mergeMap(function (evt) {
            return _this.humanTaskDefService.addStartDeadline(evt.id, evt.deadLine)
                .pipe(map(function (deadLine) { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_START_DEADLINE, content: deadLine }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_ADD_START_DEADLINE }); }));
        }));
        this.addCompletionDeadline = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.ADD_COMPLETION_DEADLINE), mergeMap(function (evt) {
            return _this.humanTaskDefService.addCompletionDeadline(evt.id, evt.deadLine)
                .pipe(map(function (deadLine) { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE, content: deadLine }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_ADD_COMPLETION_DEADLINE }); }));
        }));
        this.updateHumanTaskDefInfo = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_HUMANTASKDEF_INFO), mergeMap(function (evt) {
            return _this.humanTaskDefService.updateInfo(evt.id, evt.name, evt.priority)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO, name: evt.name, priority: evt.priority }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_HUMANTASK_INFO }); }));
        }));
        this.addInputParameterAction = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.ADD_OPERATION_INPUT_PARAMETER), mergeMap(function (evt) {
            return _this.humanTaskDefService.addInputParameter(evt.id, evt.parameter)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_OPERATION_INPUT_PARAMETER, parameter: evt.parameter }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_ADD_OPERATION_INPUT_PARAMETER }); }));
        }));
        this.addOutputParameterAction = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.ADD_OPERATION_OUTPUT_PARAMETER), mergeMap(function (evt) {
            return _this.humanTaskDefService.addOutputParameter(evt.id, evt.parameter)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_OPERATION_OUTPUT_PARAMETER, parameter: evt.parameter }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_ADD_OPERATION_OUTPUT_PARAMETER }); }));
        }));
        this.deleteInputParameterAction = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.DELETE_OPERATION_INPUT_PARAMETER), mergeMap(function (evt) {
            return _this.humanTaskDefService.deleteInputParameter(evt.id, evt.name)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_OPERATION_INPUT_PARAMETER, name: evt.name }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_OPERATION_INPUT_PARAMETER }); }));
        }));
        this.deleteOutputParameterAction = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.DELETE_OPERATION_OUTPUT_PARAMETER), mergeMap(function (evt) {
            return _this.humanTaskDefService.deleteOutputParameter(evt.id, evt.name)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_OPERATION_OUTPUT_PARAMETER, name: evt.name }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_OPERATION_OUTPUT_PARAMETER }); }));
        }));
        this.updateRenderingAction = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_RENDERING_PARAMETER), mergeMap(function (evt) {
            return _this.humanTaskDefService.updateRendering(evt.id, evt.renderingElements)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_RENDERING_PARAMETER, renderingElements: evt.renderingElements }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_RENDERING_PARAMETER }); }));
        }));
        this.deleteStartDealineAction = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.DELETE_START_DEADLINE), mergeMap(function (evt) {
            return _this.humanTaskDefService.deleteStartDeadline(evt.id, evt.deadLineId)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_START_DEALINE, deadLineId: evt.deadLineId }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_START_DEALINE }); }));
        }));
        this.deleteCompletionDeadlineAction = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.DELETE_COMPLETION_DEADLINE), mergeMap(function (evt) {
            return _this.humanTaskDefService.deleteCompletionDeadline(evt.id, evt.deadLineId)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_COMPLETION_DEADLINE, deadLineId: evt.deadLineId }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE }); }));
        }));
        this.updateStartDealine = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_START_DEADLINE), mergeMap(function (evt) {
            return _this.humanTaskDefService.updateStartDealine(evt.id, evt.deadline)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_START_DEADLINE, deadline: evt.deadline }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_START_DEADLINE }); }));
        }));
        this.updateCompletionDeadline = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_COMPLETION_DEADLINE), mergeMap(function (evt) {
            return _this.humanTaskDefService.updateCompletionDealine(evt.id, evt.deadline)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_COMPLETION_DEADLINE, deadline: evt.deadline }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_COMPLETION_DEADLINE }); }));
        }));
        this.addEscalationStartDeadline = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.ADD_ESCALATION_STARTDEADLINE), mergeMap(function (evt) {
            return _this.humanTaskDefService.addEscalationStartDeadline(evt.id, evt.deadlineId, evt.condition)
                .pipe(map(function (escId) { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_ESCALATION_STARTDEADLINE, deadlineId: evt.deadlineId, condition: evt.condition, escId: escId }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_ADD_ESCALATION_STARTDEADLINE }); }));
        }));
        this.addEscalationCompletionDeadline = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.ADD_ESCALATION_COMPLETIONDEADLINE), mergeMap(function (evt) {
            return _this.humanTaskDefService.addEscalationCompletionDeadline(evt.id, evt.deadlineId, evt.condition)
                .pipe(map(function (escId) { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_ESCALATION_COMPLETIONDEADLINE, deadlineId: evt.deadlineId, condition: evt.condition, escId: escId }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_ADD_ESCALATION_COMPLETIONDEADLINE }); }));
        }));
        this.updatePeopleAssignment = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_PEOPLE_ASSIGNMENT), mergeMap(function (evt) {
            return _this.humanTaskDefService.updatePeopleAssignment(evt.id, evt.peopleAssignments)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT, peopleAssignments: evt.peopleAssignments }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_PEOPLE_ASSIGNMENT }); }));
        }));
        this.updateStartEscalation = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_START_ESCALATION), mergeMap(function (evt) {
            return _this.humanTaskDefService.updateStartEscalation(evt.id, evt.deadLineId, evt.escalation)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_START_ESCALATION, deadLineId: evt.deadLineId, escalation: evt.escalation }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_START_ESCALATION }); }));
        }));
        this.updateCompletionEscalation = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_COMPLETION_ESCALATION), mergeMap(function (evt) {
            return _this.humanTaskDefService.updateCompletionEscalation(evt.id, evt.deadLineId, evt.escalation)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_COMPLETION_ESCALATION, deadLineId: evt.deadLineId, escalation: evt.escalation }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_COMPLETION_ESCALATION }); }));
        }));
        this.deleteCompletionEscalation = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.DELETE_COMPLETION_ESCALATION), mergeMap(function (evt) {
            return _this.humanTaskDefService.deleteCompletionEscalation(evt.id, evt.deadLineId, evt.escalation)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_COMPLETION_ESCALATION, deadLineId: evt.deadLineId, escalation: evt.escalation }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_COMPLETION_DEADLINE }); }));
        }));
        this.deleteStartEscalation = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.DELETE_START_ESCALATION), mergeMap(function (evt) {
            return _this.humanTaskDefService.deleteStartEscalation(evt.id, evt.deadLineId, evt.escalation)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_DELETE_START_ESCALATION, deadLineId: evt.deadLineId, escalation: evt.escalation }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_DELETE_START_ESCALATION }); }));
        }));
        this.addHumanTaskDef = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.ADD_HUMANTASKEF), mergeMap(function (evt) {
            return _this.humanTaskDefService.addHumanTask(evt.name)
                .pipe(map(function (humanTaskDef) { return { type: fromHumanTask.ActionTypes.COMPLETE_ADD_HUMANTASKDEF, humanTaskDef: humanTaskDef }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_ADD_HUMANTASKDEF }); }));
        }));
        this.search = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.SEARCH_HUMANTASKDEFS), mergeMap(function (evt) {
            return _this.humanTaskDefService.search(evt.startIndex, evt.count, evt.order, evt.direction)
                .pipe(map(function (humanTaskDefsResult) { return { type: fromHumanTask.ActionTypes.COMPLETE_SEARCH_HUMANTASKDEFS, humanTaskDefsResult: humanTaskDefsResult }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_SEARCH_HUMANTASKDEFS }); }));
        }));
        this.updatePresentationElement = this.actions$
            .pipe(ofType(fromHumanTask.ActionTypes.UPDATE_PRESENTATIONELEMENT), mergeMap(function (evt) {
            return _this.humanTaskDefService.updatePresentationElement(evt.id, evt.presentationElements, evt.presentationParameters)
                .pipe(map(function () { return { type: fromHumanTask.ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT, presentationElements: evt.presentationElements, presentationParameters: evt.presentationParameters }; }), catchError(function () { return of({ type: fromHumanTask.ActionTypes.ERROR_UPDATE_PRESENTATIONELEMENT }); }));
        }));
    }
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "getHumanTaskDef", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updateHumanTaskDef", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "addStartDeadline", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "addCompletionDeadline", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updateHumanTaskDefInfo", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "addInputParameterAction", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "addOutputParameterAction", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "deleteInputParameterAction", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "deleteOutputParameterAction", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updateRenderingAction", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "deleteStartDealineAction", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "deleteCompletionDeadlineAction", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updateStartDealine", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updateCompletionDeadline", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "addEscalationStartDeadline", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "addEscalationCompletionDeadline", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updatePeopleAssignment", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updateStartEscalation", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updateCompletionEscalation", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "deleteCompletionEscalation", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "deleteStartEscalation", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "addHumanTaskDef", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "search", void 0);
    __decorate([
        Effect(),
        __metadata("design:type", Object)
    ], HumanTaskDefEffects.prototype, "updatePresentationElement", void 0);
    HumanTaskDefEffects = __decorate([
        Injectable(),
        __metadata("design:paramtypes", [Actions,
            HumanTaskDefService])
    ], HumanTaskDefEffects);
    return HumanTaskDefEffects;
}());
export { HumanTaskDefEffects };
//# sourceMappingURL=humantaskdef.effects.js.map