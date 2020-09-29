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
import { FormBuilder, FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCaseFileActions from '@app/stores/casefiles/actions/case-files.actions';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { CaseFile } from '../../../stores/casefiles/models/case-file.model';
var CmmnViewer = require('cmmn-js/lib/Modeler'), propertiesPanelModule = require('casemanagement-js-properties-panel'), propertiesProviderModule = require('casemanagement-js-properties-panel/lib/provider/casemanagement'), caseModdle = require('casemanagement-cmmn-moddle/resources/casemanagement'), cmmnModdle = require('casemanagement-cmmn-moddle/resources/cmmn');
var ViewCaseFilesComponent = (function () {
    function ViewCaseFilesComponent(store, route, formBuilder, snackBar, translateService, actions$, router) {
        this.store = store;
        this.route = route;
        this.formBuilder = formBuilder;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.actions$ = actions$;
        this.router = router;
        this.isEditorDisplayed = true;
        this.editorOptions = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
        this.caseFile = new CaseFile();
        this.saveForm = this.formBuilder.group({
            id: new FormControl({ value: '', disabled: true }),
            name: new FormControl({ value: '' }),
            createDateTime: new FormControl({ value: '', disabled: true }),
            updateDateTime: new FormControl({ value: '', disabled: true }),
            description: new FormControl({ value: '' })
        });
    }
    ViewCaseFilesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.viewer = new CmmnViewer({
            additionalModules: [
                propertiesPanelModule,
                propertiesProviderModule
            ],
            container: "#canvas",
            keyboard: {
                bindTo: window
            },
            propertiesPanel: {
                parent: '#properties'
            },
            moddleExtensions: {
                case: caseModdle,
                cmmn: cmmnModdle
            }
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseFileActions.ActionTypes.COMPLETE_UPDATE_CASEFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CASE_FILE_SAVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseFileActions.ActionTypes.ERROR_UPDATE_CASEFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('ERROR_SAVE_CASE_FILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseFileActions.ActionTypes.COMPLETE_PUBLISH_CASEFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('PUBLISH_CASE_FILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCaseFileActions.ActionTypes.ERROR_PUBLISH_CASEFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('ERROR_PUBLISH_CASE_FILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.router.navigate(["/cases/casefiles/" + _this.caseFile.id]);
        });
        this.store.pipe(select(fromAppState.selectCaseFileResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.saveForm.controls['id'].setValue(e.id);
            _this.saveForm.controls['name'].setValue(e.name);
            _this.saveForm.controls['createDateTime'].setValue(e.createDateTime);
            _this.saveForm.controls['updateDateTime'].setValue(e.updateDateTime);
            _this.saveForm.controls['description'].setValue(e.description);
            _this.xml = e.payload;
            _this.viewer.importXML(e.payload);
            _this.caseFile = e;
        });
        this.refresh();
    };
    ViewCaseFilesComponent.prototype.navigate = function (isEditorDisplayed) {
        var self = this;
        this.isEditorDisplayed = isEditorDisplayed;
        if (!this.isEditorDisplayed) {
            this.viewer.saveXML({}, function (e, x) {
                if (e) {
                    return;
                }
                self.xml = self.formatXML(x);
            });
        }
        return false;
    };
    ViewCaseFilesComponent.prototype.onXmlChange = function (evt) {
        this.viewer.importXML(evt);
    };
    ViewCaseFilesComponent.prototype.onSave = function (form) {
        var self = this;
        this.viewer.saveXML({}, function (e, x) {
            if (e) {
                return;
            }
            var id = self.saveForm.get('id').value;
            var act = new fromCaseFileActions.UpdateCaseFile(id, form.name, form.description, x);
            self.store.dispatch(act);
        });
    };
    ViewCaseFilesComponent.prototype.onPublish = function (e) {
        e.preventDefault();
        var id = this.route.snapshot.params['id'];
        var act = new fromCaseFileActions.PublishCaseFile(id);
        this.store.dispatch(act);
    };
    ViewCaseFilesComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var request = new fromCaseFileActions.GetCaseFile(id);
        this.store.dispatch(request);
    };
    ViewCaseFilesComponent.prototype.formatXML = function (xml) {
        var PADDING = ' '.repeat(2);
        var reg = /(>)(<)(\/*)/g;
        var pad = 0;
        xml = xml.replace(reg, '$1\r\n$2$3');
        return xml.split('\r\n').map(function (node) {
            var indent = 0;
            if (node.match(/.+<\/\w[^>]*>$/)) {
                indent = 0;
            }
            else if (node.match(/^<\/\w/) && pad > 0) {
                pad -= 1;
            }
            else if (node.match(/^<\w[^>]*[^\/]>.*$/)) {
                indent = 1;
            }
            else {
                indent = 0;
            }
            pad += indent;
            return PADDING.repeat(pad - indent) + node;
        }).join('\r\n');
    };
    ViewCaseFilesComponent = __decorate([
        Component({
            selector: 'view-case-file',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            FormBuilder,
            MatSnackBar,
            TranslateService,
            ScannedActionsSubject,
            Router])
    ], ViewCaseFilesComponent);
    return ViewCaseFilesComponent;
}());
export { ViewCaseFilesComponent };
//# sourceMappingURL=view.component.js.map