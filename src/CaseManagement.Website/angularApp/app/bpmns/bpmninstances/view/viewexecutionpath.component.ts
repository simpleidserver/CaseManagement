import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import { BpmnExecutionPath, BpmnExecutionPointer, BpmnInstance } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { select, Store } from '@ngrx/store';
let BpmnViewer = require('bpmn-js/lib/Viewer');

declare var $: any;

@Component({
    selector: 'view-execution-path',
    templateUrl: './viewexecutionpath.component.html',
    styleUrls: ['./viewexecutionpath.component.scss']
})
export class ViewExecutionPathComponent implements OnInit {
    viewer: any;
    bpmnInstance: BpmnInstance;
    bpmnFile: BpmnFile = null;

    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private router: Router) { }

    ngOnInit() {
        this.viewer = new BpmnViewer.default({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe((e: BpmnFile) => {
            if (!e || !e.payload) {
                return;
            }

            this.bpmnFile = e;
            this.refresh();
        });
        this.store.pipe(select(fromAppState.selectBpmnInstanceResult)).subscribe((e: BpmnInstance) => {
            if (!e) {
                return;
            }

            this.bpmnInstance = e;
            this.refresh();
        });
    }

    refresh() {
        const self = this;
        if (!self.bpmnFile || !this.bpmnInstance) {
            return;
        }
        
        const pathid = this.route.snapshot.params['pathid'];
        this.viewer.importXML(self.bpmnFile.payload).then(function () {
            if (self.bpmnInstance.executionPaths && self.bpmnInstance.executionPaths.length > 0) {
                const filtered = self.bpmnInstance.executionPaths.filter(function (v: BpmnExecutionPath) {
                    return v.id == pathid;
                })
                if (filtered.length !== 1) {
                    return;
                }

                self.displayExecutionPath(null, filtered[0]);
            }
        });
    }

    displayExecutionPath(evt: any, execPath: BpmnExecutionPath) {
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
            let errorOverlayHtml = "<div class='{0}' data-id='" + execPointer.id +"' style='width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>";
            let completeOverlayHtml = "<div class='{0}' data-id='" + execPointer.id+"' style='width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>";
            let selectedOverlayHtml : any = "<div class='{0}'></div>";
            let outgoingTokens = "<div class='outgoing-tokens'>" + execPointer.outgoingTokens.length + "</div>"
            var isCircle = eltReg.type === "bpmn:StartEvent" ? true : false;
            var errorOverlayCl = "error-overlay";
            var completeOverlayCl = "complete-overlay";
            var selectedOverlayCl = "selected-overlay";
            if (isCircle) {
                errorOverlayCl = errorOverlayCl + " circle";
                completeOverlayCl = completeOverlayCl + " circle";
                selectedOverlayCl = selectedOverlayCl + " selected-circle";
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
            const id = self.route.parent.parent.snapshot.params['id'];
            const pathid = self.route.snapshot.params['pathid'];
            $(completeOverlayHtml).click(function () {
                const eltid = $(this).data('id');
                $(".selected-overlay").hide();
                self.router.navigate(['/bpmns/bpmninstances/' + id + '/' + pathid + '/' + eltid]);
                selectedOverlayHtml.show();
            });
            $(errorOverlayHtml).click(function () {
                const eltid = $(this).data('id');
                $(".selected-overlay").hide();
                self.router.navigate(['/bpmns/bpmninstances/' + id + '/' + pathid + '/' + eltid]);
                selectedOverlayHtml.show();
            });
        });
    }
}
