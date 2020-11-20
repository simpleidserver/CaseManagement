import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCaseFileActions from '@app/stores/casefiles/actions/case-files.actions';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { CaseFile } from '@app/stores/casefiles/models/case-file.model';
let CmmnViewer = require('cmmn-js/lib/Modeler'),
    propertiesPanelModule = require('casemanagement-js-properties-panel'),
    propertiesProviderModule = require('casemanagement-js-properties-panel/lib/provider/casemanagement'),
    // propertiesProviderModule = require('cmmn-js-properties-panel/lib/provider/camunda'),
    caseModdle = require('casemanagement-cmmn-moddle/resources/casemanagement'),
    cmmnModdle = require('casemanagement-cmmn-moddle/resources/cmmn');

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

    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private actions$: ScannedActionsSubject,
        private router: Router) {
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
                case: caseModdle,
                cmmn: cmmnModdle
            }
        });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseFileActions.ActionTypes.COMPLETE_UPDATE_CASEFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CASE_FILE_SAVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseFileActions.ActionTypes.ERROR_UPDATE_CASEFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('ERROR_SAVE_CASE_FILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseFileActions.ActionTypes.COMPLETE_PUBLISH_CASEFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('PUBLISH_CASE_FILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseFileActions.ActionTypes.ERROR_PUBLISH_CASEFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('ERROR_PUBLISH_CASE_FILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.router.navigate(["/cases/casefiles/" + this.caseFile.id]);
            });
        this.store.pipe(select(fromAppState.selectCaseFileResult)).subscribe((e: CaseFile) => {
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

            const id = self.saveForm.get('id').value;
            const act = new fromCaseFileActions.UpdateCaseFile(id, form.name, form.description, x);
            self.store.dispatch(act);
        });
    }

    onPublish(e: any) {
        e.preventDefault();
        const id = this.route.snapshot.params['id'];
        const act = new fromCaseFileActions.PublishCaseFile(id);
        this.store.dispatch(act);
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromCaseFileActions.GetCaseFile(id);
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
