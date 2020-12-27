import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FormControl, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import * as fromCmmnPlanActions from '@app/stores/cmmnplans/actions/cmmn-plans.actions';
import * as fromCmmnPlanInstanceActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { SearchCmmnFilesResult } from '@app/stores/cmmnfiles/models/search-cmmn-files-result.model';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { MatSnackBar, MatDialog, MatSort, MatPaginator } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { ViewXmlDialog } from './view-xml-dialog';
import { CmmnPlan } from '@app/stores/cmmnplans/models/cmmn-plan.model';
import { SearchCmmnPlanResult } from '@app/stores/cmmnplans/models/searchcmmnplanresult.model';
import { SearchCasePlanInstanceResult } from '@app/stores/cmmninstances/models/searchcmmnplaninstanceresult.model';
import { CmmnPlanInstanceResult } from '@app/stores/cmmninstances/models/cmmn-planinstance.model';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { CmmnFileState } from '@app/stores/cmmnfiles/reducers/cmmnfile.reducers';
import { Parameter } from '@app/stores/common/parameter.model';
let CmmnViewer = require('cmmn-js/lib/Modeler'),
    propertiesPanelModule = require('cmmn-js-properties-panel'),
    propertiesProviderModule = require('cmmn-js-properties-panel/lib/provider/cmmn');
let caseMgtCmmnModdle = require('@app/moddlextensions/casemanagement-cmmn');

declare var $: any;

