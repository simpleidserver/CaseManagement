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
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFilesActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import * as fromCmmnInstancesActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var CmmnViewer = require('cmmn-js/lib/Viewer');
var ViewCmmnPlanInstanceComponent = (function () {
    function ViewCmmnPlanInstanceComponent(store, route, snackBar, actions$, translateService, router) {
        this.store = store;
        this.route = route;
        this.snackBar = snackBar;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.router = router;
        this.cmmnFile = null;
        this.cmmnPlanInstance = null;
    }
    ViewCmmnPlanInstanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.viewer = new CmmnViewer({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnInstancesActions.ActionTypes.ERROR_GET_CMMN_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_GET_CMMN_PLAN_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe(function (e) {
            if (!e || !e.payload) {
                return;
            }
            _this.cmmnFile = e;
            _this.refresh();
        });
        this.store.pipe(select(fromAppState.selectCmmnPlanInstanceResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.cmmnPlanInstance = e;
            var request = new fromCmmnFilesActions.GetCmmnFile(e.caseFileId);
            _this.store.dispatch(request);
        });
        this.init();
    };
    ViewCmmnPlanInstanceComponent.prototype.refresh = function () {
        var self = this;
        if (!self.cmmnFile || !this.cmmnPlanInstance) {
            return;
        }
        this.viewer.importXML(self.cmmnFile.payload, function () {
            self.displayExecution();
            var canvas = self.viewer.get('canvas');
            canvas.zoom('fit-viewport');
        });
    };
    ViewCmmnPlanInstanceComponent.prototype.displayExecution = function () {
        var self = this;
        var id = this.route.snapshot.params['id'];
        var overlays = self.viewer.get('overlays');
        var elementRegistry = self.viewer.get('elementRegistry');
        var grouped = this.cmmnPlanInstance.children.reduce(function (rv, x) {
            rv[x.eltId] = rv[x.eltId] || [];
            rv[x.eltId].push(x);
            return rv;
        }, {});
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
            overlays.add(firstValue.eltId, {
                position: {
                    top: 0,
                    right: 50
                },
                html: nbOccurrenceHtml
            });
            overlays.add(firstValue.eltId, {
                position: {
                    top: 0,
                    right: 20
                },
                html: stateHtml
            });
            overlays.add(firstValue.eltId, {
                position: {
                    top: 0,
                    left: 0,
                },
                html: overlayHtml
            });
            $(overlayHtml).click(function () {
                var eltid = $(this).data('id');
                var elts = elementRegistry.getAll();
                elts.forEach(function (e) {
                    $("[data-element-id='" + e.id + "']").find(".djs-visual > rect").css("stroke", "black");
                    $("[data-element-id='" + e.id + "']").find(".djs-visual > polygon").css("stroke", "black");
                });
                $("[data-element-id='" + eltid + "']").find(".djs-visual > rect").css("stroke", "red");
                $("[data-element-id='" + eltid + "']").find(".djs-visual > polygon").css("stroke", "red");
                self.router.navigate(['/cmmns/cmmninstances/' + id + '/' + eltid]);
            });
        }
    };
    ViewCmmnPlanInstanceComponent.prototype.init = function () {
        var id = this.route.snapshot.params['id'];
        var request = new fromCmmnInstancesActions.GetCmmnPlanInstance(id);
        this.store.dispatch(request);
    };
    ViewCmmnPlanInstanceComponent = __decorate([
        Component({
            selector: 'view-cmmnplaninstance',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            MatSnackBar,
            ScannedActionsSubject,
            TranslateService,
            Router])
    ], ViewCmmnPlanInstanceComponent);
    return ViewCmmnPlanInstanceComponent;
}());
export { ViewCmmnPlanInstanceComponent };
//# sourceMappingURL=view.component.js.map