import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { OutputRenderingElement, Rendering } from '@app/stores/tasks/models/rendering';
import { TaskState } from '@app/stores/tasks/reducers/tasks.reducers';
import { select, Store } from '@ngrx/store';
import { Task } from '../../stores/tasks/models/task.model';

@Component({
    selector: 'view-task-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewTaskComponent implements OnInit {
    baseTranslationKey: string = "TASKS.VIEW";
    rendering: Rendering = new Rendering();
    task: Task = new Task();
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