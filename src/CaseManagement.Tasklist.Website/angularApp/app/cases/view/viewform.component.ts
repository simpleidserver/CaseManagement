import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { RenderingTask } from '@app/stores/tasks/actions/tasks.actions';
import { Task } from '@app/stores/tasks/models/task.model';
import { TaskState } from '@app/stores/tasks/reducers/tasks.reducers';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-form-component',
    templateUrl: './viewform.component.html',
    styleUrls: ['./viewform.component.scss']
})
export class ViewFormComponent implements OnInit {
    option: any = {
        type: 'container',
        children: []
    };
    task: Task = new Task();

    constructor(
        private store: Store<fromAppState.AppState>,
        private activatedRoute: ActivatedRoute,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private router: Router,
        private translate: TranslateService) {

    }

    ngOnInit(): void {
        const id = this.activatedRoute.parent.snapshot.params['id'];
        this.actions$.pipe(
            filter((action: any) => action.type === fromTaskActions.ActionTypes.COMPLETE_SUBMIT_TASK))
            .subscribe(() => {
                this.snackBar.open(this.translate.instant('TASKS.MESSAGES.TASK_COMPLETED'), this.translate.instant('undo'), {
                    duration: 2000
                });
                this.router.navigate(['/cases/' + id]);
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
        });
        this.activatedRoute.params.subscribe(() => {
            this.refresh();
        });
    }

    refresh() {
        const id = this.activatedRoute.snapshot.params['formid'];
        const req = new RenderingTask(id, "eventTime", "desc");
        this.store.dispatch(req);
    }

    onSubmit() {
        const result: any = {};
        this.buildJSON(result, this.option);
        const id = this.task.id;
        const req = new fromTaskActions.SubmitTask(id, result);
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