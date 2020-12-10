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
import * as fromBpmnFilesActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { BpmnInstance } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ViewBpmnInstanceComponent = (function () {
    function ViewBpmnInstanceComponent(store, translateService, route, snackBar, actions$, router) {
        this.store = store;
        this.translateService = translateService;
        this.route = route;
        this.snackBar = snackBar;
        this.actions$ = actions$;
        this.router = router;
        this.bpmnInstance = new BpmnInstance();
        this.bpmnFile = null;
        this.executionPaths = [];
        this.currentExecPathId = '';
    }
    ViewBpmnInstanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.ERROR_GET_BPMNINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_GET_BPMNINSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe(function (e) {
            if (!e || !e.payload) {
                return;
            }
            _this.bpmnFile = e;
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
            var request = new fromBpmnFilesActions.GetBpmnFile(e.processFileId);
            _this.store.dispatch(request);
        });
        this.id = this.route.parent.snapshot.params['id'];
        if (this.route.children && this.route.children.length === 1) {
            this.route.children[0].params.subscribe(function (r) {
                _this.currentExecPathId = r['pathid'];
            });
        }
        this.refresh();
    };
    ViewBpmnInstanceComponent.prototype.refresh = function () {
        var request = new fromBpmnInstanceActions.GetBpmnInstance(this.id);
        this.store.dispatch(request);
    };
    ViewBpmnInstanceComponent.prototype.navigate = function (evt, execPath) {
        evt.preventDefault();
        this.currentExecPathId = execPath.id;
        this.router.navigate(['/bpmns/bpmninstances/' + this.id + '/' + execPath.id]);
    };
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
            Router])
    ], ViewBpmnInstanceComponent);
    return ViewBpmnInstanceComponent;
}());
export { ViewBpmnInstanceComponent };
//# sourceMappingURL=view.component.js.map