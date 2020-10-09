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
import { FormBuilder } from '@angular/forms';
import { MatSnackBar, MatTableDataSource } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDefinitionDeadLine } from '@app/stores/humantaskdefs/models/humantaskdef-deadlines';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ParameterType = (function () {
    function ParameterType(type, displayName) {
        this.type = type;
        this.displayName = displayName;
    }
    return ParameterType;
}());
export { ParameterType };
var ViewHumanTaskDefDeadlinesComponent = (function () {
    function ViewHumanTaskDefDeadlinesComponent(store, formBuilder, actions$, snackBar, translateService) {
        this.store = store;
        this.formBuilder = formBuilder;
        this.actions$ = actions$;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.baseTranslationKey = "HUMANTASK.DEF.VIEW.DEADLINES";
        this.humanTaskDef = new HumanTaskDef();
        this.displayedDeadLineColumns = ['name', 'for', 'until'];
        this.startDeadlines = new MatTableDataSource();
        this.completionDeadlines = new MatTableDataSource();
        this.addDeadlineForm = this.formBuilder.group({
            name: '',
            deadlineType: '',
            validityType: '',
            validity: ''
        });
    }
    ViewHumanTaskDefDeadlinesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_START_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.START_DEADLINE_ADDED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.COMPLETION_DEADLINE_ADDED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant(_this.baseTranslationKey + '.COMPLETION_DEADLINE_ADDED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.humanTaskDef = e;
            _this.startDeadlines.data = _this.humanTaskDef.deadLines.startDeadLines;
            _this.completionDeadlines.data = _this.humanTaskDef.deadLines.completionDeadLines;
        });
    };
    ViewHumanTaskDefDeadlinesComponent.prototype.addDeadLine = function (form) {
        var deadline = new HumanTaskDefinitionDeadLine();
        deadline.name = form.name;
        if (form.validityType === 'duration') {
            deadline.for = form.validity;
        }
        else {
            deadline.until = form.validity;
        }
        if (form.deadlineType === 'start') {
            var request = new fromHumanTaskDefActions.AddStartDeadLine(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        }
        else {
            var request = new fromHumanTaskDefActions.AddCompletionDeadLine(this.humanTaskDef.id, deadline);
            this.store.dispatch(request);
        }
        this.addDeadlineForm.reset();
    };
    ViewHumanTaskDefDeadlinesComponent = __decorate([
        Component({
            selector: 'view-humantaskdef-deadlines-component',
            templateUrl: './deadlines.component.html',
            styleUrls: ['./deadlines.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store,
            FormBuilder,
            ScannedActionsSubject,
            MatSnackBar,
            TranslateService])
    ], ViewHumanTaskDefDeadlinesComponent);
    return ViewHumanTaskDefDeadlinesComponent;
}());
export { ViewHumanTaskDefDeadlinesComponent };
//# sourceMappingURL=deadlines.component.js.map