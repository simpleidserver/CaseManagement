import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFilesActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { BpmnExecutionPath, BpmnInstance } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-bpmn-instance',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewBpmnInstanceComponent implements OnInit {
    id: string;
    viewer: any;
    bpmnInstance: BpmnInstance = new BpmnInstance();
    bpmnFile: BpmnFile = null;
    executionPaths: BpmnExecutionPath[] = [];
    currentExecPathId: string = '';

    constructor(private store: Store<fromAppState.AppState>,
        private translateService: TranslateService,
        private route: ActivatedRoute,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject,
        private router: Router) {
    }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnInstanceActions.ActionTypes.ERROR_GET_BPMNINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_GET_BPMNINSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe((e: BpmnFile) => {
            if (!e || !e.payload) {
                return;
            }

            this.bpmnFile = e;
        });
        this.store.pipe(select(fromAppState.selectBpmnInstanceResult)).subscribe((e: BpmnInstance) => {
            if (!e) {
                return;
            }

            e.executionPaths.sort(function (a: BpmnExecutionPath, b: BpmnExecutionPath) {
                return new Date(b.createDateTime).getTime() - new Date(a.createDateTime).getTime();
            });
            this.executionPaths = e.executionPaths;
            this.bpmnInstance = e;
            const request = new fromBpmnFilesActions.GetBpmnFile(e.processFileId);
            this.store.dispatch(request);
        });
        this.id = this.route.parent.snapshot.params['id'];
        if (this.route.children && this.route.children.length === 1) {
            this.route.children[0].params.subscribe((r: any) => {
                this.currentExecPathId = r['pathid'];
            });
        }

        this.refresh();
    }

    refresh() {
        const request = new fromBpmnInstanceActions.GetBpmnInstance(this.id);
        this.store.dispatch(request);
    }

    navigate(evt: any, execPath: BpmnExecutionPath) {
        evt.preventDefault();
        this.currentExecPathId = execPath.id;
        this.router.navigate(['/bpmns/bpmninstances/' + this.id + '/' + execPath.id]);
    }
}
