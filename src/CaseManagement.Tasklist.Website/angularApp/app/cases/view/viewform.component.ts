import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RenderingTask, SubmitTask } from '@app/stores/tasks/actions/tasks.actions';
import * as fromAppState from '@app/stores/appstate';
import { Store, select, ScannedActionsSubject } from '@ngrx/store';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { Task } from '@app/stores/tasks/models/task.model';
import { TaskState } from '@app/stores/tasks/reducers/tasks.reducers';
import { RenderingElement } from '@app/stores/tasks/models/rendering';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { filter } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material';

@Component({
    selector: 'view-form-component',
    templateUrl: './viewform.component.html',
    styleUrls: ['./viewform.component.scss']
})
export class ViewFormComponent implements OnInit {
    submitForm: FormGroup;
    renderingElts: RenderingElement[] = [];
    task: Task = new Task();

    constructor(
        private store: Store<fromAppState.AppState>,
        private activatedRoute: ActivatedRoute,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private router: Router,
        private translate: TranslateService,
        private formBuilder: FormBuilder) {

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

            const grp: any = {};
            if (r.renderingElts) {
                this.renderingElts = r.renderingElts;
                if (r.renderingElts && r.renderingElts.length > 0) {
                    r.renderingElts.forEach((oe: RenderingElement) => {
                        grp[oe.id] = new FormControl('');
                    });
                }
            }

            this.submitForm = this.formBuilder.group(grp);
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

    onSubmit(e: any) {
        const id = this.activatedRoute.snapshot.params['formid'];
        const req = new SubmitTask(id, e);
        this.store.dispatch(req);
    }
}