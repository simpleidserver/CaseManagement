import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { PeopleAssignment } from '@app/stores/common/people-assignment.model';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDefAssignment } from '@app/stores/humantaskdefs/models/humantaskdef-assignment.model';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-task-peopleassignment-component',
    templateUrl: './view-peopleassignment.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ViewTaskPeopleAssignmentComponent implements OnInit {
    peopleAssignment: HumanTaskDefAssignment = new HumanTaskDefAssignment();
    id: string = '';
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
            this.peopleAssignment = e.peopleAssignment;
        });
    }

    updatePotentialOwner(evt: PeopleAssignment) {
        this.peopleAssignment.potentialOwner = evt;
    }

    updateExcludedOwner(evt: PeopleAssignment) {
        this.peopleAssignment.excludedOwner = evt;
    }

    updateTaskInitiator(evt: PeopleAssignment) {
        this.peopleAssignment.taskInitiator = evt;
    }

    updateTaskStakeHolder(evt: PeopleAssignment) {
        this.peopleAssignment.taskStakeHolder = evt;
    }

    updateBusinessAdministrator(evt: PeopleAssignment) {
        this.peopleAssignment.businessAdministrator = evt;
    }

    updateRecipient(evt: PeopleAssignment) {
        this.peopleAssignment.recipient = evt;
    }

    update() {
        const request = new fromHumanTaskDefActions.UpdatePeopleAssignmentOperation(this.id, this.peopleAssignment);
        this.store.dispatch(request);
    }
}
