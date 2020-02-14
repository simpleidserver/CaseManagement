import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { StartGet } from '../actions/case-files';
import { CaseFile } from '../models/case-file.model';
import * as fromCaseFiles from '../reducers';
import { CaseFilesService } from '../services/casefiles.service';
let CmmnViewer = require('cmmn-js/lib/Modeler'),
    propertiesPanelModule = require('casemanagement-js-properties-panel'),
    propertiesProviderModule = require('casemanagement-js-properties-panel/lib/provider/casemanagement'),
    caseModdle = require('casemanagement-cmmn-moddle/resources/casemanagement');

@Component({
    selector: 'view-case-file',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewCaseFilesComponent implements OnInit {
    isEditorDisplayed: boolean = true;
    xml: string;
    saveForm: FormGroup;
    editorOptions: any = { theme: 'vs-dark', language: 'xml', automaticLayout: true };
    viewer: any;
    caseFile: CaseFile = new CaseFile();

    constructor(private store: Store<fromCaseFiles.CaseFilesState>, private route: ActivatedRoute, private formBuilder: FormBuilder, private caseFilesService: CaseFilesService, private snackBar: MatSnackBar, private translateService: TranslateService, private router: Router) {
        this.saveForm = this.formBuilder.group({
            id: new FormControl({ value: '', disabled: true }),
            name: new FormControl({ value: '' }),
            createDateTime: new FormControl({ value: '', disabled: true }),
            updateDateTime: new FormControl({ value: '', disabled: true }),
            description: new FormControl({ value: '' })
        });
    }

    ngOnInit() {
        this.viewer = new CmmnViewer({
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
            },
            moddleExtensions: {
                case: caseModdle
            }
        });
        this.store.pipe(select(fromCaseFiles.selectGetResult)).subscribe((e: CaseFile) => {
            if (!e) {
                return;
            }

            this.saveForm.controls['id'].setValue(e.Id);
            this.saveForm.controls['name'].setValue(e.Name);
            this.saveForm.controls['createDateTime'].setValue(e.CreateDateTime);
            this.saveForm.controls['updateDateTime'].setValue(e.UpdateDateTime);
            this.saveForm.controls['description'].setValue(e.Description);
            this.xml = e.Payload;
            this.viewer.importXML(e.Payload);
            this.caseFile = e;
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

    onSave(form: any) {
        let self = this;
        this.viewer.saveXML({}, function (e: any, x: any) {
            if (e) {
                return;
            }

            let id = self.saveForm.get('id').value;
            let cancel = self.translateService.instant('CANCEL');
            self.caseFilesService.update(id, form.name, form.description, x).subscribe(() => {
                self.snackBar.open(self.translateService.instant('CASE_FILE_SAVED'), cancel, {
                    duration: 2000
                });
            }, () => {
                self.snackBar.open(self.translateService.instant('ERROR_SAVE_CASE_FILE'), cancel, {
                    duration: 2000
                });
            });
        });
    }

    onPublish(e: any) {
        e.preventDefault();
        let self = this;
        var id = self.route.snapshot.params['id'];
        let cancel = self.translateService.instant('CANCEL');
        self.caseFilesService.publish(id).subscribe((caseFileId : string) => {
            self.snackBar.open(self.translateService.instant('PUBLISH_CASE_FILE'), cancel, {
                duration: 2000
            });
            this.router.navigate(["/cases/casefiles/" + caseFileId]);
        }, () => {
            self.snackBar.open(self.translateService.instant('ERROR_PUBLISH_CASE_FILE'), cancel, {
                duration: 2000
            });
        });
    }

    refresh() {
        var id = this.route.snapshot.params['id'];
        let request = new StartGet(id);
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
