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
import { FormControl, FormGroup } from '@angular/forms';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import { select, Store } from '@ngrx/store';
var BpmnViewer = require('bpmn-js/lib/Modeler'), propertiesPanelModule = require('bpmn-js-properties-panel'), propertiesProviderModule = require('bpmn-js-properties-panel/lib/provider/bpmn');
var caseMgtBpmnModdle = require('@app/moddlextensions/casemanagement-bpmn');
var ViewBpmnFileUIEditorComponent = (function () {
    function ViewBpmnFileUIEditorComponent(store) {
        this.store = store;
        this.bpmnFile = new BpmnFile();
        this.buildingForm = true;
        this.selectedElt = null;
        this.parameters = [];
        this.outgoingElts = [];
        this.isEltSelected = false;
        this.updatePropertiesForm = new FormGroup({
            id: new FormControl(''),
            name: new FormControl(''),
            implementation: new FormControl(''),
            className: new FormControl(''),
            wsHumanTaskDefName: new FormControl(''),
            default: new FormControl(''),
            gatewayDirection: new FormControl(''),
            sequenceFlowCondition: new FormControl('')
        });
        this.addParameterForm = new FormGroup({
            key: new FormControl(''),
            value: new FormControl('')
        });
    }
    ViewBpmnFileUIEditorComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
        this.viewer = new BpmnViewer.default({
            additionalModules: [
                propertiesPanelModule,
                propertiesProviderModule
            ],
            container: "#canvas",
            keyboard: {
                bindTo: window
            },
            moddleExtensions: {
                cmg: caseMgtBpmnModdle
            }
        });
        var evtBus = this.viewer.get('eventBus');
        evtBus.on('element.click', function (evt) {
            self.updateProperties(evt.element);
        });
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.viewer.importXML(e.payload);
            _this.bpmnFile = e;
        });
        this.updatePropertiesForm.valueChanges.subscribe(function () {
            if (self.buildingForm) {
                return;
            }
            self.saveProperties(_this.updatePropertiesForm.value);
        });
        this.updatePropertiesForm.get('default').valueChanges.subscribe(function () {
            var selectedPath = self.updatePropertiesForm.get('default').value;
            if (!selectedPath || !self.selectedElt) {
                return;
            }
            var elementRegistry = self.viewer.get('elementRegistry');
            var connection = elementRegistry.get(selectedPath);
            var modeling = self.viewer.get('modeling');
            if (self.selectedElt.outgoing) {
                modeling.setColor(self.selectedElt.outgoing, {
                    stroke: 'black'
                });
            }
            modeling.setColor([connection], {
                stroke: 'orange'
            });
        });
    };
    ViewBpmnFileUIEditorComponent.prototype.onSave = function () {
        var self = this;
        this.viewer.saveXML({}, function (e, x) {
            if (e) {
                return;
            }
            var id = self.bpmnFile.id;
            var act = new fromBpmnFileActions.UpdateBpmnFilePayload(id, x);
            self.store.dispatch(act);
        });
    };
    ViewBpmnFileUIEditorComponent.prototype.updateProperties = function (elt) {
        this.buildingForm = true;
        this.updatePropertiesForm.reset();
        this.selectedElt = elt;
        this.isEltSelected = true;
        var self = this;
        var bo = elt.businessObject;
        this.updatePropertiesForm.get('id').setValue(bo.id);
        this.updatePropertiesForm.get('name').setValue(bo.name);
        if (bo.$type === 'bpmn:ServiceTask') {
            this.updatePropertiesForm.get('implementation').setValue(bo.implementation);
            this.updatePropertiesForm.get('className').setValue(bo.get('cmg:className'));
        }
        if (bo.$type === 'bpmn:UserTask') {
            this.updatePropertiesForm.get('implementation').setValue(bo.implementation);
            this.updatePropertiesForm.get('wsHumanTaskDefName').setValue(bo.get('cmg:wsHumanTaskDefName'));
            var parameters = this.getExtension(bo, 'cmg:Parameters');
            self.parameters = [];
            if (parameters && parameters.parameter) {
                parameters.parameter.forEach(function (p) {
                    self.parameters.push({ key: p.key, value: p.value });
                });
            }
        }
        if (bo.$type === 'bpmn:ExclusiveGateway') {
            if (bo.default) {
                this.updatePropertiesForm.get('default').setValue(bo.default.id);
            }
            this.updatePropertiesForm.get('gatewayDirection').setValue(bo.gatewayDirection);
            if (bo.outgoing) {
                this.outgoingElts = bo.outgoing.map(function (o) {
                    return o.id;
                });
            }
        }
        if (bo.$type === 'bpmn:SequenceFlow') {
            if (bo.conditionExpression) {
                this.updatePropertiesForm.get('sequenceFlowCondition').setValue(bo.conditionExpression.body);
            }
        }
        this.buildingForm = false;
    };
    ViewBpmnFileUIEditorComponent.prototype.saveProperties = function (form) {
        if (!this.selectedElt) {
            return;
        }
        var moddle = this.viewer.get('moddle');
        var modeling = this.viewer.get('modeling');
        var elementRegistry = this.viewer.get('elementRegistry');
        var obj = {
            id: form.id,
            name: form.name
        };
        var bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:ServiceTask') {
            obj['cmg:className'] = form.className;
            obj['implementation'] = form.implementation;
        }
        if (bo.$type === 'bpmn:UserTask') {
            obj['implementation'] = form.implementation;
            obj['cmg:wsHumanTaskDefName'] = form.wsHumanTaskDefName;
            var extensionElements = bo.extensionElements || moddle.create('bpmn:ExtensionElements');
            var parameters_1 = this.getExtension(bo, 'cmg:Parameters');
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
        }
        if (bo.$type === 'bpmn:ExclusiveGateway') {
            if (form.default) {
                obj['default'] = elementRegistry.get(form.default).businessObject;
            }
            obj['gatewayDirection'] = form.gatewayDirection;
        }
        if (bo.$type === 'bpmn:SequenceFlow') {
            var newCondition = moddle.create('bpmn:FormalExpression', {
                body: form.sequenceFlowCondition
            });
            obj['conditionExpression'] = newCondition;
        }
        modeling.updateProperties(this.selectedElt, obj);
    };
    ViewBpmnFileUIEditorComponent.prototype.getExtension = function (elt, type) {
        if (!elt.extensionElements) {
            return null;
        }
        return elt.extensionElements.values.filter(function (e) {
            return e.$type === type;
        })[0];
    };
    ViewBpmnFileUIEditorComponent.prototype.removeParameter = function (elt) {
        if (!this.selectedElt) {
            return;
        }
        var bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:UserTask') {
            var index = this.parameters.indexOf(elt);
            this.parameters.splice(index, 1);
        }
    };
    ViewBpmnFileUIEditorComponent.prototype.addParameter = function (form) {
        if (!this.selectedElt) {
            return;
        }
        var bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:UserTask') {
            this.parameters.push(form);
            this.addParameterForm.reset();
        }
    };
    ViewBpmnFileUIEditorComponent = __decorate([
        Component({
            selector: 'view-bpmn-uieditor-file',
            templateUrl: './uieditor.component.html',
            styleUrls: ['./uieditor.component.scss']
        }),
        __metadata("design:paramtypes", [Store])
    ], ViewBpmnFileUIEditorComponent);
    return ViewBpmnFileUIEditorComponent;
}());
export { ViewBpmnFileUIEditorComponent };
//# sourceMappingURL=uieditor.component.js.map