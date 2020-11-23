import { Component, OnInit } from '@angular/core';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'view-bpmn-xmleditor-file',
    templateUrl: './xmleditor.component.html',
    styleUrls: ['./xmleditor.component.scss']
})
export class ViewBpmnFileXMLEditorComponent implements OnInit {
    xml: string;
    editorOptions: any = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
    bpmnFile: BpmnFile = new BpmnFile();

    constructor(private store: Store<fromAppState.AppState>) { }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe((e: BpmnFile) => {
            if (!e) {
                return;
            }

            this.xml = this.formatXML(e.payload);
            this.bpmnFile = e;
        });
    }

    onSave() {
        const id = this.bpmnFile.id;
        const act = new fromBpmnFileActions.UpdateBpmnFilePayload(id, this.xml);
        this.store.dispatch(act);
    }

    private formatXML(xml: any) {
        const PADDING = ' '.repeat(2);
        const reg = /(>)(<)(\/*)/g;
        let pad = 0;
        xml = xml.replace(reg, '$1\r\n$2$3');
        return xml.split('\r\n').map((node: any) => {
            let indent = 0;
            if (node.match(/.+<\/\w[^>]*>$/)) {
                indent = 0;
            } else if (node.match(/^<\/\w/) && pad > 0) {
                pad -= 1;
            } else if (node.match(/^<\w[^>]*[^\/]>.*$/)) {
                indent = 1;
            } else {
                indent = 0;
            }

            pad += indent;

            return PADDING.repeat(pad - indent) + node;
        }).join('\r\n');
    }
}
