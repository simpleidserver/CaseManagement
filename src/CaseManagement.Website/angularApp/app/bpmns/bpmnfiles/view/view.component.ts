import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { select, Store } from '@ngrx/store';
let BpmnViewer = require('bpmn-js/lib/Modeler');

@Component({
    selector: 'view-bpmn-file',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewBpmnFileComponent implements OnInit {
    isEditorDisplayed: boolean = true;
    xml: string;
    saveForm: FormGroup;
    editorOptions: any = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
    viewer: any;
    bpmnFile: BpmnFile = new BpmnFile();

    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder) {
        this.saveForm = this.formBuilder.group({
            id: new FormControl({ value: '', disabled: true }),
            name: new FormControl({ value: '' }),
            createDateTime: new FormControl({ value: '', disabled: true }),
            updateDateTime: new FormControl({ value: '', disabled: true }),
            description: new FormControl({ value: '' })
        });
    }

    ngOnInit() {
        console.log(BpmnViewer);
        this.viewer = new BpmnViewer.default({
            additionalModules: [
            ],
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe((e: BpmnFile) => {
            if (!e) {
                return;
            }

            this.saveForm.controls['id'].setValue(e.id);
            this.saveForm.controls['name'].setValue(e.name);
            this.saveForm.controls['createDateTime'].setValue(e.createDateTime);
            this.saveForm.controls['updateDateTime'].setValue(e.updateDateTime);
            this.saveForm.controls['description'].setValue(e.description);
            this.xml = e.payload;
            this.viewer.importXML(e.payload);
            this.bpmnFile = e;
        });
        this.refresh();
    }

    navigate(isEditorDisplayed: boolean) {
        let self = this;
        this.isEditorDisplayed = isEditorDisplayed;
        if (!this.isEditorDisplayed) {
            this.viewer.saveXML({}, function (e: any, x: any) {
                if (e) {
                    return;
                }

                self.xml = self.formatXML(x);
            });
        }

        return false;
    }

    onXmlChange(evt: any) {
        this.viewer.importXML(evt);
    }

    onSave() {
        this.viewer.saveXML({}, function (e: any) {
            if (e) {
                return;
            }
        });
    }

    onPublish(e: any) {
        e.preventDefault();
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromBpmnFileActions.GetBpmnFile(id);
        this.store.dispatch(request);
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
