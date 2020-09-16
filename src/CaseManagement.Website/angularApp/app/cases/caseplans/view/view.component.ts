import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSelectChange, MatSnackBar, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCasePlanInstanceActions from '@app/stores/caseplaninstances/actions/caseplaninstance.actions';
import { CasePlanInstanceResult } from '@app/stores/caseplaninstances/models/caseplaninstance.model';
import { SearchCasePlanInstanceResult } from '@app/stores/caseplaninstances/models/searchcaseplaninstanceresult.model';
import * as fromCasePlanActions from '@app/stores/caseplans/actions/caseplan.actions';
import { CasePlan } from '@app/stores/caseplans/models/caseplan.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-case-files',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCaseDefinitionComponent implements OnInit, OnDestroy {
    selectedTimer: string = "4000";
    casePlan$: CasePlan = new CasePlan();
    casePlanInstances$: CasePlanInstanceResult[] = new Array<CasePlanInstanceResult>();
    displayedColumns: string[] = ['id', 'state', 'create_datetime', 'actions'];
    casePlanInstancesLength: number;
    interval: any;
    @ViewChild('casePlanInstanceSort') casePlanInstanceSort: MatSort;
    @ViewChild('casePlanInstancePaginator') casePlanInstancePaginator: MatPaginator;

    constructor(private casePlanStore: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private snackBar: MatSnackBar) { }

    ngOnInit() {
        this.casePlanStore.pipe(select(fromAppState.selectCasePlanResult)).subscribe((casePlan: CasePlan) => {
            if (!casePlan) {
                return;
            }

            this.casePlan$ = casePlan;
        });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_LAUNCH_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_LAUNCHED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_LAUNCH_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CANNOT_LAUNCH_CASE_PLAN_INSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_REACTIVATE_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_REACTIVATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_REACTIVATE_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CANNOT_REACTIVATE_CASE_PLAN_INSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_SUSPEND_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_SUSPENDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_SUSPEND_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CANNOT_SUSPEND_CASE_PLAN_INSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_RESUME_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_RESUMED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_RESUME_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CANNOT_RESUME_CASE_PLAN_INSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_CLOSE_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_CLOSED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_CLOSE_CASE_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CANNOT_CLOSE_CASE_PLAN_INSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.casePlanStore.pipe(select(fromAppState.selectCasePlanInstanceLstResult)).subscribe((searchCasePlanInstanceResult: SearchCasePlanInstanceResult) => {
            if (!searchCasePlanInstanceResult) {
                return;
            }

            this.casePlanInstances$ = searchCasePlanInstanceResult.content;
            this.casePlanInstancesLength = searchCasePlanInstanceResult.totalLength;
        });
        this.interval = setInterval(() => {
            this.refresh();
        }, 4000);
        this.refresh();
    }

    selectTimer(evt: MatSelectChange) {
        clearInterval(this.interval);
        this.interval = setInterval(() => {
            this.refresh();
        }, evt.value);
    }

    launchCaseInstance() {
        const launchCasePlanInstance = new fromCasePlanInstanceActions.LaunchCasePlanInstance(this.casePlan$.id);
        this.casePlanStore.dispatch(launchCasePlanInstance);
    }

    reactivateCaseInstance(caseInstance: CasePlanInstanceResult) {
        const reactivateCasePlanInstance = new fromCasePlanInstanceActions.ReactivateCasePlanInstance(caseInstance.id);
        this.casePlanStore.dispatch(reactivateCasePlanInstance);
    }

    suspendCaseInstance(caseInstance: CasePlanInstanceResult) {
        const suspendCasePlanInstance = new fromCasePlanInstanceActions.SuspendCasePlanInstance(caseInstance.id);
        this.casePlanStore.dispatch(suspendCasePlanInstance);
    }

    resumeCaseInstance(caseInstance: CasePlanInstanceResult) {
        const suspendCasePlanInstance = new fromCasePlanInstanceActions.ResumeCasePlanInstance(caseInstance.id);
        this.casePlanStore.dispatch(suspendCasePlanInstance);
    }

    closeCaseInstance(caseInstance: CasePlanInstanceResult) {
        const suspendCasePlanInstance = new fromCasePlanInstanceActions.CloseCasePlanInstance(caseInstance.id);
        this.casePlanStore.dispatch(suspendCasePlanInstance);
    }

    ngAfterViewInit() {
        merge(this.casePlanInstanceSort.sortChange, this.casePlanInstancePaginator.page).subscribe(() => this.refreshCaseInstances());
    }

    refresh() {
        this.refreshCaseDefinition();
        this.refreshCaseInstances();
    }

    refreshCaseDefinition() {
        const id = this.route.snapshot.params['id'];
        const loadCaseDefinition = new fromCasePlanActions.StartGet(id);
        this.casePlanStore.dispatch(loadCaseDefinition);
    }

    refreshCaseInstances() {
        let startIndex = 0;
        let count = 5;
        if (this.casePlanInstancePaginator.pageIndex && this.casePlanInstancePaginator.pageSize) {
            startIndex = this.casePlanInstancePaginator.pageIndex * this.casePlanInstancePaginator.pageSize;
        }

        if (this.casePlanInstancePaginator.pageSize) {
            count = this.casePlanInstancePaginator.pageSize;
        }

        let active = "create_datetime";
        let direction = "desc";
        if (this.casePlanInstanceSort.active) {
            active = this.casePlanInstanceSort.active;
        }

        if (this.casePlanInstanceSort.direction) {
            direction = this.casePlanInstanceSort.direction;
        }

        const loadCaseInstances = new fromCasePlanInstanceActions.SearchCasePlanInstances(startIndex, count, active, direction, this.route.snapshot.params['id']);
        this.casePlanStore.dispatch(loadCaseInstances);
    }

    ngOnDestroy() {
        clearInterval(this.interval);
    }
}