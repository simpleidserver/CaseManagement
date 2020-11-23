import { Component, OnInit } from '@angular/core';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import { select, Store } from '@ngrx/store';
let BpmnViewer = require('bpmn-js/lib/Modeler'),
    propertiesPanelModule = require('bpmn-js-properties-panel'),
    propertiesProviderModule = require('bpmn-js-properties-panel/lib/provider/bpmn');

@Component({
    selector: 'view-bpmn-uieditor-file',
    templateUrl: './uieditor.component.html',
    styleUrls: ['./uieditor.component.scss']
})
export class ViewBpmnFileUIEditorComponent implements OnInit {
    viewer: any;
    bpmnFile: BpmnFile = new BpmnFile();

    constructor(private store: Store<fromAppState.AppState>) { }

    ngOnInit() {
        this.viewer = new BpmnViewer.default({
            additionalModules: [
                propertiesPanelModule,
                propertiesProviderModule
            ],
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        let evtBus = this.viewer.get('eventBus');
        evtBus.on('element.click', function (evt: any) {
            console.log(evt);
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
            if (e) {
                return;
            }

            const id = self.bpmnFile.id;
            const act = new fromBpmnFileActions.UpdateBpmnFilePayload(id, x);
            self.store.dispatch(act);
        });
    }
}
