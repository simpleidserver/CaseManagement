import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatPaginator, MatSnackBar, MatSort, MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import { SearchBpmnFilesResult } from '@app/stores/bpmnfiles/models/search-bpmn-files-result.model';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { BpmnInstance } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { SearchBpmnInstancesResult } from '@app/stores/bpmninstances/models/search-bpmn-instances-result.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { ViewXmlDialog } from './view-xml-dialog';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { Parameter } from '@app/stores/common/parameter.model';
import { BpmnFileState } from '@app/stores/bpmnfiles/reducers/bpmnfile.reducers';
let BpmnViewer = require('bpmn-js/lib/Modeler'),
    propertiesPanelModule = require('bpmn-js-properties-panel'),
    propertiesProviderModule = require('bpmn-js-properties-panel/lib/provider/bpmn');

let caseMgtBpmnModdle = require('@app/moddlextensions/casemanagement-bpmn');

@Component({
    selector: 'view-bpmn-file',
    templateUrl: './viewfile.component.html',
    styleUrls: ['./viewfile.component.scss']
})
export class ViewBpmnFileComponent implements OnInit, OnDestroy {
    bpmnInstancesListener: any;
    bpmnFileListener: any;
    displayedColumns: string[] = ['status', 'create_datetime', 'update_datetime', 'nbExecutionPath', 'actions'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    paramsSub: any = null;
    viewer: any;
    length: number;
    buildingForm: boolean = true;
    isEltSelected: boolean = false;
    outgoingElts: string[] = [];
    selectedElt: any = null;
    parameters: { key: string, value: string }[] = [];
    bpmnFile: BpmnFile = new BpmnFile();
    bpmnFiles: BpmnFile[] = [];
    bpmnInstances$: BpmnInstance[] = [];
    inputParameters: Parameter[] = [];
    humanTaskDefs: HumanTaskDef[] = [];
    versionFormControl: FormControl = new FormControl('');
    saveForm: FormGroup = new FormGroup({
        name: new FormControl({ value: '' }),
        description: new FormControl({ value: '' })
    });
    updatePropertiesForm: FormGroup = new FormGroup({
        id: new FormControl(''),
        name: new FormControl(''),
        implementation: new FormControl(''),
        className: new FormControl(''),
        wsHumanTaskDefName: new FormControl(''),
        default: new FormControl(''),
        gatewayDirection: new FormControl(''),
        sequenceFlowCondition: new FormControl('')
    });
    addParameterForm: FormGroup = new FormGroup({
        key: new FormControl(''),
        value: new FormControl('')
    });

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private store: Store<fromAppState.AppState>,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private dialog: MatDialog) {
    }

