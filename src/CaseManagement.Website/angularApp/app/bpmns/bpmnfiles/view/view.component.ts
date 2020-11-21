import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { filter } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { SidenavService } from '@app/shared/SidenavService';
let BpmnViewer = require('bpmn-js/lib/Modeler'),
    propertiesPanelModule = require('bpmn-js-properties-panel'),
    propertiesProviderModule = require('bpmn-js-properties-panel/lib/provider/bpmn');

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
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private formBuilder: FormBuilder,
        private router: Router,
        private sidenavService: SidenavService) {
        this.saveForm = this.formBuilder.group({
            id: new FormControl({ value: '', disabled: true }),
            name: new FormControl({ value: '' }),
            createDateTime: new FormControl({ value: '', disabled: true }),
            updateDateTime: new FormControl({ value: '', disabled: true }),
            description: new FormControl({ value: '' })
        });
    }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_GET_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_GET_BPMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.BPMNFILE_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_UPDATE_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_UPDATE_BPMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.COMPLETE_PUBLISH_BPMNFILE))
            .subscribe((e) => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.BPMNFILE_PUBLISHED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.router.navigate(["/bpmns/bpmnfiles/" + e.id]);
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_PUBLISH_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_PUBLISH_BPMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.viewer = new BpmnViewer.default({
            additionalModules: [
                propertiesPanelModule,
                propertiesProviderModule
            ],
            container: "#canvas",
            keyboard: {
                bindTo: window
            },
            propertiesPanel: {
                parent: '#properties'
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
        this.sidenavService.close();
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

    onSave(form: any) {
        const self = this;
        this.viewer.saveXML({}, function (e: any, x: any) {
            if (e) {
                return;
            }

            const id = self.saveForm.get('id').value;
            const act = new fromBpmnFileActions.UpdateBpmnFile(id, form.name, form.description, x);
            self.store.dispatch(act);
        });
    }

    onPublish(e: any) {
        e.preventDefault();
        const id = this.route.snapshot.params['id'];
        const act = new fromBpmnFileActions.PublishBpmnFile(id);
        this.store.dispatch(act);
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
