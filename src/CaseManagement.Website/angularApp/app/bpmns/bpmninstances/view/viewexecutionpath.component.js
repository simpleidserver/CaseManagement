var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { select, Store } from '@ngrx/store';
var BpmnViewer = require('bpmn-js/lib/Viewer');
var ViewExecutionPathComponent = (function () {
    function ViewExecutionPathComponent(store, route, router) {
        this.store = store;
        this.route = route;
        this.router = router;
        this.bpmnFile = null;
    }
    ViewExecutionPathComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.viewer = new BpmnViewer.default({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe(function (e) {
            if (!e || !e.payload) {
                return;
            }
            _this.bpmnFile = e;
            _this.refresh();
        });
        this.store.pipe(select(fromAppState.selectBpmnInstanceResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.bpmnInstance = e;
            _this.refresh();
        });
        this.route.params.subscribe(function () {
            if (!_this.bpmnInstance) {
                return;
            }
            _this.refresh();
        });
    };
    ViewExecutionPathComponent.prototype.refresh = function () {
        var self = this;
        if (!self.bpmnFile || !this.bpmnInstance) {
            return;
        }
        var pathid = this.route.snapshot.params['pathid'];
        this.viewer.importXML(self.bpmnFile.payload).then(function () {
            if (self.bpmnInstance.executionPaths && self.bpmnInstance.executionPaths.length > 0) {
                var filtered = self.bpmnInstance.executionPaths.filter(function (v) {
                    return v.id == pathid;
                });
                if (filtered.length !== 1) {
                    return;
                }
                self.displayExecutionPath(null, filtered[0]);
                var canvas = self.viewer.get('canvas');
                canvas.zoom('fit-viewport');
            }
        });
    };
    ViewExecutionPathComponent.prototype.displayExecutionPath = function (evt, execPath) {
        if (evt) {
            evt.preventDefault();
        }
        var self = this;
        var overlays = self.viewer.get('overlays');
        var elementRegistry = self.viewer.get('elementRegistry');
        execPath.executionPointers.forEach(function (execPointer) {
            var elt = execPointer.flowNodeInstance;
            var eltReg = elementRegistry.get(elt.flowNodeId);
            overlays.remove({ element: elt.flowNodeId });
            var errorOverlayHtml = "<div class='{0}' data-id='" + execPointer.id + "' style='width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>";
            var completeOverlayHtml = "<div class='{0}' data-id='" + execPointer.id + "' style='width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>";
            var selectedOverlayHtml = "<div class='{0}'></div>";
            var outgoingTokens = "<div class='outgoing-tokens'>" + execPointer.outgoingTokens.length + "</div>";
            var isCircle = eltReg.type === "bpmn:StartEvent" ? true : false;
            var isDiamond = eltReg.type === "bpmn:ExclusiveGateway" ? true : false;
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
            }
            else if (elt.state === 'Complete') {
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
            var id = self.route.parent.parent.snapshot.params['id'];
            var pathid = self.route.snapshot.params['pathid'];
            $(completeOverlayHtml).click(function () {
                var eltid = $(this).data('id');
                $(".selected-overlay").hide();
                self.router.navigate(['/bpmns/bpmninstances/' + id + '/' + pathid + '/' + eltid]);
                selectedOverlayHtml.show();
            });
            $(errorOverlayHtml).click(function () {
                var eltid = $(this).data('id');
                $(".selected-overlay").hide();
                self.router.navigate(['/bpmns/bpmninstances/' + id + '/' + pathid + '/' + eltid]);
                selectedOverlayHtml.show();
            });
        });
    };
    ViewExecutionPathComponent = __decorate([
        Component({
            selector: 'view-execution-path',
            templateUrl: './viewexecutionpath.component.html',
            styleUrls: ['./viewexecutionpath.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            Router])
    ], ViewExecutionPathComponent);
    return ViewExecutionPathComponent;
}());
export { ViewExecutionPathComponent };
//# sourceMappingURL=viewexecutionpath.component.js.map