    ngOnInit() {
        const self = this;
        this.viewer = new BpmnViewer.default({
            additionalModules: [
                propertiesPanelModule,
                propertiesProviderModule
            ],
            container: "#canvas",
            keyboard: {
                bindTo: window
            },
            moddleExtensions: {
                cmg: caseMgtBpmnModdle
            }
        });
        const evtBus = this.viewer.get('eventBus');
        evtBus.on('element.click', function (evt: any) {
            self.updateProperties(evt.element);
        });
        this.store.pipe(select(fromAppState.selectBpmnInstancesResult)).subscribe((searchBpmnInstancesResult: SearchBpmnInstancesResult) => {
            if (!searchBpmnInstancesResult) {
                return;
            }

            this.bpmnInstances$ = searchBpmnInstancesResult.content;
            this.length = searchBpmnInstancesResult.totalLength;
        });
        this.bpmnInstancesListener = this.store.pipe(select(fromAppState.selectBpmnFilesResult)).subscribe((res: SearchBpmnFilesResult) => {
            if (!res) {
                return;
            }

            this.bpmnFiles = res.content;
            this.versionFormControl.setValue(this.bpmnFile.version);
        });
        this.bpmnFileListener = this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe((bpmnFileState: BpmnFileState) => {
            if (!bpmnFileState || !bpmnFileState.content) {
                return;
            }

            this.bpmnFile = bpmnFileState.content;
            this.humanTaskDefs = bpmnFileState.humanTaskDefs;
            this.saveForm.controls['name'].setValue(bpmnFileState.content.name);
            this.saveForm.controls['description'].setValue(bpmnFileState.content.description);
            this.viewer.importXML(bpmnFileState.content.payload);
            const request = new fromBpmnFileActions.SearchBpmnFiles("create_datetime", "desc", 20000, 0, false, bpmnFileState.content.fileId);
            this.store.dispatch(request);
        });
        this.updatePropertiesForm.valueChanges.subscribe(() => {
            if (self.buildingForm) {
                return;
            }

            self.saveProperties(this.updatePropertiesForm.value);
        });
        this.updatePropertiesForm.get('default').valueChanges.subscribe(() => {
            const selectedPath = self.updatePropertiesForm.get('default').value;
            if (!selectedPath || !self.selectedElt) {
                return;
            }

            const elementRegistry = self.viewer.get('elementRegistry');
            const connection = elementRegistry.get(selectedPath);
            const modeling = self.viewer.get('modeling');
            if (self.selectedElt.outgoing) {
                modeling.setColor(self.selectedElt.outgoing, {
                    stroke: 'black'
                });
            }

            modeling.setColor([connection], {
                stroke: 'orange'
            });
        });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnInstanceActions.ActionTypes.COMPLETE_START_BPMNINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.INSTANCE_STARTED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnInstanceActions.ActionTypes.ERROR_START_BPMNINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_START_INSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnInstanceActions.ActionTypes.COMPLETE_CREATE_BPMN_INSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.INSTANCE_CREATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnInstanceActions.ActionTypes.ERROR_CREATE_BPMNINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_CREATE_INSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.FILE_SAVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_UPDATE_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_SAVE_FILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.COMPLETE_PUBLISH_BPMNFILE))
            .subscribe((act: any) => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.FILE_PUBLISHED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.router.navigate(['/bpmns/'+ act.id]);
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_PUBLISH_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_PUBLISH_FILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.paramsSub = this.route.params.subscribe(() => {
            this.refresh();
        });
    }

    ngOnDestroy(): void {
        this.paramsSub.unsubscribe();
        this.bpmnInstancesListener.unsubscribe();
        this.bpmnFileListener.unsubscribe();
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refreshBpmnInstances());
    }

    refresh() {
        let id = this.route.snapshot.params['id'];
        const request = new fromBpmnFileActions.GetBpmnFile(id);
        this.store.dispatch(request);
        this.refreshBpmnInstances();
    }

    start(evt: any, bpmn: BpmnInstance) {
        evt.preventDefault();
        const request = new fromBpmnInstanceActions.StartBpmnInstance(bpmn.id);
        this.store.dispatch(request);
    }

    refreshBpmnInstances() {
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

        const searchBpmn = new fromBpmnInstanceActions.SearchBpmnInstances(active, direction, count, startIndex, id);
        this.store.dispatch(searchBpmn);
    }

    onSave(form: any) {
        const self = this;
        const id = this.route.snapshot.params['id'];
        this.viewer.saveXML({}, function (e: any, x: any) {
            if (e) {
                return;
            }

            const act = new fromBpmnFileActions.UpdateBpmnFile(id, form.name, form.description, x);
            self.store.dispatch(act);
        });

    }

    onPublish(evt: any) {
        evt.preventDefault();
        const id = this.route.snapshot.params['id'];
        const act = new fromBpmnFileActions.PublishBpmnFile(id);
        this.store.dispatch(act);
    }

    removeParameter(elt: any) {
        if (!this.selectedElt) {
            return;
        }

        const bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:UserTask') {
            const index = this.parameters.indexOf(elt);
            this.parameters.splice(index, 1);
            this.saveProperties(this.updatePropertiesForm.value);
        }
    }

    addParameter(form: any) {
        if (!this.selectedElt) {
            return;
        }

        const bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:UserTask') {
            this.parameters.push(form);
            this.addParameterForm.reset();
            this.saveProperties(this.updatePropertiesForm.value);
        }
    }

    createInstance() {
        const id = this.route.snapshot.params['id'];
        const request = new fromBpmnInstanceActions.CreateBpmnInstance(id);
        this.store.dispatch(request);
    }

    updateFileVersion() {
        const self = this;
        const filtered = this.bpmnFiles.filter(function (b: BpmnFile) {
            return b.version === self.versionFormControl.value;
        })[0];
        this.router.navigate(['/bpmns/' + filtered.id]);
    }

    viewXML() {
        const self = this;
        const id = this.route.snapshot.params['id'];
        const xml = this.bpmnFile.payload;
        const d = this.dialog.open(ViewXmlDialog, {
            data: { xml: xml },
            width: '900px'
        });
        d.afterClosed().subscribe((r : any) => {
            if (!r) {
                return;
            }

            const act = new fromBpmnFileActions.UpdateBpmnFile(id, self.saveForm.value.name, self.saveForm.value.description, r.xml);
            self.store.dispatch(act);
        });
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

    private updateProperties(elt: any) {
        this.buildingForm = true;
        this.updatePropertiesForm.reset();
        this.selectedElt = elt;
        this.isEltSelected = true;
        const self = this;
        const bo = elt.businessObject;
        this.updatePropertiesForm.get('id').setValue(bo.id);
        this.updatePropertiesForm.get('name').setValue(bo.name);
        if (bo.$type === 'bpmn:ServiceTask') {
            this.updatePropertiesForm.get('implementation').setValue(bo.implementation);
            this.updatePropertiesForm.get('className').setValue(bo.get('cmg:className'));
        }

        if (bo.$type === 'bpmn:UserTask') {
            this.updatePropertiesForm.get('implementation').setValue(bo.implementation);
            this.updatePropertiesForm.get('wsHumanTaskDefName').setValue(bo.get('cmg:wsHumanTaskDefName'));
            this.selectHumanTask(bo.get('cmg:wsHumanTaskDefName'));
            const parameters = this.getExtension(bo, 'cmg:Parameters');
            self.parameters = [];
            if (parameters && parameters.parameter) {
                parameters.parameter.forEach(function (p: any) {
                    self.parameters.push({ key: p.key, value: p.value });
                });
            }
        }

        if (bo.$type === 'bpmn:ExclusiveGateway') {
            if (bo.default) {
                this.updatePropertiesForm.get('default').setValue(bo.default.id);
            }

            this.updatePropertiesForm.get('gatewayDirection').setValue(bo.gatewayDirection);
            if (bo.outgoing) {
                this.outgoingElts = bo.outgoing.map(function (o: any) {
                    return o.id;
                });
            }
        }

        if (bo.$type === 'bpmn:SequenceFlow') {
            if (bo.conditionExpression) {
                this.updatePropertiesForm.get('sequenceFlowCondition').setValue(bo.conditionExpression.body);
            }
        }

        this.buildingForm = false;
    }

    private saveProperties(form: any) {
        if (!this.selectedElt) {
            return;
        }

        const moddle = this.viewer.get('moddle');
        const modeling = this.viewer.get('modeling');
        const elementRegistry = this.viewer.get('elementRegistry');
        const obj: any = {
            id: form.id,
            name: form.name
        };

        const bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:ServiceTask') {
            obj['cmg:className'] = form.className;
            obj['implementation'] = form.implementation;
        }

        if (bo.$type === 'bpmn:UserTask') {
            obj['implementation'] = form.implementation;
            obj['cmg:wsHumanTaskDefName'] = form.wsHumanTaskDefName;
            let extensionElements = bo.extensionElements || moddle.create('bpmn:ExtensionElements');
            let parameters = this.getExtension(bo, 'cmg:Parameters');
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
        }

        if (bo.$type === 'bpmn:ExclusiveGateway') {
            if (form.default) {
                obj['default'] = elementRegistry.get(form.default).businessObject;
            }

            obj['gatewayDirection'] = form.gatewayDirection;
        }

        if (bo.$type === 'bpmn:SequenceFlow') {
            var newCondition = moddle.create('bpmn:FormalExpression', {
                body: form.sequenceFlowCondition
            });
            obj['conditionExpression'] = newCondition;
        }

        modeling.updateProperties(this.selectedElt, obj);
    }

    private getExtension(elt: any, type: string) {
        if (!elt.extensionElements) {
            return null;
        }

        return elt.extensionElements.values.filter(function (e: any) {
            return e.$type === type;
        })[0];
    }
}
