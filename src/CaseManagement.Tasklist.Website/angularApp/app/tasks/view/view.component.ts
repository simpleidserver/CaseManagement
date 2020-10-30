import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { OutputRenderingElement, Rendering } from '@app/stores/tasks/models/rendering';
import { TaskHistory } from '@app/stores/tasks/models/task-history.model';
import { Task } from '@app/stores/tasks/models/task.model';
import { TaskState } from '@app/stores/tasks/reducers/tasks.reducers';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'view-task-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewTaskComponent implements OnInit {
    baseTranslationKey: string = "TASKS.VIEW";
    @ViewChild(MatSort) sort: MatSort;
    displayedColumns: string[] = ["eventTime", "userPrincipal", "eventType", "startOwner", "endOwner"];
    rendering: Rendering = new Rendering();
    task: Task = new Task();
    histories$: MatTableDataSource<TaskHistory>;
    submitForm: FormGroup;

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder) { }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectTask)).subscribe((r: TaskState) => {
            if (!r) {
                return;
            }

            if (r.task) {
                this.task = r.task;
            }

            const grp: any = {};
            if (r.rendering) {
                this.rendering = r.rendering;
                if (r.rendering.output && r.rendering.output.length > 0) {
                    r.rendering.output.forEach((oe: OutputRenderingElement) => {
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
        console.log(e);
    }
}