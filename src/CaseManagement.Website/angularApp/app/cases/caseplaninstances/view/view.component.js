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
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { StartGet } from '../actions/caseplaninstance';
import { CasePlanInstance } from '../models/caseplaninstance.model';
import * as fromCasePlanInstance from '../reducers';
import { CasePlanInstanceService } from '../services/caseplaninstance.service';
import { FormBuilder } from '@angular/forms';
var FormElementView = (function () {
    function FormElementView() {
    }
    return FormElementView;
}());
export { FormElementView };
var FormView = (function () {
    function FormView() {
        this.Elements = [];
    }
    return FormView;
}());
export { FormView };
var ViewCasePlanInstanceComponent = (function () {
    function ViewCasePlanInstanceComponent(store, route, casePlanInstanceService, translateService, snackBar, formBuilder) {
        this.store = store;
        this.route = route;
        this.casePlanInstanceService = casePlanInstanceService;
        this.translateService = translateService;
        this.snackBar = snackBar;
        this.formBuilder = formBuilder;
        this.form = null;
        this.casePlanInstance = new CasePlanInstance();
        this.activeActivities = [];
        this.enableActivities = [];
        this.completeActivities = [];
    }
    ViewCasePlanInstanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromCasePlanInstance.selectGetResult)).subscribe(function (casePlanInstance) {
            if (!casePlanInstance) {
                return;
            }
            _this.casePlanInstance = casePlanInstance;
            _this.activeActivities = casePlanInstance.Elements.filter(function (cp) {
                return cp.State == "Active";
            });
            _this.enableActivities = casePlanInstance.Elements.filter(function (cp) {
                return cp.State == "Enabled";
            });
            _this.completeActivities = casePlanInstance.Elements.filter(function (cp) {
                return cp.State == "Completed";
            });
        });
        this.refresh();
    };
    ViewCasePlanInstanceComponent.prototype.onSubmit = function (v) {
        var _this = this;
        this.casePlanInstanceService.confirmForm(this.form.CasePlanInstanceId, this.form.CasePlanElementInstanceId, v).subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('FORM_IS_SUBMITTED'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_SUBMIT_THE_FORM'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    };
    ViewCasePlanInstanceComponent.prototype.enable = function (casePlanElementInstance) {
        var _this = this;
        this.casePlanInstanceService.enable(this.casePlanInstance.Id, casePlanElementInstance.Id).subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('ACTIVITY_IS_ENABLED'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, function () {
            _this.snackBar.open(_this.translateService.instant('CANNOT_ENABLE_ACTIVITY'), _this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    };
    ViewCasePlanInstanceComponent.prototype.viewForm = function (casePlanElementInstance) {
        var lng = this.translateService.getDefaultLang();
        var formView = new FormView();
        formView.CasePlanInstanceId = this.casePlanInstance.Id;
        formView.CasePlanElementInstanceId = casePlanElementInstance.Id;
        var form = casePlanElementInstance.Form;
        form.Titles.filter(function (tr) {
            if (tr.Language == lng) {
                formView.Title = tr.Value;
            }
        });
        var obj = {};
        form.Elements.forEach(function (e) {
            var formElementView = new FormElementView();
            obj[e.Id] = '';
            formElementView.Id = e.Id;
            formElementView.IsRequired = e.IsRequired;
            formElementView.Type = e.Type;
            e.Titles.filter(function (tr) {
                if (tr.Language == lng) {
                    formElementView.Title = tr.Value;
                }
            });
            e.Descriptions.filter(function (tr) {
                if (tr.Language == lng) {
                    formElementView.Description = tr.Value;
                }
            });
            formView.Elements.push(formElementView);
        });
        this.formGroup = this.formBuilder.group(obj);
        this.form = formView;
    };
    ViewCasePlanInstanceComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var request = new StartGet(id);
        this.store.dispatch(request);
    };
    ViewCasePlanInstanceComponent = __decorate([
        Component({
            selector: 'view-case-instances',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store, ActivatedRoute, CasePlanInstanceService, TranslateService, MatSnackBar, FormBuilder])
    ], ViewCasePlanInstanceComponent);
    return ViewCasePlanInstanceComponent;
}());
export { ViewCasePlanInstanceComponent };
//# sourceMappingURL=view.component.js.map