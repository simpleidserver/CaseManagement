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
import { FormGroup, FormControl } from '@angular/forms';
import * as fromAppState from '@app/stores/appstate';
import { select, Store } from '@ngrx/store';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
var CmmnViewer = require('cmmn-js/lib/Modeler'), propertiesPanelModule = require('cmmn-js-properties-panel'), propertiesProviderModule = require('cmmn-js-properties-panel/lib/provider/cmmn');
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
var caseMgtCmmnModdle = require('@app/moddlextensions/casemanagement-cmmn');
var ViewCmmnFileUIEditorComponent = (function () {
    function ViewCmmnFileUIEditorComponent(store) {
        this.store = store;
        this.buildingForm = true;
        this.selectedElt = null;
        this.isEltSelected = false;
        this.parameters = [];
        this.updatePropertiesForm = new FormGroup({
            id: new FormControl(''),
            name: new FormControl(''),
            implementation: new FormControl(''),
            formId: new FormControl('')
        });
        this.addParameterForm = new FormGroup({
            key: new FormControl(''),
            value: new FormControl('')
        });
        this.editorOptions = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
        this.cmmnFile = new CmmnFile();
    }
    ViewCmmnFileUIEditorComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
        this.viewer = new CmmnViewer({
            additionalModules: [
                propertiesPanelModule,
                propertiesProviderModule
            ],
            container: "#canvas",
            keyboard: {
                bindTo: window
            },
            moddleExtensions: {
                cmg: caseMgtCmmnModdle
            }
        });
        var evtBus = this.viewer.get('eventBus');
        evtBus.on('element.click', function (evt) {
            self.updateProperties(evt.element);
        });
        this.updatePropertiesForm.valueChanges.subscribe(function () {
            if (self.buildingForm) {
                return;
            }
            self.saveProperties(_this.updatePropertiesForm.value);
        });
        this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.xml = e.payload;
            _this.viewer.importXML(e.payload);
            _this.cmmnFile = e;
        });
    };
    ViewCmmnFileUIEditorComponent.prototype.onSave = function () {
        var self = this;
        this.viewer.saveXML({}, function (e, x) {
            if (e) {
                return;
            }
            var id = self.cmmnFile.id;
            var act = new fromCmmnFileActions.UpdateCmmnFilePayload(id, x);
            self.store.dispatch(act);
        });
    };
    ViewCmmnFileUIEditorComponent.prototype.updateProperties = function (elt) {
        this.buildingForm = true;
        this.selectedElt = elt;
        this.isEltSelected = true;
        if (!elt || !elt.businessObject || !elt.businessObject.definitionRef) {
            return;
        }
        var self = this;
        var defRef = elt.businessObject.definitionRef;
        this.updatePropertiesForm.get('id').setValue(defRef.id);
        this.updatePropertiesForm.get('name').setValue(elt.businessObject.name);
        if (defRef.$type === 'cmmn:HumanTask') {
            var parameters = this.getExtension(defRef, 'cmg:Parameters');
            this.updatePropertiesForm.get('implementation').setValue(defRef.get('cmg:implementation'));
            this.updatePropertiesForm.get('formId').setValue(defRef.get('cmg:formId'));
            self.parameters = [];
            if (parameters && parameters.parameter) {
                parameters.parameter.forEach(function (p) {
                    self.parameters.push({ key: p.key, value: p.value });
                });
            }
        }
        this.buildingForm = false;
    };
    ViewCmmnFileUIEditorComponent.prototype.saveProperties = function (form) {
        if (!this.selectedElt || !this.selectedElt.businessObject || !this.selectedElt.businessObject.definitionRef) {
            return;
        }
        var moddle = this.viewer.get('moddle');
        var modeling = this.viewer.get('modeling');
        var obj = {
            id: form.id
        };
        var defRef = this.selectedElt.businessObject.definitionRef;
        if (defRef.$type === 'cmmn:HumanTask') {
            obj['cmg:implementation'] = form.implementation;
            obj['cmg:formId'] = form.formId;
            console.log(this.selectedElt);
            var extensionElements = defRef.extensionElements || moddle.create('cmmn:ExtensionElements');
            var parameters_1 = this.getExtension(defRef, 'cmg:Parameters');
            if (!parameters_1) {
                parameters_1 = moddle.create('cmg:Parameters');
                extensionElements.get('values').push(parameters_1);
            }
            parameters_1.parameter = [];
            this.parameters.forEach(function (p) {
                var parameter = moddle.create('cmg:Parameter');
                parameter.key = p.key;
                parameter.value = p.value;
                parameters_1.parameter.push(parameter);
            });
            obj['cmmn:extensionElements'] = extensionElements;
        }
        modeling.updateProperties(defRef, obj);
        modeling.updateProperties(this.selectedElt, {
            name: form.name
        });
    };
    ViewCmmnFileUIEditorComponent.prototype.getExtension = function (elt, type) {
        if (!elt.extensionElements) {
            return null;
        }
        return elt.extensionElements.values.filter(function (e) {
            return e.$type === type;
        })[0];
    };
    ViewCmmnFileUIEditorComponent.prototype.removeParameter = function (elt) {
        if (!this.selectedElt) {
            return;
        }
        var defRef = this.selectedElt.businessObject.definitionRef;
        if (defRef.$type === 'cmmn:HumanTask') {
            var index = this.parameters.indexOf(elt);
            this.parameters.splice(index, 1);
            this.saveProperties(this.updatePropertiesForm.value);
        }
    };
    ViewCmmnFileUIEditorComponent.prototype.addParameter = function (form) {
        if (!this.selectedElt) {
            return;
        }
        var defRef = this.selectedElt.businessObject.definitionRef;
        if (defRef.$type === 'cmmn:HumanTask') {
            this.parameters.push(form);
            this.addParameterForm.reset();
            this.saveProperties(this.updatePropertiesForm.value);
        }
    };
    ViewCmmnFileUIEditorComponent = __decorate([
        Component({
            selector: 'view-cmmn-uieditor-file',
            templateUrl: './uieditor.component.html',
            styleUrls: ['./uieditor.component.scss']
        }),
        __metadata("design:paramtypes", [Store])
    ], ViewCmmnFileUIEditorComponent);
    return ViewCmmnFileUIEditorComponent;
}());
export { ViewCmmnFileUIEditorComponent };
//# sourceMappingURL=uieditor.component.js.map