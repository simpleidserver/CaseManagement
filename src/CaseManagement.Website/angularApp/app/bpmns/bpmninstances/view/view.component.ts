import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFilesActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { BpmnInstance, BpmnExecutionPath, BpmnExecutionPointer } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
let BpmnViewer = require('bpmn-js/lib/Viewer');

declare var $: any;

@Component({
    selector: 'view-bpmn-instance',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewBpmnInstanceComponent implements OnInit {
    displayedTokenColumns: string[] = ['name', 'content'];
    displayedStateColumns: string[] = ['state', 'content', 'message'];
    id: string;
    selectedPointer: BpmnExecutionPointer = null;
    currentExecPathId: string = null;
    viewer: any;
    bpmnInstance: BpmnInstance = new BpmnInstance();
    bpmnFile: BpmnFile = null;

    constructor(private store: Store<fromAppState.AppState>,
        private translateService: TranslateService,
        private route: ActivatedRoute,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject) {
    }

    ngOnInit() {
        const self = this;
        this.viewer = new BpmnViewer.default({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
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
            this.viewer.importXML(e.payload).then(function () {
                if (self.bpmnInstance.executionPaths && self.bpmnInstance.executionPaths.length > 0) {
                    self.displayExecutionPath(null, self.bpmnInstance.executionPaths[0]);
                }
            });
        });
        this.store.pipe(select(fromAppState.selectBpmnInstanceResult)).subscribe((e: BpmnInstance) => {
            if (!e) {
                return;
            }

            e.executionPaths.sort(function (a: BpmnExecutionPath, b: BpmnExecutionPath) {
                return new Date(b.createDateTime).getTime() - new Date(a.createDateTime).getTime();
            });
            this.bpmnInstance = e;
            const request = new fromBpmnFilesActions.GetBpmnFile(e.processFileId);
            this.store.dispatch(request);
        });
        this.id = this.route.snapshot.params['id'];
        this.refresh();
    }

    displayExecutionPath(evt: any, execPath: BpmnExecutionPath) {
        if (evt) {
            evt.preventDefault();
        }

        const self = this;
        this.currentExecPathId = execPath.id;
        let overlays = self.viewer.get('overlays');
        let elementRegistry = self.viewer.get('elementRegistry');
        execPath.executionPointers.forEach(function (execPointer: BpmnExecutionPointer) {
            let elt = execPointer.flowNodeInstance;
            let eltReg = elementRegistry.get(elt.flowNodeId);
            overlays.remove({ element: elt.flowNodeId });
            let errorOverlayHtml = "<div class='{0}' style='width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>";
            let completeOverlayHtml = "<div class='{0}' style='width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>";
            let outgoingTokens = "<div class='outgoing-tokens'>" + execPointer.outgoingTokens.length +"</div>"
            var isCircle = eltReg.type === "bpmn:StartEvent" ? true : false;
            var errorOverlayCl = "error-overlay";
            var completeOverlayCl = "complete-overlay";
            if (isCircle) {
                errorOverlayCl = errorOverlayCl + " circle";
                completeOverlayCl = completeOverlayCl + " circle";
            }

            errorOverlayHtml = errorOverlayHtml.replace('{0}', errorOverlayCl);
            completeOverlayHtml = completeOverlayHtml.replace('{0}', completeOverlayCl);
            errorOverlayHtml = $(errorOverlayHtml);
            completeOverlayHtml = $(completeOverlayHtml);
            if (elt.activityState && elt.activityState === 'FAILING') {
                overlays.add(elt.flowNodeId, {
                    position: {
                        top: 0,
                        left: 0,
                    },
                    html: errorOverlayHtml
                });
            } else if (elt.state === 'Complete') {
                overlays.add(elt.flowNodeId, {
                    position: {
                        top: 0,
                        left: 0,
                    },
                    html: completeOverlayHtml
                });
            }

            overlays.add(elt.flowNodeId, {
                position: {
                    bottom: 10,
                    right: 10
                },
                html: outgoingTokens
            });
            $(completeOverlayHtml).click(function () {
                self.updateProperties(execPointer);
            });
            $(errorOverlayHtml).click(function () {
                self.updateProperties(execPointer);
            });
        });
    }

    refresh() {
        const request = new fromBpmnInstanceActions.GetBpmnInstance(this.id);
        this.store.dispatch(request);
    }

    updateProperties(execPointer: BpmnExecutionPointer) {
        this.selectedPointer = execPointer;
    }
}
