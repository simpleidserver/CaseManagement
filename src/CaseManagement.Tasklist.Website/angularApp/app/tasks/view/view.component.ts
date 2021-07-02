import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatSnackBar, MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { TaskHistory } from '@app/stores/tasks/models/task-history.model';
import { Task } from '@app/stores/tasks/models/task.model';
import { TaskState } from '@app/stores/tasks/reducers/tasks.reducers';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-task-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss', '../../common/rendering/rendering.scss']
})
export class ViewTaskComponent implements OnInit {
    @ViewChild(MatSort) sort: MatSort;
    displayedColumns: string[] = ["eventTime", "userPrincipal", "eventType", "startOwner", "endOwner"];
    task: Task = new Task();
    histories$: MatTableDataSource<TaskHistory>;
    formGroup: FormGroup = new FormGroup({});
    uiOption: any = {
        editMode: false
    };
    option: any = {
        type: 'container',
        children: []
    };

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private translate: TranslateService,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject) { }

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

            if (r.rendering) {
                this.option = r.rendering;
            }

            if (r.searchTaskHistory) {
                this.histories$ = new MatTableDataSource(r.searchTaskHistory.content);
                this.histories$.sort = this.sort;
            }
        });
        this.refresh();
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const req = new fromTaskActions.RenderingTask(id, "eventTime", "desc");
        this.store.dispatch(req);
    }

    onSubmit() {
        if (!this.formGroup.valid) {
            return;
        }

        const id = this.route.snapshot.params['id'];
        const req = new fromTaskActions.SubmitTask(id, this.formGroup.value);
        this.store.dispatch(req);
    }

    private buildJSON(result: any, opt: any) {
        switch (opt.type) {
            case 'txt':
            case 'select':
                if (opt.value) {
                    result[opt.name] = opt.value;
                }
                break;
            default:
                if (opt.children && opt.children.length > 0) {
                    opt.children.forEach((r: any) => {
                        this.buildJSON(result, r);
                    });
                }
        }
    }
}