import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { PeopleAssignment } from '@app/stores/common/people-assignment.model';

@Component({
    selector: 'view-task-peopleassignment-component',
    templateUrl: './view-peopleassignment.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ViewTaskPeopleAssignmentComponent implements OnInit {
    id: string = '';
    potentialOwners: PeopleAssignment[] = [];
    excludedOwners: PeopleAssignment[] = [];
    taskInitiators: PeopleAssignment[] = [];
    taskStakeHolders: PeopleAssignment[] = [];
    businessAdministrators: PeopleAssignment[] = [];
    recipients: PeopleAssignment[] = [];
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.PEOPLEASSIGNMENT";

    constructor(
        private store: Store<fromAppState.AppState>,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService) {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.PEOPLE_ASSIGNMENT_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_PEOPLE_ASSIGNMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.CANNOT_UPDATE_PEOPLE_ASSIGNMENT'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
    }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.id = e.id;
            this.potentialOwners = HumanTaskDef.getPotentialOwners(e);
            this.excludedOwners = HumanTaskDef.getExcludedOwners(e);
            this.taskInitiators = HumanTaskDef.getTaskInitiators(e);
            this.taskStakeHolders = HumanTaskDef.getTaskStakeHolders(e);
            this.businessAdministrators = HumanTaskDef.getBusinessAdministrators(e);
            this.recipients = HumanTaskDef.getRecipients(e);
        });
    }

    update() {
        let peopleAssignments: PeopleAssignment[] = [];
        peopleAssignments = peopleAssignments.concat(
            this.potentialOwners,
            this.excludedOwners,
            this.taskInitiators,
            this.businessAdministrators,
            this.recipients);
        const request = new fromHumanTaskDefActions.UpdatePeopleAssignmentOperation(this.id, peopleAssignments);
        this.store.dispatch(request);
    }
}
