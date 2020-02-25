import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSelectChange, MatSort, MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import * as fromCaseInstance from '../actions/caseinstance';
import * as fromCasePlan from '../actions/caseplan';
import * as fromCaseWorker from '../actions/caseworker';
import * as fromFormInstance from '../actions/forminstance';
import { CasePlan } from '../models/caseplan.model';
import { CasePlanInstance } from '../models/caseplaninstance.model';
import { FormInstance } from '../models/forminstance.model';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';
import { SearchFormInstanceResult } from '../models/searchforminstanceresult.model';
import { SearchWorkerTaskResult } from '../models/searchworkertaskresult.model';
import { WorkerTask } from '../models/workertask.model';
import * as fromCasePlanDefinitions from '../reducers';
import { CasePlanService } from '../services/caseplan.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'view-case-files',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCaseDefinitionComponent implements OnInit, OnDestroy {
    selectedTimer: string = "4000";
    casePlan$: CasePlan = new CasePlan();
    casePlanInstances$: CasePlanInstance[] = new Array<CasePlanInstance>();
    formInstances$: FormInstance[] = new Array<FormInstance>();
    workerTasks$: WorkerTask[] = new Array<WorkerTask>();
    displayedColumns: string[] = ['id', 'state', 'create_datetime', 'actions'];
    formInstanceDisplayedColumns: string[] = ['form_id', 'performer', 'status', 'update_datetime', 'create_datetime'];
    workerTaskDisplayedColumns: string[] = ['type', 'status', 'performer', 'create_datetime', 'update_datetime'];
    casePlanInstancesLength: number;
    formInstancesLength: number;
    workerTasksLength: number;
    interval: any;
    @ViewChild('casePlanInstanceSort') casePlanInstanceSort: MatSort;
    @ViewChild('formInstanceSort') formInstanceSort: MatSort;
    @ViewChild('workerTaskSort') workerTaskSort: MatSort;
    @ViewChild('casePlanInstancePaginator') casePlanInstancePaginator: MatPaginator;
    @ViewChild('formInstancePaginator') formInstancePaginator: MatPaginator;
    @ViewChild('caseWorkerPaginator') caseWorkerPaginator: MatPaginator;

    constructor(private casePlanStore: Store<fromCasePlanDefinitions.CasePlanState>, private route: ActivatedRoute, private casePlanService: CasePlanService, private translateService: TranslateService, private snackBar: MatSnackBar) {  }

    ngOnInit() {
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectGetResult)).subscribe((casePlan: CasePlan) => {
            if (!casePlan) {
                return;
            }

            this.casePlan$ = casePlan;
        });
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectSearchInstanceResult)).subscribe((searchCasePlanInstanceResult: SearchCasePlanInstanceResult) => {
            if (!searchCasePlanInstanceResult) {
                return;
            }

            this.casePlanInstances$ = searchCasePlanInstanceResult.Content;
            this.casePlanInstancesLength = searchCasePlanInstanceResult.TotalLength;
        });
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectSearchFormInstancesResult)).subscribe((searchFormInstanceResult: SearchFormInstanceResult) => {
            if (!searchFormInstanceResult) {
                return;
            }

            this.formInstances$ = searchFormInstanceResult.Content;
            this.formInstancesLength = searchFormInstanceResult.TotalLength;
        });
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectSearchCaseWorkerResult)).subscribe((searchWorkerTaskResult: SearchWorkerTaskResult) => {
            if (!searchWorkerTaskResult) {
                return;
            }

            this.workerTasks$ = searchWorkerTaskResult.Content;
            this.workerTasksLength = searchWorkerTaskResult.TotalLength;
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
        this.casePlanService.launchCasePlanInstance(this.route.snapshot.params['id']).subscribe(() => {
            this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_LAUNCHED'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, () => {
            this.snackBar.open(this.translateService.instant('CANNOT_LAUNCH_CASE_PLAN_INSTANCE'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    }

    reactivateCaseInstance(caseInstance: CasePlanInstance) {
        this.casePlanService.reactivateCasePlanInstance(this.route.snapshot.params['id'], caseInstance.Id).subscribe(() => {
            this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_REACTIVATED'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, () => {
            this.snackBar.open(this.translateService.instant('CANNOT_REACTIVATE_CASE_PLAN_INSTANCE'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    }

    suspendCaseInstance(caseInstance: CasePlanInstance) {
        this.casePlanService.suspendCasePlanInstance(this.route.snapshot.params['id'], caseInstance.Id).subscribe(() => {
            this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_SUSPENDED'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, () => {
            this.snackBar.open(this.translateService.instant('CANNOT_SUSPEND_CASE_PLAN_INSTANCE'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    }

    resumeCaseInstance(caseInstance: CasePlanInstance) {
        this.casePlanService.resumeCasePlanInstance(this.route.snapshot.params['id'], caseInstance.Id).subscribe(() => {
            this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_RESUMED'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, () => {
            this.snackBar.open(this.translateService.instant('CANNOT_RESUME_CASE_PLAN_INSTANCE'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    }

    closeCaseInstance(caseInstance: CasePlanInstance) {
        this.casePlanService.closeCasePlanInstance(this.route.snapshot.params['id'], caseInstance.Id).subscribe(() => {
            this.snackBar.open(this.translateService.instant('CASE_PLAN_INSTANCE_CLOSED'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, () => {
            this.snackBar.open(this.translateService.instant('CANNOT_CLOSE_CASE_PLAN_INSTANCE'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    }

    ngAfterViewInit() {
        merge(this.casePlanInstanceSort.sortChange, this.casePlanInstancePaginator.page).subscribe(() => this.refreshCaseInstances());
        merge(this.formInstanceSort.sortChange, this.formInstancePaginator.page).subscribe(() => this.refreshFormInstances());
        merge(this.workerTaskSort.sortChange, this.caseWorkerPaginator.page).subscribe(() => this.refreshWorkerTasks());
    }

    refresh() {
        this.refreshCaseDefinition();
        this.refreshCaseInstances();
        this.refreshFormInstances();
        this.refreshWorkerTasks();
    }

    refreshCaseDefinition() {
        var id = this.route.snapshot.params['id'];
        let loadCaseDefinition = new fromCasePlan.StartGet(id);
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

        let loadCaseInstances = new fromCaseInstance.StartSearch(this.route.snapshot.params['id'], startIndex, count, active, direction);
        this.casePlanStore.dispatch(loadCaseInstances);
    }

    refreshFormInstances() {
        let startIndex = 0;
        let count = 5;
        if (this.formInstancePaginator.pageSize) {
            count = this.formInstancePaginator.pageSize;
        }

        if (this.formInstancePaginator.pageIndex && this.formInstancePaginator.pageSize) {
            startIndex = this.formInstancePaginator.pageIndex * this.formInstancePaginator.pageSize;
        }

        let active = "create_datetime";
        let direction = "desc";
        if (this.formInstanceSort.active) {
            active = this.formInstanceSort.active;
        }

        if (this.formInstanceSort.direction) {
            direction = this.formInstanceSort.direction;
        }

        let loadFormInstances = new fromFormInstance.StartSearch(this.route.snapshot.params['id'], active, direction, count, startIndex);
        this.casePlanStore.dispatch(loadFormInstances);
    }

    refreshWorkerTasks() {
        let count = 5;
        let startIndex = 0;
        if (this.caseWorkerPaginator.pageSize) {
            count = this.caseWorkerPaginator.pageSize;
        }

        if (this.caseWorkerPaginator.pageIndex && this.caseWorkerPaginator.pageSize) {
            startIndex = this.caseWorkerPaginator.pageIndex * this.caseWorkerPaginator.pageSize;
        }

        let active = "create_datetime";
        let direction = "desc";
        if (this.workerTaskSort.active) {
            active = this.workerTaskSort.active;
        }

        if (this.workerTaskSort.direction) {
            direction = this.workerTaskSort.direction;
        }

        let loadCaseWorker = new fromCaseWorker.StartSearch(this.route.snapshot.params['id'], active, direction, count, startIndex);
        this.casePlanStore.dispatch(loadCaseWorker);
    }

    ngOnDestroy() {
        clearInterval(this.interval);
    }
}