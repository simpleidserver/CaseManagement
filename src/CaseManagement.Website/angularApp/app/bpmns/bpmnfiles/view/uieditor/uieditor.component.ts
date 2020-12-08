import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import { select, Store } from '@ngrx/store';
let BpmnViewer = require('bpmn-js/lib/Modeler'),
    propertiesPanelModule = require('bpmn-js-properties-panel'),
    propertiesProviderModule = require('bpmn-js-properties-panel/lib/provider/bpmn');

let caseMgtBpmnModdle = require('@app/moddlextensions/casemanagement-bpmn');

@Component({
    selector: 'view-bpmn-uieditor-file',
    templateUrl: './uieditor.component.html',
    styleUrls: ['./uieditor.component.scss']
})
export class ViewBpmnFileUIEditorComponent implements OnInit {
    viewer: any;
    bpmnFile: BpmnFile = new BpmnFile();
    buildingForm: boolean = true;
    selectedElt: any = null;
    parameters: { key: string, value: string }[] = [];
    outgoingElts: string[] = [];
    isEltSelected: boolean = false;
    updatePropertiesForm: FormGroup = new FormGroup({
        id: new FormControl(''),
        name: new FormControl(''),
        implementation: new FormControl(''),
        className: new FormControl(''),
        wsHumanTaskDefName: new FormControl(''),
        default: new FormControl(''),
        gatewayDirection: new FormControl(''),
        sequenceFlowCondition: new FormControl('')
    });
    addParameterForm: FormGroup = new FormGroup({
        key: new FormControl(''),
        value: new FormControl('')
    });

    constructor(private store: Store<fromAppState.AppState>) { }

    ngOnInit() {
        const self = this;
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
        const evtBus = this.viewer.get('eventBus');
        evtBus.on('element.click', function (evt: any) {
            self.updateProperties(evt.element);
        });

        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe((e: BpmnFile) => {
            if (!e) {
                return;
            }

            this.viewer.importXML(e.payload);
            this.bpmnFile = e;
        });
        this.updatePropertiesForm.valueChanges.subscribe(() => {
            if (self.buildingForm) {
                return;
            }

            self.saveProperties(this.updatePropertiesForm.value);
        });
        this.updatePropertiesForm.get('default').valueChanges.subscribe(() => {
            const selectedPath = self.updatePropertiesForm.get('default').value;
            if (!selectedPath || !self.selectedElt) {
                return;
            }

            const elementRegistry = self.viewer.get('elementRegistry');
            const connection = elementRegistry.get(selectedPath);
            const modeling = self.viewer.get('modeling');
            if (self.selectedElt.outgoing) {
                modeling.setColor(self.selectedElt.outgoing, {
                    stroke: 'black'
                });
            }

            modeling.setColor([connection], {
                stroke: 'orange'
            });
        });
    }

    onSave() {
        const self = this;
        this.viewer.saveXML({}, function (e: any, x: any) {
            if (e) {
                return;
            }

            const id = self.bpmnFile.id;
            const act = new fromBpmnFileActions.UpdateBpmnFilePayload(id, x);
            self.store.dispatch(act);
        });
    }

    updateProperties(elt: any) {
        this.buildingForm = true;
        this.updatePropertiesForm.reset();
        this.selectedElt = elt;
        this.isEltSelected = true;
        const self = this;
        const bo = elt.businessObject;
        this.updatePropertiesForm.get('id').setValue(bo.id);
        this.updatePropertiesForm.get('name').setValue(bo.name);
        if (bo.$type === 'bpmn:ServiceTask') {
            this.updatePropertiesForm.get('implementation').setValue(bo.implementation);
            this.updatePropertiesForm.get('className').setValue(bo.get('cmg:className'));
        }

        if (bo.$type === 'bpmn:UserTask') {
            this.updatePropertiesForm.get('implementation').setValue(bo.implementation);
            this.updatePropertiesForm.get('wsHumanTaskDefName').setValue(bo.get('cmg:wsHumanTaskDefName'));
            const parameters = this.getExtension(bo, 'cmg:Parameters');
            self.parameters = [];
            if (parameters && parameters.parameter) {
                parameters.parameter.forEach(function (p: any) {
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
                this.outgoingElts = bo.outgoing.map(function (o: any) {
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
    }

    saveProperties(form: any) {
        if (!this.selectedElt) {
            return;
        }

        const moddle = this.viewer.get('moddle');
        const modeling = this.viewer.get('modeling');
        const elementRegistry = this.viewer.get('elementRegistry');
        const obj : any = {
            id: form.id,
            name: form.name
        };

        const bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:ServiceTask') {
            obj['cmg:className'] = form.className;
            obj['implementation'] = form.implementation;
        }

        if (bo.$type === 'bpmn:UserTask') {
            obj['implementation'] = form.implementation;
            obj['cmg:wsHumanTaskDefName'] = form.wsHumanTaskDefName;
            let extensionElements = bo.extensionElements || moddle.create('bpmn:ExtensionElements');
            let parameters = this.getExtension(bo, 'cmg:Parameters');
            if (!parameters) {
                parameters = moddle.create('cmg:Parameters');
                extensionElements.get('values').push(parameters);
            }

            parameters.parameter = [];
            this.parameters.forEach(function (p: any) {
                let parameter = moddle.create('cmg:Parameter');
                parameter.key = p.key;
                parameter.value = p.value;
                parameters.parameter.push(parameter);
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
    }

    getExtension(elt: any, type: string) {
        if (!elt.extensionElements) {
            return null;
        }

        return elt.extensionElements.values.filter(function (e: any) {
            return e.$type === type;
        })[0];
    }

    removeParameter(elt: any) {
        if (!this.selectedElt) {
            return;
        }

        const bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:UserTask') {
            const index = this.parameters.indexOf(elt);
            this.parameters.splice(index, 1);
        }
    }

    addParameter(form: any) {
        if (!this.selectedElt) {
            return;
        }

        const bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:UserTask') {
            this.parameters.push(form);
            this.addParameterForm.reset();
        }
    }
}
