import { Component, OnInit } from '@angular/core';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'view-cmmn-xmleditor-file',
    templateUrl: './xmleditor.component.html',
    styleUrls: ['./xmleditor.component.scss']
})
export class ViewCmmnFileXmlEditorComponent implements OnInit {
    xml: string;
    editorOptions: any = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
    cmmnFile: CmmnFile = new CmmnFile();

    constructor(private store: Store<fromAppState.AppState>) { }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe((e: CmmnFile) => {
            if (!e) {
                return;
            }

            this.xml = this.formatXML(e.payload);
            this.cmmnFile = e;
        });
    }

    onSave() {
        const id = this.cmmnFile.id;
        const act = new fromCmmnFileActions.UpdateCmmnFilePayload(id, this.xml);
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
