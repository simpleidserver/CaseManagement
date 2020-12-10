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
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { ScannedActionsSubject, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ViewBpmnFileComponent = (function () {
    function ViewBpmnFileComponent(store, route, actions$, snackBar, translateService, router) {
        this.store = store;
        this.route = route;
        this.actions$ = actions$;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.router = router;
    }
    ViewBpmnFileComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.ERROR_GET_BPMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_GET_BPMNFILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.BPMNFILE_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.ERROR_UPDATE_BPMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_UPDATE_BPMNFILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.BPMNFILE_PAYLOAD_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.ERROR_UPDATE_BPMNFILE_PAYLOAD; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_UPDATE_BPMNFILE_PAYLOAD'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.COMPLETE_PUBLISH_BPMNFILE; }))
            .subscribe(function (e) {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.BPMNFILE_PUBLISHED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.router.navigate(["/bpmns/bpmnfiles/" + e.id]);
            _this.id = e.id;
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.ERROR_PUBLISH_BPMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_PUBLISH_BPMNFILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.BPMNFILE_PAYLOAD_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.ERROR_UPDATE_BPMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_UPDATE_BPMNFILE_PAYLOAD'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.COMPLETE_CREATE_BPMN_INSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.BPMN_INSTANCE_CREATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.ERROR_CREATE_BPMNINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_CREATE_BPMN_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.COMPLETE_START_BPMNINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.BPMN_INSTANCE_STARTED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.ERROR_START_BPMNINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_START_BPMN_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.id = this.route.snapshot.params['id'];
        this.refresh();
    };
    ViewBpmnFileComponent.prototype.publish = function () {
        var act = new fromBpmnFileActions.PublishBpmnFile(this.id);
        this.store.dispatch(act);
    };
    ViewBpmnFileComponent.prototype.refresh = function () {
        var request = new fromBpmnFileActions.GetBpmnFile(this.id);
        this.store.dispatch(request);
    };
    ViewBpmnFileComponent.prototype.createInstance = function () {
        var request = new fromBpmnInstanceActions.CreateBpmnInstance(this.id);
        this.store.dispatch(request);
    };
    ViewBpmnFileComponent = __decorate([
        Component({
            selector: 'view-bpmn-file',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            ScannedActionsSubject,
            MatSnackBar,
            TranslateService,
            Router])
    ], ViewBpmnFileComponent);
    return ViewBpmnFileComponent;
}());
export { ViewBpmnFileComponent };
//# sourceMappingURL=view.component.js.map