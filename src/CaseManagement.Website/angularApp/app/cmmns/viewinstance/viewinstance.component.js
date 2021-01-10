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
import { ActivatedRoute } from '@angular/router';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { CmmnPlanInstanceResult } from '@app/stores/cmmninstances/models/cmmn-planinstance.model';
import * as fromAppState from '@app/stores/appstate';
import { Store, select, ScannedActionsSubject } from '@ngrx/store';
import * as fromCmmnPlanInstanceActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { MatSnackBar, MatDialog, MatTableDataSource, MatSort } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { ViewMessageDialog } from './view-message-dialog';
var CmmnViewer = require('cmmn-js/lib/Viewer');
var Instance = (function () {
    function Instance() {
    }
    return Instance;
}());
export { Instance };
var ViewCmmnInstanceComponent = (function () {
    function ViewCmmnInstanceComponent(activatedRoute, store, snackBar, actions$, translateService, dialog) {
        this.activatedRoute = activatedRoute;
        this.store = store;
        this.snackBar = snackBar;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.dialog = dialog;
        this.overlayStore = [];
        this.displayedColumns = ['name', 'executionDateTime', 'state', 'transition', 'nbOccurrence', 'message'];
        this.cmmnFile = new CmmnFile();
        this.cmmnPlanInstance = new CmmnPlanInstanceResult();
        this.instances$ = new MatTableDataSource();
    }
    ViewCmmnInstanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
        this.instances$.data = [new Instance(), new Instance()];
        this.viewer = new CmmnViewer({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnPlanInstanceActions.ActionTypes.ERROR_GET_CMMN_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_GET_CMMNPLANINSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.ERROR_GET_CMMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_GET_CMMNFILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.cmmnFileListener = this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe(function (e) {
            if (!e || !e.content) {
                return;
            }
            _this.cmmnFile = e.content;
            if (self.overlayStore.length === 0) {
                _this.viewer.importXML(e.content.payload, function () {
                    self.displayExecution();
                    var canvas = self.viewer.get('canvas');
                    canvas.zoom('fit-viewport');
                });
            }
            else {
                self.displayExecution();
            }
        });
        this.cmmnPlanInstanceListener = this.store.pipe(select(fromAppState.selectCmmnPlanInstanceResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.cmmnPlanInstance = e;
            var getCmmnFileRequest = new fromCmmnFileActions.GetCmmnFile(e.caseFileId);
            _this.store.dispatch(getCmmnFileRequest);
        });
        this.cmmnFileId = this.activatedRoute.snapshot.params['id'];
        this.cmmnPlanInstanceId = this.activatedRoute.snapshot.params['instanceid'];
        this.interval = setInterval(function () {
            self.refresh();
        }, 2000);
        this.refresh();
    };
    ViewCmmnInstanceComponent.prototype.ngOnDestroy = function () {
        this.cmmnFileListener.unsubscribe();
        this.cmmnPlanInstanceListener.unsubscribe();
        if (this.interval) {
            clearInterval(this.interval);
        }
    };
    ViewCmmnInstanceComponent.prototype.ngAfterViewInit = function () {
        this.instances$.sort = this.instancesSort;
    };
    ViewCmmnInstanceComponent.prototype.viewMessage = function (json) {
        if (typeof json !== "string") {
            json = JSON.stringify(json);
        }
        this.dialog.open(ViewMessageDialog, {
            data: { json: json },
            width: '800px'
        });
    };
    ViewCmmnInstanceComponent.prototype.displayExecution = function () {
        var self = this;
        var overlays = self.viewer.get('overlays');
        var elementRegistry = self.viewer.get('elementRegistry');
        var grouped = this.cmmnPlanInstance.children.filter(function (x) {
            return x.state;
        }).reduce(function (rv, x) {
            rv[x.eltId] = rv[x.eltId] || [];
            rv[x.eltId].push(x);
            return rv;
        }, {});
        self.overlayStore.forEach(function (record) {
            record.overlayIds.forEach(function (id) {
                overlays.remove(id);
            });
        });
        self.overlayStore = [];
        for (var key in grouped) {
            var values = grouped[key];
            var ordered = values.sort(function (a, b) {
                return b.nbOccurrence - a.nbOccurrence;
            });
            var firstValue = ordered[0];
            var eltReg = elementRegistry.get(firstValue.eltId);
            if (!eltReg) {
                continue;
            }
            var stateHtml = "<div class='state " + firstValue.state + "'>" + firstValue.state + "</div>";
            var nbOccurrenceHtml = "<div class='nbOccurrence'>" + values.length + "</div>";
            var overlayHtml = "<div data-id='" + firstValue.eltId + "' style='cursor: pointer !important; width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>";
            overlayHtml = $(overlayHtml);
            var id1 = overlays.add(firstValue.eltId, {
                position: {
                    top: 0,
                    right: 50
                },
                html: nbOccurrenceHtml
            });
            var id2 = overlays.add(firstValue.eltId, {
                position: {
                    top: 0,
                    right: 20
                },
                html: stateHtml
            });
            var id3 = overlays.add(firstValue.eltId, {
                position: {
                    top: 0,
                    left: 0,
                },
                html: overlayHtml
            });
            self.overlayStore.push({ overlayIds: [id1, id2, id3], eltId: firstValue.eltId });
            $(overlayHtml).click(function () {
                var eltid = $(this).data('id');
                var elts = elementRegistry.getAll();
                elts.forEach(function (e) {
                    $("[data-element-id='" + e.id + "']").find(".djs-visual > rect").css("stroke", "black");
                    $("[data-element-id='" + e.id + "']").find(".djs-visual > polygon").css("stroke", "black");
                });
                $("[data-element-id='" + eltid + "']").find(".djs-visual > rect").css("stroke", "red");
                $("[data-element-id='" + eltid + "']").find(".djs-visual > polygon").css("stroke", "red");
                self.displayInstances(grouped[eltid]);
            });
        }
    };
    ViewCmmnInstanceComponent.prototype.displayInstances = function (arr) {
        var self = this;
        var records = [];
        arr.forEach(function (r) {
            r.transitionHistories.forEach(function (th) {
                var record = new Instance();
                record.executionDateTime = th.executionDateTime;
                record.transition = th.transition;
                record.name = r.name;
                record.nbOccurrence = r.nbOccurrence;
                record.state = r.state;
                record.message = th.message;
                records.push(record);
            });
        });
        self.instances$.data = records;
    };
    ViewCmmnInstanceComponent.prototype.refresh = function () {
        var getCmmnPlanInstanceRequest = new fromCmmnPlanInstanceActions.GetCmmnPlanInstance(this.cmmnPlanInstanceId);
        this.store.dispatch(getCmmnPlanInstanceRequest);
    };
    __decorate([
        ViewChild('instancesSort'),
        __metadata("design:type", MatSort)
    ], ViewCmmnInstanceComponent.prototype, "instancesSort", void 0);
    ViewCmmnInstanceComponent = __decorate([
        Component({
            selector: 'view-cmmn-instance',
            templateUrl: './viewinstance.component.html',
            styleUrls: ['./viewinstance.component.scss']
        }),
        __metadata("design:paramtypes", [ActivatedRoute,
            Store,
            MatSnackBar,
            ScannedActionsSubject,
            TranslateService,
            MatDialog])
    ], ViewCmmnInstanceComponent);
    return ViewCmmnInstanceComponent;
}());
export { ViewCmmnInstanceComponent };
//# sourceMappingURL=viewinstance.component.js.map