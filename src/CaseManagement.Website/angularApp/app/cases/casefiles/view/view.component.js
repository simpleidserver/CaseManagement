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
import { select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { StartGet } from '../actions/case-files';
import { CaseFile } from '../models/case-file.model';
import * as fromCaseFiles from '../reducers';
import { CaseFilesService } from '../services/casefiles.service';
var CmmnViewer = require('cmmn-js/lib/Modeler'), propertiesPanelModule = require('casemanagement-js-properties-panel'), propertiesProviderModule = require('casemanagement-js-properties-panel/lib/provider/casemanagement'), caseModdle = require('casemanagement-cmmn-moddle/resources/casemanagement');
var ViewCaseFilesComponent = (function () {
    function ViewCaseFilesComponent(store, route, formBuilder, caseFilesService, snackBar, translateService, router) {
        this.store = store;
        this.route = route;
        this.formBuilder = formBuilder;
        this.caseFilesService = caseFilesService;
        this.snackBar = snackBar;
        this.translateService = translateService;
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
                case: caseModdle
            }
        });
        this.store.pipe(select(fromCaseFiles.selectGetResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.saveForm.controls['id'].setValue(e.Id);
            _this.saveForm.controls['name'].setValue(e.Name);
            _this.saveForm.controls['createDateTime'].setValue(e.CreateDateTime);
            _this.saveForm.controls['updateDateTime'].setValue(e.UpdateDateTime);
            _this.saveForm.controls['description'].setValue(e.Description);
            _this.xml = e.Payload;
            _this.viewer.importXML(e.Payload);
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
            var cancel = self.translateService.instant('CANCEL');
            self.caseFilesService.update(id, form.name, form.description, x).subscribe(function () {
                self.snackBar.open(self.translateService.instant('CASE_FILE_SAVED'), cancel, {
                    duration: 2000
                });
            }, function () {
                self.snackBar.open(self.translateService.instant('ERROR_SAVE_CASE_FILE'), cancel, {
                    duration: 2000
                });
            });
        });
    };
    ViewCaseFilesComponent.prototype.onPublish = function (e) {
        var _this = this;
        e.preventDefault();
        var self = this;
        var id = self.route.snapshot.params['id'];
        var cancel = self.translateService.instant('CANCEL');
        self.caseFilesService.publish(id).subscribe(function (caseFileId) {
            self.snackBar.open(self.translateService.instant('PUBLISH_CASE_FILE'), cancel, {
                duration: 2000
            });
            _this.router.navigate(["/cases/casefiles/" + caseFileId]);
        }, function () {
            self.snackBar.open(self.translateService.instant('ERROR_PUBLISH_CASE_FILE'), cancel, {
                duration: 2000
            });
        });
    };
    ViewCaseFilesComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var request = new StartGet(id);
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
        __metadata("design:paramtypes", [Store, ActivatedRoute, FormBuilder, CaseFilesService, MatSnackBar, TranslateService, Router])
    ], ViewCaseFilesComponent);
    return ViewCaseFilesComponent;
}());
export { ViewCaseFilesComponent };
//# sourceMappingURL=view.component.js.map