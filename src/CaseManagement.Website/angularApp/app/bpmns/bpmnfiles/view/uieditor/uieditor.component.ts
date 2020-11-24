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
    selectedElt: any = null;
    parameters: { key: string, value: string }[] = [];
    isEltSelected: boolean = false;
    updatePropertiesForm: FormGroup = new FormGroup({
        id: new FormControl(''),
        name: new FormControl(''),
        implementation: new FormControl(''),
        className: new FormControl(''),
        wsHumanTaskDefName: new FormControl('')
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
    }

    onSave() {
        const self = this;
        this.viewer.saveXML({}, function (e: any, x: any) {
            console.log(e);
            console.log(x);
            if (e) {
                return;
            }

            const id = self.bpmnFile.id;
            const act = new fromBpmnFileActions.UpdateBpmnFilePayload(id, x);
            self.store.dispatch(act);
        });
    }

    updateProperties(elt: any) {
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
    }

    saveProperties(form: any) {
        if (!this.selectedElt) {
            return;
        }

        const modeling = this.viewer.get('modeling');
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
            const parameters = this.getExtension(bo, 'cmg:Parameters');
            const moddle = this.viewer.get('moddle');
            parameters.parameter = [];
            this.parameters.forEach(function (p: any) {
                let parameter = moddle.create('cmg:Parameter');
                parameter.key = p.key;
                parameter.value = p.value;
                parameters.parameter.push(parameter);
            });
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
