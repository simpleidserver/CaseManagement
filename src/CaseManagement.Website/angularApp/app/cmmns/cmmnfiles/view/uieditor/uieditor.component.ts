import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import * as fromAppState from '@app/stores/appstate';
import { select, Store } from '@ngrx/store';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
let CmmnViewer = require('cmmn-js/lib/Modeler'),
    propertiesPanelModule = require('cmmn-js-properties-panel'),
    propertiesProviderModule = require('cmmn-js-properties-panel/lib/provider/cmmn');
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';

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
    updatePropertiesForm: FormGroup = new FormGroup({
        id: new FormControl(''),
        name: new FormControl('')
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
            moddleExtensions: { }
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

        var defRef = elt.businessObject.definitionRef;
        this.updatePropertiesForm.get('id').setValue(defRef.id);
        this.updatePropertiesForm.get('name').setValue(elt.businessObject.name);
        // if (defRef.$type === 'cmmn:HumanTask') {
        // 
        // }
        this.buildingForm = false;
    }

    saveProperties(form: any) {
        if (!this.selectedElt || !this.selectedElt.businessObject || !this.selectedElt.businessObject.definitionRef) {
            return;
        }

        const modeling = this.viewer.get('modeling');
        const obj: any = {
            id: form.id
        };
        modeling.updateProperties(this.selectedElt.businessObject.definitionRef, obj);
        modeling.updateProperties(this.selectedElt, {
            name: form.name
        });
    }
}
