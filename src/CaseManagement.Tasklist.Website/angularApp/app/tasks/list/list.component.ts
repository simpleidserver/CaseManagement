import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog, MatPaginator, MatSnackBar, MatSort } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { SearchTasksResult } from '@app/stores/tasks/models/search-tasks-result.model';
import { Task } from '@app/stores/tasks/models/task.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { SearchTasks } from '@app/stores/tasks/actions/tasks.actions';
import { NominateParameter } from '@app/stores/tasks/parameters/nominate-parameter';
import { NominateTaskDialogComponent } from './nominate-task-dialog.component';

@Component({
    selector: 'list-tasks-component',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListTasksComponent implements OnInit {
    displayedColumns: string[] = ['priority', 'presentationName', 'presentationSubject', 'actualOwner', 'status', 'createdTime', 'actions'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    searchTasksForm: FormGroup;
    length: number;
    tasks$: Task[] = [];

    constructor(private store: Store<fromAppState.AppState>,
        private translate: TranslateService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject,
        public dialog: MatDialog,
        private formBuilder: FormBuilder) {
        this.searchTasksForm = this.formBuilder.group({
            actualOwner: new FormControl(''),
            status: new FormControl('')
        });
    }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromTaskActions.ActionTypes.COMPLETE_START_TASK))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('TASKS.MESSAGES.TASK_STARTED'), this.translate.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromTaskActions.ActionTypes.ERROR_START_TASK))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('TASKS.MESSAGES.ERROR_START_TASK'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromTaskActions.ActionTypes.COMPLETE_NOMINATE_TASK))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('TASKS.MESSAGES.TASK_NOMINATED'), this.translate.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromTaskActions.ActionTypes.ERROR_NOMINATE_TASK))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('TASKS.MESSAGES.ERROR_NOMINATE_TASK'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromTaskActions.ActionTypes.COMPLETE_CLAIM_TASK))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('TASKS.MESSAGES.TASK_CLAIMED'), this.translate.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromTaskActions.ActionTypes.ERROR_CLAIM_TASK))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('TASKS.MESSAGES.ERROR_CLAIM_TASK'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectTaskLstResult)).subscribe((l: SearchTasksResult) => {
            if (!l || !l.content) {
                return;
            }

            this.tasks$ = l.content;
            this.length = l.totalLength;
        });
        this.activatedRoute.queryParamMap.subscribe((p: any) => {
            this.sort.active = p.get('active');
            this.sort.direction = p.get('direction');
            this.paginator.pageSize = p.get('pageSize');
            this.paginator.pageIndex = p.get('pageIndex');
            const actualOwner = p.get('actualOwner');
            if (actualOwner) {
                this.searchTasksForm.get('actualOwner').setValue(actualOwner);
            }

            const status = p.get('status');
            if (status) {
                this.searchTasksForm.get('status').setValue(status.split(','));
            }

            this.refresh()
        });
        this.translate.onLangChange.subscribe(() =>
        {
            this.refresh();
        });
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => {
            this.refreshUrl();
        });
    }

    onSearchTasks() {
        this.refreshUrl();
    }

    refreshUrl() {
        const queryParams: any = {
            pageIndex: this.paginator.pageIndex,
            pageSize: this.paginator.pageSize,
            active: this.sort.active,
            direction: this.sort.direction
        };
        const actualOwner = this.searchTasksForm.get('actualOwner').value;
        const status = this.searchTasksForm.get('status').value;
        if (actualOwner) {
            queryParams['actualOwner'] = actualOwner;
        }

        if (status) {
            queryParams['status'] = status.join(',');
        }

        this.router.navigate(['.'], {
            relativeTo: this.activatedRoute,
            queryParams: queryParams
        });
    }

    refresh() {
        let startIndex: number = 0;
        let count: number = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }

        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }

        let active = this.getOrder();
        let direction = this.getDirection();
        let request = new SearchTasks(active, direction, count, startIndex, this.searchTasksForm.get('actualOwner').value, this.searchTasksForm.get('status').value);
        this.store.dispatch(request);
    }

    executeAction(act: string, task: Task) {
        switch (act) {
            case 'START':
                {
                    const request = new fromTaskActions.StartTask(task.id);
                    this.store.dispatch(request);
                }
                break;
            case 'NOMINATE':
                const dialogRef = this.dialog.open(NominateTaskDialogComponent);
                dialogRef.afterClosed().subscribe((result: NominateParameter) => {
                    if (!result) {
                        return;
                    }

                    const act = new fromTaskActions.NominateTask(task.id, result);
                    this.store.dispatch(act);
                });
                break;
            case 'CLAIM':
                {
                    const act = new fromTaskActions.ClaimTask(task.id);
                    this.store.dispatch(act);
                }
                break;
            case 'COMPLETE':
                {
                    this.router.navigate(['/tasks/' + task.id]);
                }
                break;
        }
    }

    private getOrder() {
        let active = "createdTime";
        if (this.sort.active) {
            active = this.sort.active;
        }

        return active;
    }

    private getDirection() {
        let direction = "desc";
        if (this.sort.direction) {
            direction = this.sort.direction;
        }

        return direction;
    }
}