@Component({
    selector: 'view-cmmnfile',
    templateUrl: './viewfile.component.html',
    styleUrls: ['./viewfile.component.scss']
})
export class ViewCmmnFileComponent implements OnInit, OnDestroy {
    cmmnPlanInstanceLstListener: any;
    cmmnFileLstListener: any;
    cmmnFileListener: any;
    displayedColumns: string[] = ['status', 'name', 'create_datetime', 'update_datetime'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    buildingForm: boolean = true;
    selectedElt: any = null;
    isEltSelected: boolean = false;
    inputParameters: Parameter[] = [];
    humanTaskDefs: HumanTaskDef[] = [];
    parameters: { key: string, value: string }[] = [];
    viewer: any;
    paramsSub: any;
    id: string;
    cmmnFiles$: CmmnFile[] = [];
    cmmnPlans$: CmmnPlan[] = [];
    cmmnPlanInstances$: CmmnPlanInstanceResult[] = [];
    cmmnFile: CmmnFile = new CmmnFile();
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
    versionFormControl: FormControl = new FormControl('');
    saveForm: FormGroup;

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private router: Router,
        private dialog : MatDialog) {
        this.saveForm = this.formBuilder.group({
            name: new FormControl({ value: '' }),
            description: new FormControl({ value: '' })
        });
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
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnPlanInstanceActions.ActionTypes.COMPLETE_LAUNCH_CMMN_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.CMMNPLANINSTANCE_LAUNCHED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnPlanInstanceActions.ActionTypes.ERROR_LAUNCH_CMMN_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_LAUNCH_CMMNPLANINSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.CMMNFILE_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.ERROR_UPDATE_CMMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_UPDATE_CMMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.COMPLETE_PUBLISH_CMMNFILE))
            .subscribe((act: any) => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.CMMNFILE_PUBLISHED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.router.navigate(['/cmmns/' + act.id]);
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.ERROR_PUBLISH_CMMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_PUBLISH_CMMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.updatePropertiesForm.valueChanges.subscribe(() => {
            if (self.buildingForm) {
                return;
            }

            self.saveProperties(this.updatePropertiesForm.value);
        });
        this.cmmnPlanInstanceLstListener = this.store.pipe(select(fromAppState.selectCmmnPlanInstanceLstResult)).subscribe((e: SearchCasePlanInstanceResult) => {
            if (!e) {
                return;
            }

            this.cmmnPlanInstances$ = e.content;
        });
        this.cmmnFileLstListener = this.store.pipe(select(fromAppState.selectCmmnFileLstResult)).subscribe((e: SearchCmmnFilesResult) => {
            if (!e) {
                return;
            }

            this.displayCanvas();
            this.cmmnFiles$ = e.content;
        });
        this.cmmnFileListener = this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe((cmmnFile: CmmnFileState) => {
            if (!cmmnFile || !cmmnFile.content) {
                return;
            }

            this.cmmnFile = cmmnFile.content;
            this.humanTaskDefs = cmmnFile.humanTaskDefs;
            this.displayCanvas();
            this.saveForm.controls['name'].setValue(cmmnFile.content.name);
            this.saveForm.controls['description'].setValue(cmmnFile.content.description);
            const request = new fromCmmnFileActions.SearchCmmnFiles("create_datetime", "desc", 10000, 0, null, cmmnFile.content.fileId, false);
            this.store.dispatch(request);
        });
        this.store.pipe(select(fromAppState.selectCmmnPlanLstResult)).subscribe((searchResult: SearchCmmnPlanResult) => {
            if (!searchResult) {
                return;
            }

            this.cmmnPlans$ = searchResult.content;
        });
        this.paramsSub = this.route.params.subscribe(() => {
            let id = this.route.snapshot.params['id'];
            this.versionFormControl.setValue(id);
            this.refresh();
        });
    }

    ngOnDestroy() {
        this.paramsSub.unsubscribe();
        this.cmmnPlanInstanceLstListener.unsubscribe();
        this.cmmnFileLstListener.unsubscribe();
        this.cmmnFileListener.unsubscribe();
    }

    displayCanvas() {
        if (!this.cmmnFile || !this.cmmnPlans$) {
            return;
        }

        const self = this;
        this.viewer.importXML(this.cmmnFile.payload, function () {
            let elementRegistry = self.viewer.get('elementRegistry');
            let overlays = self.viewer.get('overlays');
            let elts = elementRegistry.getAll();
            const stageElts = elts.filter(function (e: any) {
                return e.type === "cmmn:Stage";
            });
            let launchTemplate: any = "<div class='launch-stage' data-id='{id}'><svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='black' width='24px' height='24px'><path d='M0 0h24v24H0z' fill='none'/><path d='M19 19H5V5h7V3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.1 0 2-.9 2-2v-7h-2v7zM14 3v2h3.59l-9.83 9.83 1.41 1.41L19 6.41V10h2V3h-7z'/></svg></div>";
            stageElts.forEach(function (se: any) {
                let filtered = self.cmmnPlans$.filter(function (cmmnPlan: CmmnPlan) {
                    return cmmnPlan.casePlanId === se.id;
                });
                if (filtered.length !== 1) {
                    return;
                }

                let launch: any = launchTemplate.replace('{id}', filtered[0].id);
                launch = $(launch);
                overlays.add(se.id, {
                    position: {
                        top: 0,
                        right: 24
                    },
                    html: launch
                });
                launch.click(function () {
                    let id = $(this).data('id');
                    const request = new fromCmmnPlanInstanceActions.LaunchCmmnPlanInstance(id);
                    self.store.dispatch(request);
                });
            });
        });
    }

    updateFileVersion() {
        const self = this;
        const filtered = this.cmmnFiles$.filter(function (b: CmmnFile) {
            return b.id === self.versionFormControl.value;
        })[0];
        this.router.navigate(['/cmmns/' + filtered.id]);
    }

    onSave(form: any) {
        const self = this;
        const id = this.route.snapshot.params['id'];
        this.viewer.saveXML({}, function (e: any, x: any) {
            if (e) {
                return;
            }

            const act = new fromCmmnFileActions.UpdateCmmnFile(id, form.name, form.description, x);
            self.store.dispatch(act);
        });
    }

    onPublish(evt: any) {
        evt.preventDefault();
        const id = this.route.snapshot.params['id'];
        const act = new fromCmmnFileActions.PublishCmmnFile(id);
        this.store.dispatch(act);
    }

    viewXML() {
        const self = this;
        const id = this.route.snapshot.params['id'];
        const xml = this.cmmnFile.payload;
        const d = this.dialog.open(ViewXmlDialog, {
            data: { xml: xml },
            width: '900px'
        });
        d.afterClosed().subscribe((r: any) => {
            if (!r) {
                return;
            }

            const act = new fromCmmnFileActions.UpdateCmmnFile(id, self.saveForm.value.name, self.saveForm.value.description, r.xml);
            self.store.dispatch(act);
        });
    }

    refresh() {
        this.id = this.route.snapshot.params['id'];
        let request = new fromCmmnFileActions.GetCmmnFile(this.id);
        this.store.dispatch(request);
        let searchPlans = new fromCmmnPlanActions.SearchCmmnPlans("create_datetime", "desc", 2000, 0, this.id, false);
        this.store.dispatch(searchPlans);
        this.refreshCmmnInstances();
    }

    refreshCmmnInstances() {
        let id = this.route.snapshot.params['id'];
        let startIndex: number = 0;
        let count: number = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }

        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }

        let active = "create_datetime";
        let direction = "desc";
        if (this.sort.active) {
            active = this.sort.active;
        }

        if (this.sort.direction) {
            direction = this.sort.direction;
        }

        const searchBpmn = new fromCmmnPlanInstanceActions.SearchCmmnPlanInstance(active, direction, count, startIndex, null, id);
        this.store.dispatch(searchBpmn);
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
            this.selectHumanTask(defRef.get('cmg:formId'));
            self.parameters = [];
            if (parameters && parameters.parameter) {
                parameters.parameter.forEach(function (p: any) {
                    self.parameters.push({ key: p.key, value: p.value });
                });
            }
        }

        this.buildingForm = false;
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

    onHumanTaskChanged(evt: any) {
        const value: string = evt.value;
        this.selectHumanTask(value);
    }

    private selectHumanTask(name: string) {
        const filteredHumanTaskDefs = this.humanTaskDefs.filter(function (ht: HumanTaskDef) {
            return ht.name === name;
        })
        if (filteredHumanTaskDefs.length !== 1) {
            this.inputParameters = [];
            return;
        }

        this.inputParameters = HumanTaskDef.getInputOperationParameters(filteredHumanTaskDefs[0]);
    }
}
