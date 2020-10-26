var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDefAssignment } from '@app/stores/humantaskdefs/models/humantaskdef-assignment.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ViewTaskPeopleAssignmentComponent = (function () {
    function ViewTaskPeopleAssignmentComponent(store, snackBar, actions$, translateService) {
        var _this = this;
        this.store = store;
        this.snackBar = snackBar;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.peopleAssignment = new HumanTaskDefAssignment();
        this.id = '';
        this.baseTranslationKey = "HUMANTASK.DEF.VIEW.PEOPLEASSIGNMENT";
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.PEOPLE_ASSIGNMENT_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_PEOPLE_ASSIGNMENT; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.CANNOT_UPDATE_PEOPLE_ASSIGNMENT'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
    }
    ViewTaskPeopleAssignmentComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.id = e.id;
            _this.peopleAssignment = e.peopleAssignment;
        });
    };
    ViewTaskPeopleAssignmentComponent.prototype.updatePotentialOwner = function (evt) {
        this.peopleAssignment.potentialOwner = evt;
    };
    ViewTaskPeopleAssignmentComponent.prototype.updateExcludedOwner = function (evt) {
        this.peopleAssignment.excludedOwner = evt;
    };
    ViewTaskPeopleAssignmentComponent.prototype.updateTaskInitiator = function (evt) {
        this.peopleAssignment.taskInitiator = evt;
    };
    ViewTaskPeopleAssignmentComponent.prototype.updateTaskStakeHolder = function (evt) {
        this.peopleAssignment.taskStakeHolder = evt;
    };
    ViewTaskPeopleAssignmentComponent.prototype.updateBusinessAdministrator = function (evt) {
        this.peopleAssignment.businessAdministrator = evt;
    };
    ViewTaskPeopleAssignmentComponent.prototype.updateRecipient = function (evt) {
        this.peopleAssignment.recipient = evt;
    };
    ViewTaskPeopleAssignmentComponent.prototype.update = function () {
        var request = new fromHumanTaskDefActions.UpdatePeopleAssignmentOperation(this.id, this.peopleAssignment);
        this.store.dispatch(request);
    };
    ViewTaskPeopleAssignmentComponent = __decorate([
        Component({
            selector: 'view-task-peopleassignment-component',
            templateUrl: './view-peopleassignment.component.html',
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store,
            MatSnackBar,
            ScannedActionsSubject,
            TranslateService])
    ], ViewTaskPeopleAssignmentComponent);
    return ViewTaskPeopleAssignmentComponent;
}());
export { ViewTaskPeopleAssignmentComponent };
//# sourceMappingURL=view-peopleassignment.component.js.map