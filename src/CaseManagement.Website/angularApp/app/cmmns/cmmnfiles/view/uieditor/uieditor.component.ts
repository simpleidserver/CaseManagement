import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import * as fromAppState from '@app/stores/appstate';
import { select, Store } from '@ngrx/store';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
let CmmnViewer = require('cmmn-js/lib/Modeler'),
    propertiesPanelModule = require('cmmn-js-properties-panel'),
    propertiesProviderModule = require('cmmn-js-properties-panel/lib/provider/cmmn');

import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';

let caseMgtCmmnModdle = require('@app/moddlextensions/casemanagement-cmmn');

@Component({
    selector: 'view-cmmn-uieditor-file',
    templateUrl: './uieditor.component.html',
    styleUrls: ['./uieditor.component.scss']
})
export class ViewCmmnFileUIEditorComponent implements OnInit {
    buildingForm: boolean = true;
    selectedElt: any = null;
    isEltSelected: boolean = false;
    xml: string;
    parameters: { key: string, value: string }[] = [];
    updatePropertiesForm: FormGroup = new FormGroup({
        id: new FormControl(''),
        name: new FormControl(''),
        implementation: new FormControl(''),
        formId: new FormControl('')
    });
    addParameterForm: FormGroup = new FormGroup({
        key: new FormControl(''),
        value: new FormControl('')
    });
    editorOptions: any = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
    viewer: any;
    cmmnFile: CmmnFile = new CmmnFile();

    constructor(private store: Store<fromAppState.AppState>) {
    }

    ngOnInit() {
        const self = this;
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
        const evtBus = this.viewer.get('eventBus');
        evtBus.on('element.click', function (evt: any) {
            self.updateProperties(evt.element);
        });
        this.updatePropertiesForm.valueChanges.subscribe(() => {
            if (self.buildingForm) {
                return;
            }

            self.saveProperties(this.updatePropertiesForm.value);
        });
        this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe((e: CmmnFile) => {
            if (!e) {
                return;
            }

            this.xml = e.payload;
            this.viewer.importXML(e.payload);
            this.cmmnFile = e;
        });
    }

    onSave() {
        const self = this;
        this.viewer.saveXML({}, function (e: any, x: any) {
            if (e) {
                return;
            }

            const id = self.cmmnFile.id;
            const act = new fromCmmnFileActions.UpdateCmmnFilePayload(id, x);
            self.store.dispatch(act);
        });
    }

    updateProperties(elt: any) {
        this.buildingForm = true;
        this.selectedElt = elt;
        this.isEltSelected = true;
        if (!elt || !elt.businessObject || !elt.businessObject.definitionRef) {
            return;
        }

        const self = this;
        var defRef = elt.businessObject.definitionRef;
        this.updatePropertiesForm.get('id').setValue(defRef.id);
        this.updatePropertiesForm.get('name').setValue(elt.businessObject.name);
        if (defRef.$type === 'cmmn:HumanTask') {
            const parameters = this.getExtension(defRef, 'cmg:Parameters');
            this.updatePropertiesForm.get('implementation').setValue(defRef.get('cmg:implementation'));
            this.updatePropertiesForm.get('formId').setValue(defRef.get('cmg:formId'));
            self.parameters = [];
            if (parameters && parameters.parameter) {
                parameters.parameter.forEach(function (p: any) {
                    self.parameters.push({ key: p.key, value: p.value });
                });
            }
        }

        this.buildingForm = false;
    }

    saveProperties(form: any) {
        if (!this.selectedElt || !this.selectedElt.businessObject || !this.selectedElt.businessObject.definitionRef) {
            return;
        }

        const moddle = this.viewer.get('moddle');
        const modeling = this.viewer.get('modeling');
        const obj: any = {
            id: form.id
        };
        const defRef = this.selectedElt.businessObject.definitionRef;
        if (defRef.$type === 'cmmn:HumanTask') {
            obj['cmg:implementation'] = form.implementation;
            obj['cmg:formId'] = form.formId;
            console.log(this.selectedElt);
            let extensionElements = defRef.extensionElements || moddle.create('cmmn:ExtensionElements');
            let parameters = this.getExtension(defRef, 'cmg:Parameters');
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

            obj['cmmn:extensionElements'] = extensionElements;
        }

        modeling.updateProperties(defRef, obj);
        modeling.updateProperties(this.selectedElt, {
            name: form.name
        });
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

        const defRef = this.selectedElt.businessObject.definitionRef;
        if (defRef.$type === 'cmmn:HumanTask') {
            const index = this.parameters.indexOf(elt);
            this.parameters.splice(index, 1);
            this.saveProperties(this.updatePropertiesForm.value);
        }
    }

    addParameter(form: any) {
        if (!this.selectedElt) {
            return;
        }

        const defRef = this.selectedElt.businessObject.definitionRef;
        if (defRef.$type === 'cmmn:HumanTask') {
            this.parameters.push(form);
            this.addParameterForm.reset();
            this.saveProperties(this.updatePropertiesForm.value);
        }
    }
}
