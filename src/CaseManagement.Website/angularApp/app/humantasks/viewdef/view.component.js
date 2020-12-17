var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import * as fromHumanTaskInstActions from '@app/stores/humantaskinstances/actions/humantaskinst.actions';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { CreateHumanTaskInstanceDialog } from './create-humantaskinstance-dialog.component';
var ViewHumanTaskDef = (function () {
    function ViewHumanTaskDef(store, actions$, snackBar, translateService, dialog, route) {
        this.store = store;
        this.actions$ = actions$;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.dialog = dialog;
        this.route = route;
        this.humanTaskDef = null;
        this.isErrorOccured = false;
        this.baseTranslationKey = "HUMANTASK.DEF.VIEW";
    }
    ViewHumanTaskDef.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_GET_HUMANTASKDEF; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.HUMANTASKDEF_DOESNT_EXIST'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.isErrorOccured = true;
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskInstActions.ActionTypes.COMPLETE_ME_CREATE_HUMANTASKINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.HUMANTASKINSTANCE_CREATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskInstActions.ActionTypes.ERROR_ME_CREATE_HUMANTASKINSTANCE; }))
            .subscribe(function (e) {
            var msg = [];
            for (var k in e.error.error.errors) {
                msg.push(k + " : " + e.error.error.errors[k].join(','));
            }
            _this.snackBar.open(msg.join(';'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.humanTaskDef = e;
            _this.id = e.id;
        });
        this.refresh();
    };
    ViewHumanTaskDef.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var request = new fromHumanTaskDefActions.GetHumanTaskDef(id);
        this.store.dispatch(request);
    };
    ViewHumanTaskDef.prototype.createHumanTaskInstance = function () {
        var _this = this;
        if (!this.humanTaskDef) {
            return;
        }
        var dialogRef = this.dialog.open(CreateHumanTaskInstanceDialog, {
            width: '800px',
            data: this.humanTaskDef
        });
        dialogRef.afterClosed().subscribe(function (cmd) {
            if (!cmd) {
                return;
            }
            _this.store.dispatch(new fromHumanTaskInstActions.CreateMeHumanTaskInstanceOperation(cmd));
        });
    };
    ViewHumanTaskDef = __decorate([
        Component({
            selector: 'view-humantaskdef-component',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store,
            ScannedActionsSubject,
            MatSnackBar,
            TranslateService,
            MatDialog,
            ActivatedRoute])
    ], ViewHumanTaskDef);
    return ViewHumanTaskDef;
}());
export { ViewHumanTaskDef };
//# sourceMappingURL=view.component.js.map