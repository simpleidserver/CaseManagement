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
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
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
        this.id = '';
        this.potentialOwners = [];
        this.excludedOwners = [];
        this.taskInitiators = [];
        this.taskStakeHolders = [];
        this.businessAdministrators = [];
        this.recipients = [];
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
            _this.potentialOwners = HumanTaskDef.getPotentialOwners(e);
            _this.excludedOwners = HumanTaskDef.getExcludedOwners(e);
            _this.taskInitiators = HumanTaskDef.getTaskInitiators(e);
            _this.taskStakeHolders = HumanTaskDef.getTaskStakeHolders(e);
            _this.businessAdministrators = HumanTaskDef.getBusinessAdministrators(e);
            _this.recipients = HumanTaskDef.getRecipients(e);
        });
    };
    ViewTaskPeopleAssignmentComponent.prototype.update = function () {
        var peopleAssignments = [];
        peopleAssignments = peopleAssignments.concat(this.potentialOwners, this.excludedOwners, this.taskInitiators, this.businessAdministrators, this.recipients);
        var request = new fromHumanTaskDefActions.UpdatePeopleAssignmentOperation(this.id, peopleAssignments);
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