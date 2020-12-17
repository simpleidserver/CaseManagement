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
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { ScannedActionsSubject, Store, select } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
var ViewCmmnFileComponent = (function () {
    function ViewCmmnFileComponent(store, route, snackBar, translateService, actions$, router) {
        this.store = store;
        this.route = route;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.actions$ = actions$;
        this.router = router;
        this.cmmnFiles$ = [];
        this.cmmnFile = new CmmnFile();
    }
    ViewCmmnFileComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.CMMN_FILE_SAVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE_PAYLOAD; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.CMMN_FILE_PAYLOAD_SAVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.ERROR_UPDATE_CMMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_UPDATE_CMMNFILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.COMPLETE_PUBLISH_CMMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.COMPLETE_PUBLISH_CMMNFILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.ERROR_PUBLISH_CMMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_PUBLISH_CMMNFILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectCmmnFileLstResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.cmmnFiles$ = e.content;
        });
        this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.cmmnFile = e;
            var request = new fromCmmnFileActions.SearchCmmnFiles("create_datetime", "desc", 10000, 0, null, e.fileId, false);
            _this.store.dispatch(request);
        });
        this.route.params.subscribe(function () {
            _this.refresh();
        });
        this.refresh();
    };
    ViewCmmnFileComponent.prototype.publish = function () {
        var id = this.route.snapshot.params['id'];
        var act = new fromCmmnFileActions.PublishCmmnFile(id);
        this.store.dispatch(act);
    };
    ViewCmmnFileComponent.prototype.refresh = function () {
        this.id = this.route.snapshot.params['id'];
        var request = new fromCmmnFileActions.GetCmmnFile(this.id);
        this.store.dispatch(request);
    };
    ViewCmmnFileComponent.prototype.navigate = function (evt, cmmnFile) {
        evt.preventDefault();
        this.router.navigate(['/cmmns/cmmnfiles/' + cmmnFile.id]);
    };
    ViewCmmnFileComponent = __decorate([
        Component({
            selector: 'view-cmmnfile',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            MatSnackBar,
            TranslateService,
            ScannedActionsSubject,
            Router])
    ], ViewCmmnFileComponent);
    return ViewCmmnFileComponent;
}());
export { ViewCmmnFileComponent };
//# sourceMappingURL=view.component.js.map