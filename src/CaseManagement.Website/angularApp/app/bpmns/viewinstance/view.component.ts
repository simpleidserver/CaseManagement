import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar, MatTableDataSource, MatSort, MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFilesActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { BpmnExecutionPath, BpmnInstance, BpmnExecutionPointer, ActivityStateHistory, BpmnMessageToken } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { ViewMessageDialog } from './view-message-dialog';

let BpmnViewer = require('bpmn-js/lib/Viewer');

declare var $: any;

@Component({
    selector: 'view-bpmn-instance',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewBpmnInstanceComponent implements OnInit {
    activityStatesDisplayedColumns: string[] = ['state', 'executionDateTime', 'message'];
    incomingTokensDisplayedColumns: string[] = ['name', 'content'];
    outgoingTokensDisplayedColumns: string[] = ['name', 'content'];
    activityStates$: MatTableDataSource<ActivityStateHistory> = new MatTableDataSource<ActivityStateHistory>();
    incomingTokens$: MatTableDataSource<BpmnMessageToken> = new MatTableDataSource<BpmnMessageToken>();
    outgoingTokens$: MatTableDataSource<BpmnMessageToken> = new MatTableDataSource<BpmnMessageToken>();
    @ViewChild('activityStatesSort') activityStatesSort: MatSort;
    @ViewChild('incomingTokensSort') incomingTokensSort: MatSort;
    @ViewChild('outgoingTokensSort') outgoingTokensSort: MatSort;
    viewer: any;
    fileId: string;
    instanceId: string;
    execPathId: string;
    bpmnInstance: BpmnInstance = new BpmnInstance();
    bpmnFile: BpmnFile = new BpmnFile();
    executionPaths: BpmnExecutionPath[] = [];
    executionPathFormControl: FormControl = new FormControl();

    constructor(private store: Store<fromAppState.AppState>,
        private translateService: TranslateService,
        private route: ActivatedRoute,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject,
        private router: Router,
        public dialog: MatDialog) {
    }

    ngOnInit() {
        this.viewer = new BpmnViewer.default({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.activityStates$.data = [new ActivityStateHistory(), new ActivityStateHistory()];
        this.incomingTokens$.data = [new BpmnMessageToken(), new BpmnMessageToken()];
        this.outgoingTokens$.data = [new BpmnMessageToken(), new BpmnMessageToken()];
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnInstanceActions.ActionTypes.ERROR_GET_BPMNINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN..MESSAGES.ERROR_GET_BPMNINSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe((e: BpmnFile) => {
            if (!e || !e.payload) {
                return;
            }

            this.bpmnFile = e;
            this.refreshCanvas();

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
        });
        this.fileId = this.route.snapshot.params['id'];
        this.instanceId = this.route.snapshot.params['instanceid'];
        this.execPathId = this.route.snapshot.params['pathid'];
        if (this.execPathId) {
            this.executionPathFormControl.setValue(this.execPathId);
        }

        this.refresh();
    }

    ngAfterViewInit() {
        this.activityStates$.sort = this.activityStatesSort;
    }

    updateExecutionPath() {
        this.execPathId = this.executionPathFormControl.value;
        this.router.navigate(['/bpmns/' + this.fileId + '/' + this.instanceId + '/' + this.execPathId]);
    }

    refreshCanvas() {
        const self = this;
        this.viewer.importXML(self.bpmnFile.payload).then(function () {
            if (self.bpmnInstance.executionPaths && self.bpmnInstance.executionPaths.length > 0) {
                const filtered = self.bpmnInstance.executionPaths.filter(function (v: BpmnExecutionPath) {
                    return v.id === self.execPathId;
                })

                if (filtered.length !== 1) {
                    return;
                }

                self.displayExecutionPath(null, filtered[0]);
            }

            const canvas = self.viewer.get('canvas');
            canvas.zoom('fit-viewport');
        });
    }

    refresh() {
        this.store.dispatch(new fromBpmnInstanceActions.GetBpmnInstance(this.instanceId));
        this.store.dispatch(new fromBpmnFilesActions.GetBpmnFile(this.fileId));
    }

    viewMessage(json: string) {
        if (typeof json !== "string") {
            json = JSON.stringify(json);
        }

        this.dialog.open(ViewMessageDialog, {
            data: { json: json},
            width: '800px'
        });
    }

    private displayExecutionPath(evt: any, execPath: BpmnExecutionPath) {
        if (evt) {
            evt.preventDefault();
        }

        const self = this;
        let overlays = self.viewer.get('overlays');
        let elementRegistry = self.viewer.get('elementRegistry');
        execPath.executionPointers.forEach(function (execPointer: BpmnExecutionPointer) {
            let elt = execPointer.flowNodeInstance;
            let eltReg = elementRegistry.get(elt.flowNodeId);
            overlays.remove({ element: elt.flowNodeId });
            let errorOverlayHtml = "<div class='{0}' data-id='" + execPointer.id + "' style='width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>";
            let completeOverlayHtml = "<div class='{0}' data-id='" + execPointer.id + "' style='width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>";
            let selectedOverlayHtml: any = "<div class='{0}'></div>";
            let outgoingTokens = "<div class='outgoing-tokens'>" + execPointer.outgoingTokens.length + "</div>"
            const isCircle = eltReg.type === "bpmn:StartEvent" ? true : false;
            const isDiamond = eltReg.type === "bpmn:ExclusiveGateway" ? true : false;
            var errorOverlayCl = "error-overlay";
            var completeOverlayCl = "complete-overlay";
            var selectedOverlayCl = "selected-overlay";
            if (isCircle) {
                errorOverlayCl = errorOverlayCl + " circle";
                completeOverlayCl = completeOverlayCl + " circle";
                selectedOverlayCl = selectedOverlayCl + " selected-circle";
            }

            if (isDiamond) {
                errorOverlayCl = errorOverlayCl + " diamond";
                completeOverlayCl = completeOverlayCl + " diamond";
                selectedOverlayCl = selectedOverlayCl + " selected-diamond";
            }

            errorOverlayHtml = errorOverlayHtml.replace('{0}', errorOverlayCl);
            completeOverlayHtml = completeOverlayHtml.replace('{0}', completeOverlayCl);
            selectedOverlayHtml = selectedOverlayHtml.replace('{0}', selectedOverlayCl);
            errorOverlayHtml = $(errorOverlayHtml);
            completeOverlayHtml = $(completeOverlayHtml);
            selectedOverlayHtml = $(selectedOverlayHtml);
            selectedOverlayHtml.hide();
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
                    top: -1,
                    left: -1,
                },
                html: selectedOverlayHtml
            });
            overlays.add(elt.flowNodeId, {
                position: {
                    bottom: 10,
                    right: 10
                },
                html: outgoingTokens
            });
            $(completeOverlayHtml).click(function () {
                const eltid = $(this).data('id');
                $(".selected-overlay").hide();
                selectedOverlayHtml.show();
                self.displayElt(eltid);
            });
            $(errorOverlayHtml).click(function () {
                const eltid = $(this).data('id');
                $(".selected-overlay").hide();
                selectedOverlayHtml.show();
                self.displayElt(eltid);
            });
        });
    }

    private displayElt(eltid: string) {
        const self = this;
        const filteredPath = self.executionPaths.filter((execPath: BpmnExecutionPath) => {
            return execPath.id = self.execPathId;
        });
        if (filteredPath.length != 1) {
            return;
        }

        const execPointer = filteredPath[0].executionPointers.filter(function (p: BpmnExecutionPointer) {
            return p.id === eltid;
        })[0];
        this.activityStates$.data = execPointer.flowNodeInstance.activityStates;
        this.incomingTokens$.data = execPointer.incomingTokens;
        this.outgoingTokens$.data = execPointer.outgoingTokens;
    }
}
