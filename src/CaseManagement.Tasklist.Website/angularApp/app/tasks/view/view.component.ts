import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatSnackBar, MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { RenderingElement } from '@app/stores/tasks/models/rendering';
import { TaskHistory } from '@app/stores/tasks/models/task-history.model';
import { Task } from '@app/stores/tasks/models/task.model';
import { TaskState } from '@app/stores/tasks/reducers/tasks.reducers';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-task-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewTaskComponent implements OnInit {
    @ViewChild(MatSort) sort: MatSort;
    displayedColumns: string[] = ["eventTime", "userPrincipal", "eventType", "startOwner", "endOwner"];
    renderingElts: RenderingElement[] = [];
    task: Task = new Task();
    histories$: MatTableDataSource<TaskHistory>;
    submitForm: FormGroup;

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private translate: TranslateService,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject,
        private formBuilder: FormBuilder) { }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromTaskActions.ActionTypes.COMPLETE_SUBMIT_TASK))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('TASKS.MESSAGES.TASK_COMPLETED'), this.translate.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromTaskActions.ActionTypes.ERROR_SUBMIT_TASK))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('TASKS.MESSAGES.ERROR_SUBMIT_TASK'), this.translate.instant('undo'), {
                    duration: 2000
                });
            });

        this.store.pipe(select(fromAppState.selectTask)).subscribe((r: TaskState) => {
            if (!r) {
                return;
            }

            if (r.task) {
                this.task = r.task;
            }

            const grp: any = {};
            if (r.renderingElts) {
                this.renderingElts = r.renderingElts;
                if (r.renderingElts && r.renderingElts.length > 0) {
                    r.renderingElts.forEach((oe: RenderingElement) => {
                        grp[oe.id] = new FormControl('');
                    });
                }
            }

            if (r.searchTaskHistory) {
                this.histories$ = new MatTableDataSource(r.searchTaskHistory.content);
                this.histories$.sort = this.sort;
            }

            this.submitForm = this.formBuilder.group(grp);
        });
        this.refresh();
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const req = new fromTaskActions.RenderingTask(id, "eventTime", "desc");
        this.store.dispatch(req);
    }

    onSubmit(e: any) {
        const id = this.route.snapshot.params['id'];
        const req = new fromTaskActions.SubmitTask(id, e);
        this.store.dispatch(req);
    }
}