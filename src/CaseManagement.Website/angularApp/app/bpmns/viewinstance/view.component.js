var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild } from '@angular/core';
import { MatSnackBar, MatTableDataSource, MatSort, MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFilesActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { BpmnInstance, ActivityStateHistory, BpmnMessageToken } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
var BpmnViewer = require('bpmn-js/lib/Viewer');
var ViewBpmnInstanceComponent = (function () {
    function ViewBpmnInstanceComponent(store, translateService, route, snackBar, actions$, router, dialog) {
        this.store = store;
        this.translateService = translateService;
        this.route = route;
        this.snackBar = snackBar;
        this.actions$ = actions$;
        this.router = router;
        this.dialog = dialog;
        this.activityStatesDisplayedColumns = ['state', 'executionDateTime', 'message'];
        this.incomingTokensDisplayedColumns = ['name', 'content'];
        this.outgoingTokensDisplayedColumns = ['name', 'content'];
        this.activityStates$ = new MatTableDataSource();
        this.incomingTokens$ = new MatTableDataSource();
        this.outgoingTokens$ = new MatTableDataSource();
        this.bpmnInstance = new BpmnInstance();
        this.bpmnFile = new BpmnFile();
        this.executionPaths = [];
        this.executionPathFormControl = new FormControl();
    }
    ViewBpmnInstanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.viewer = new BpmnViewer.default({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.activityStates$.data = [new ActivityStateHistory(), new ActivityStateHistory()];
        this.incomingTokens$.data = [new BpmnMessageToken(), new BpmnMessageToken()];
        this.outgoingTokens$.data = [new BpmnMessageToken(), new BpmnMessageToken()];
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.ERROR_GET_BPMNINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN..MESSAGES.ERROR_GET_BPMNINSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe(function (e) {
            if (!e || !e.payload) {
                return;
            }
            _this.bpmnFile = e;
            _this.refreshCanvas();
        });
        this.store.pipe(select(fromAppState.selectBpmnInstanceResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            e.executionPaths.sort(function (a, b) {
                return new Date(b.createDateTime).getTime() - new Date(a.createDateTime).getTime();
            });
            _this.executionPaths = e.executionPaths;
            _this.bpmnInstance = e;
        });
        this.fileId = this.route.snapshot.params['id'];
        this.instanceId = this.route.snapshot.params['instanceid'];
        this.execPathId = this.route.snapshot.params['pathid'];
        if (this.execPathId) {
            this.executionPathFormControl.setValue(this.execPathId);
        }
        this.refresh();
    };
    ViewBpmnInstanceComponent.prototype.ngAfterViewInit = function () {
        this.activityStates$.sort = this.activityStatesSort;
    };
    ViewBpmnInstanceComponent.prototype.updateExecutionPath = function () {
        this.execPathId = this.executionPathFormControl.value;
        this.router.navigate(['/bpmns/' + this.fileId + '/' + this.instanceId + '/' + this.execPathId]);
    };
    ViewBpmnInstanceComponent.prototype.refreshCanvas = function () {
        var self = this;
        this.viewer.importXML(self.bpmnFile.payload).then(function () {
            if (self.bpmnInstance.executionPaths && self.bpmnInstance.executionPaths.length > 0) {
                var filtered = self.bpmnInstance.executionPaths.filter(function (v) {
                    return v.id === self.execPathId;
                });
                var canvas = self.viewer.get('canvas');
                canvas.zoom('fit-viewport');
                if (filtered.length !== 1) {
                    return;
                }
                self.displayExecutionPath(null, filtered[0]);
            }
        });
    };
    ViewBpmnInstanceComponent.prototype.refresh = function () {
        this.store.dispatch(new fromBpmnInstanceActions.GetBpmnInstance(this.instanceId));
        this.store.dispatch(new fromBpmnFilesActions.GetBpmnFile(this.fileId));
    };
    ViewBpmnInstanceComponent.prototype.viewMessage = function (ActivityStateHistory) {
    };
    ViewBpmnInstanceComponent.prototype.displayExecutionPath = function (evt, execPath) {
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
            $(completeOverlayHtml).click(function () {
                var eltid = $(this).data('id');
                $(".selected-overlay").hide();
                selectedOverlayHtml.show();
                self.displayElt(eltid);
            });
            $(errorOverlayHtml).click(function () {
                var eltid = $(this).data('id');
                $(".selected-overlay").hide();
                selectedOverlayHtml.show();
                self.displayElt(eltid);
            });
        });
    };
    ViewBpmnInstanceComponent.prototype.displayElt = function (eltid) {
        var self = this;
        var filteredPath = self.executionPaths.filter(function (execPath) {
            return execPath.id = self.execPathId;
        });
        if (filteredPath.length != 1) {
            return;
        }
        var execPointer = filteredPath[0].executionPointers.filter(function (p) {
            return p.id === eltid;
        })[0];
        this.activityStates$.data = execPointer.flowNodeInstance.activityStates;
        this.incomingTokens$.data = execPointer.incomingTokens;
        this.outgoingTokens$.data = execPointer.outgoingTokens;
    };
    __decorate([
        ViewChild('activityStatesSort'),
        __metadata("design:type", MatSort)
    ], ViewBpmnInstanceComponent.prototype, "activityStatesSort", void 0);
    __decorate([
        ViewChild('incomingTokensSort'),
        __metadata("design:type", MatSort)
    ], ViewBpmnInstanceComponent.prototype, "incomingTokensSort", void 0);
    __decorate([
        ViewChild('outgoingTokensSort'),
        __metadata("design:type", MatSort)
    ], ViewBpmnInstanceComponent.prototype, "outgoingTokensSort", void 0);
    ViewBpmnInstanceComponent = __decorate([
        Component({
            selector: 'view-bpmn-instance',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            TranslateService,
            ActivatedRoute,
            MatSnackBar,
            ScannedActionsSubject,
            Router,
            MatDialog])
    ], ViewBpmnInstanceComponent);
    return ViewBpmnInstanceComponent;
}());
export { ViewBpmnInstanceComponent };
//# sourceMappingURL=view.component.js.map