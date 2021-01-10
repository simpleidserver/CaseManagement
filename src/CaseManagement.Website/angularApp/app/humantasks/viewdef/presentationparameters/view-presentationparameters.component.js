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
import * as fromAppState from '@app/stores/appstate';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { filter } from 'rxjs/operators';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material';
var ViewPresentationParametersComponent = (function () {
    function ViewPresentationParametersComponent(store, actions$, translateService, snackBar) {
        var _this = this;
        this.store = store;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.snackBar = snackBar;
        this.baseTranslationKey = "HUMANTASK.DEF.VIEW.PRESENTATION_PARAMETERS";
        this.names = [];
        this.subjects = [];
        this.descriptions = [];
        this.presentationParameters = [];
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('HUMANTASK.MESSAGES.PRESENTATIONPARAMETERS_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_PRESENTATIONELEMENT; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('HUMANTASK.MESSAGES.ERROR_UPDATE_PRESENTATION_PARAMETERS'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
    }
    ViewPresentationParametersComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.humanTaskDef = e;
            _this.names = HumanTaskDef.getNames(e);
            _this.subjects = HumanTaskDef.getSubjects(e);
            _this.descriptions = HumanTaskDef.getDescriptions(e);
            _this.presentationParameters = e.presentationParameters;
        });
    };
    ViewPresentationParametersComponent.prototype.update = function () {
        var presentationElts = [];
        presentationElts = presentationElts.concat(this.names, this.subjects, this.descriptions);
        var request = new fromHumanTaskDefActions.UpdatePresentationElementOperation(this.humanTaskDef.id, presentationElts, this.presentationParameters);
        this.store.dispatch(request);
    };
    ViewPresentationParametersComponent = __decorate([
        Component({
            selector: 'view-presentationparameters-component',
            templateUrl: './view-presentationparameters.component.html',
            styleUrls: ['./view-presentationparameters.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store,
            ScannedActionsSubject,
            TranslateService,
            MatSnackBar])
    ], ViewPresentationParametersComponent);
    return ViewPresentationParametersComponent;
}());
export { ViewPresentationParametersComponent };
//# sourceMappingURL=view-presentationparameters.component.js.map