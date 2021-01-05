import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCaseActions from '@app/stores/cases/actions/cases.actions';
import { GetCase } from '@app/stores/cases/actions/cases.actions';
import { CaseInstance } from '@app/stores/cases/models/caseinstance.model';
import { CasePlanItemInstance } from '@app/stores/cases/models/caseplaniteminstance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { WorkerTask } from '../../stores/cases/models/workertask.model';

@Component({
    selector: 'view-case-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewCaseComponent implements OnInit, OnDestroy {
    caseInstance: CaseInstance = new CaseInstance();
    interval: any = null;
    get activeHumanTasks() {
        return this.caseInstance.children.filter((child: CasePlanItemInstance) => {
            return child.state === 'Active' && child.type === 'HUMANTASK';
        });
    }
    get enabledTasks() {
        return this.caseInstance.children.filter((child: CasePlanItemInstance) => {
            return child.state === 'Enabled';
        });
    }
    get disabledTasks() {
        return this.caseInstance.children.filter((child: CasePlanItemInstance) => {
            return child.state === 'Disabled';
        });
    }
    get completedTasks() {
        return this.caseInstance.children.filter((child: CasePlanItemInstance) => {
            return child.state === 'Completed' && child.type === 'HUMANTASK';
        });
    }

    constructor(
        private store: Store<fromAppState.AppState>,
        private activatedRoute: ActivatedRoute,
        private translate: TranslateService,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar) {

    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseActions.ActionTypes.ERROR_COMPLETE))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('CASES.MESSAGES.ERROR_COMPLETE_CASE'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseActions.ActionTypes.COMPLETED))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('CASES.MESSAGES.COMPLETED'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseActions.ActionTypes.ERROR_GET_CASE))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('CASES.MESSAGES.ERROR_GET_CASE'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseActions.ActionTypes.COMPLETE_ACTIVATE))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('CASES.MESSAGES.ACTIVATED'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseActions.ActionTypes.ERROR_ACTIVATE))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('CASES.MESSAGES.ERROR_ACTIVATE'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseActions.ActionTypes.COMPLETE_DISABLE))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('CASES.MESSAGES.DISABLED'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseActions.ActionTypes.ERROR_DISABLE))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('CASES.MESSAGES.ERROR_DISABLE'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseActions.ActionTypes.COMPLETE_REENABLE))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('CASES.MESSAGES.REENABLED'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseActions.ActionTypes.ERROR_REENABLE))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('CASES.MESSAGES.ERROR_REENABLE'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectCaseResult)).subscribe((l: CaseInstance) => {
            if (!l) {
                return;
            }

            this.caseInstance = l;

        });
        this.interval = setInterval(() => {
            this.refresh();
        }, 2000);
        this.refresh();
    }

    ngOnDestroy() {
        if (this.interval) {
            clearInterval(this.interval);
        }
    }

    refresh() {
        const id = this.activatedRoute.snapshot.params['id'];
        const request = new GetCase(id);
        this.store.dispatch(request);
    }

    enableTask(task: CasePlanItemInstance) {
        const id = this.activatedRoute.snapshot.params['id'];
        const request = new fromCaseActions.Activate(id, task.id);
        this.store.dispatch(request);
    }

    disableTask(task: CasePlanItemInstance) {
        const id = this.activatedRoute.snapshot.params['id'];
        const request = new fromCaseActions.Disable(id, task.id);
        this.store.dispatch(request);
    }

    reenableTask(task: CasePlanItemInstance) {
        const id = this.activatedRoute.snapshot.params['id'];
        const request = new fromCaseActions.Reenable(id, task.id);
        this.store.dispatch(request);
    }

    canConfirmTask(task: CasePlanItemInstance) {
        const filtered = this.caseInstance.workerTasks.filter((workerTask: WorkerTask) => {
            return workerTask.casePlanElementInstanceId === task.id;
        });
        return filtered.length !== 1;
    }

    getFormUrl(task: CasePlanItemInstance) {
        const filtered = this.caseInstance.workerTasks.filter((workerTask: WorkerTask) => {
            return workerTask.casePlanElementInstanceId === task.id;
        });
        if (filtered.length !== 1) {
            return '/cases/' + this.caseInstance.id;
        }

        return '/cases/' + this.caseInstance.id + '/' + filtered[0].externalId;
    }

    confirmTask(task : CasePlanItemInstance) {
        const id = this.activatedRoute.snapshot.params['id'];
        const request = new fromCaseActions.Complete(id, task.id);
        this.store.dispatch(request);
    }
}