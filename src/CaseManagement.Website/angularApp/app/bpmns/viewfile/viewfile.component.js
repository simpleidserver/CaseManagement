var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatPaginator, MatSnackBar, MatSort, MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { ViewXmlDialog } from './view-xml-dialog';
var BpmnViewer = require('bpmn-js/lib/Modeler'), propertiesPanelModule = require('bpmn-js-properties-panel'), propertiesProviderModule = require('bpmn-js-properties-panel/lib/provider/bpmn');
var caseMgtBpmnModdle = require('@app/moddlextensions/casemanagement-bpmn');
var ViewBpmnFileComponent = (function () {
    function ViewBpmnFileComponent(route, router, store, actions$, snackBar, translateService, dialog) {
        this.route = route;
        this.router = router;
        this.store = store;
        this.actions$ = actions$;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.dialog = dialog;
        this.displayedColumns = ['status', 'create_datetime', 'update_datetime', 'nbExecutionPath', 'actions'];
        this.paramsSub = null;
        this.buildingForm = true;
        this.isEltSelected = false;
        this.outgoingElts = [];
        this.selectedElt = null;
        this.parameters = [];
        this.bpmnFile = new BpmnFile();
        this.bpmnFiles = [];
        this.bpmnInstances$ = [];
        this.versionFormControl = new FormControl('');
        this.saveForm = new FormGroup({
            name: new FormControl({ value: '' }),
            description: new FormControl({ value: '' })
        });
        this.updatePropertiesForm = new FormGroup({
            id: new FormControl(''),
            name: new FormControl(''),
            implementation: new FormControl(''),
            className: new FormControl(''),
            wsHumanTaskDefName: new FormControl(''),
            default: new FormControl(''),
            gatewayDirection: new FormControl(''),
            sequenceFlowCondition: new FormControl('')
        });
        this.addParameterForm = new FormGroup({
            key: new FormControl(''),
            value: new FormControl('')
        });
    }
    ViewBpmnFileComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
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
        var evtBus = this.viewer.get('eventBus');
        evtBus.on('element.click', function (evt) {
            self.updateProperties(evt.element);
        });
        this.store.pipe(select(fromAppState.selectBpmnInstancesResult)).subscribe(function (searchBpmnInstancesResult) {
            if (!searchBpmnInstancesResult) {
                return;
            }
            _this.bpmnInstances$ = searchBpmnInstancesResult.content;
            _this.length = searchBpmnInstancesResult.totalLength;
        });
        this.store.pipe(select(fromAppState.selectBpmnFilesResult)).subscribe(function (res) {
            if (!res) {
                return;
            }
            _this.bpmnFiles = res.content;
            _this.versionFormControl.setValue(_this.bpmnFile.version);
        });
        this.store.pipe(select(fromAppState.selectBpmnFileResult)).subscribe(function (bpmnFile) {
            if (!bpmnFile) {
                return;
            }
            _this.bpmnFile = bpmnFile;
            _this.saveForm.controls['name'].setValue(bpmnFile.name);
            _this.saveForm.controls['description'].setValue(bpmnFile.description);
            _this.viewer.importXML(bpmnFile.payload);
            var request = new fromBpmnFileActions.SearchBpmnFiles("create_datetime", "desc", 20000, 0, false, bpmnFile.fileId);
            _this.store.dispatch(request);
        });
        this.updatePropertiesForm.valueChanges.subscribe(function () {
            if (self.buildingForm) {
                return;
            }
            self.saveProperties(_this.updatePropertiesForm.value);
        });
        this.updatePropertiesForm.get('default').valueChanges.subscribe(function () {
            var selectedPath = self.updatePropertiesForm.get('default').value;
            if (!selectedPath || !self.selectedElt) {
                return;
            }
            var elementRegistry = self.viewer.get('elementRegistry');
            var connection = elementRegistry.get(selectedPath);
            var modeling = self.viewer.get('modeling');
            if (self.selectedElt.outgoing) {
                modeling.setColor(self.selectedElt.outgoing, {
                    stroke: 'black'
                });
            }
            modeling.setColor([connection], {
                stroke: 'orange'
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.COMPLETE_START_BPMNINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.INSTANCE_STARTED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.ERROR_START_BPMNINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_START_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.COMPLETE_CREATE_BPMN_INSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.INSTANCE_CREATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnInstanceActions.ActionTypes.ERROR_CREATE_BPMNINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_CREATE_INSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.FILE_SAVED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.ERROR_UPDATE_BPMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_SAVE_FILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.COMPLETE_PUBLISH_BPMNFILE; }))
            .subscribe(function (act) {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.FILE_PUBLISHED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.router.navigate(['/bpmns/' + act.id]);
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromBpmnFileActions.ActionTypes.ERROR_PUBLISH_BPMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('BPMN.MESSAGES.ERROR_PUBLISH_FILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.paramsSub = this.route.params.subscribe(function () {
            _this.refresh();
        });
    };
    ViewBpmnFileComponent.prototype.ngOnDestroy = function () {
        this.paramsSub.unsubscribe();
    };
    ViewBpmnFileComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refreshBpmnInstances(); });
    };
    ViewBpmnFileComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var request = new fromBpmnFileActions.GetBpmnFile(id);
        this.store.dispatch(request);
        this.refreshBpmnInstances();
    };
    ViewBpmnFileComponent.prototype.start = function (evt, bpmn) {
        evt.preventDefault();
        var request = new fromBpmnInstanceActions.StartBpmnInstance(bpmn.id);
        this.store.dispatch(request);
    };
    ViewBpmnFileComponent.prototype.refreshBpmnInstances = function () {
        var id = this.route.snapshot.params['id'];
        var startIndex = 0;
        var count = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }
        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }
        var active = "create_datetime";
        var direction = "desc";
        if (this.sort.active) {
            active = this.sort.active;
        }
        if (this.sort.direction) {
            direction = this.sort.direction;
        }
        var searchBpmn = new fromBpmnInstanceActions.SearchBpmnInstances(active, direction, count, startIndex, id);
        this.store.dispatch(searchBpmn);
    };
    ViewBpmnFileComponent.prototype.onSave = function (form) {
        var self = this;
        var id = this.route.snapshot.params['id'];
        this.viewer.saveXML({}, function (e, x) {
            if (e) {
                return;
            }
            var act = new fromBpmnFileActions.UpdateBpmnFile(id, form.name, form.description, x);
            self.store.dispatch(act);
        });
    };
    ViewBpmnFileComponent.prototype.onPublish = function (evt) {
        evt.preventDefault();
        var id = this.route.snapshot.params['id'];
        var act = new fromBpmnFileActions.PublishBpmnFile(id);
        this.store.dispatch(act);
    };
    ViewBpmnFileComponent.prototype.removeParameter = function (elt) {
        if (!this.selectedElt) {
            return;
        }
        var bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:UserTask') {
            var index = this.parameters.indexOf(elt);
            this.parameters.splice(index, 1);
        }
    };
    ViewBpmnFileComponent.prototype.addParameter = function (form) {
        if (!this.selectedElt) {
            return;
        }
        var bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:UserTask') {
            this.parameters.push(form);
            this.addParameterForm.reset();
        }
    };
    ViewBpmnFileComponent.prototype.createInstance = function () {
        var id = this.route.snapshot.params['id'];
        var request = new fromBpmnInstanceActions.CreateBpmnInstance(id);
        this.store.dispatch(request);
    };
    ViewBpmnFileComponent.prototype.updateFileVersion = function () {
        var self = this;
        var filtered = this.bpmnFiles.filter(function (b) {
            return b.version === self.versionFormControl.value;
        })[0];
        this.router.navigate(['/bpmns/' + filtered.id]);
    };
    ViewBpmnFileComponent.prototype.viewXML = function () {
        var self = this;
        var id = this.route.snapshot.params['id'];
        var xml = this.bpmnFile.payload;
        var d = this.dialog.open(ViewXmlDialog, {
            data: { xml: xml },
            width: '900px'
        });
        d.afterClosed().subscribe(function (r) {
            if (!r) {
                return;
            }
            var act = new fromBpmnFileActions.UpdateBpmnFile(id, self.saveForm.value.name, self.saveForm.value.description, r.xml);
            self.store.dispatch(act);
        });
    };
    ViewBpmnFileComponent.prototype.updateProperties = function (elt) {
        this.buildingForm = true;
        this.updatePropertiesForm.reset();
        this.selectedElt = elt;
        this.isEltSelected = true;
        var self = this;
        var bo = elt.businessObject;
        this.updatePropertiesForm.get('id').setValue(bo.id);
        this.updatePropertiesForm.get('name').setValue(bo.name);
        if (bo.$type === 'bpmn:ServiceTask') {
            this.updatePropertiesForm.get('implementation').setValue(bo.implementation);
            this.updatePropertiesForm.get('className').setValue(bo.get('cmg:className'));
        }
        if (bo.$type === 'bpmn:UserTask') {
            this.updatePropertiesForm.get('implementation').setValue(bo.implementation);
            this.updatePropertiesForm.get('wsHumanTaskDefName').setValue(bo.get('cmg:wsHumanTaskDefName'));
            var parameters = this.getExtension(bo, 'cmg:Parameters');
            self.parameters = [];
            if (parameters && parameters.parameter) {
                parameters.parameter.forEach(function (p) {
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
                this.outgoingElts = bo.outgoing.map(function (o) {
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
    };
    ViewBpmnFileComponent.prototype.saveProperties = function (form) {
        if (!this.selectedElt) {
            return;
        }
        var moddle = this.viewer.get('moddle');
        var modeling = this.viewer.get('modeling');
        var elementRegistry = this.viewer.get('elementRegistry');
        var obj = {
            id: form.id,
            name: form.name
        };
        var bo = this.selectedElt.businessObject;
        if (bo.$type === 'bpmn:ServiceTask') {
            obj['cmg:className'] = form.className;
            obj['implementation'] = form.implementation;
        }
        if (bo.$type === 'bpmn:UserTask') {
            obj['implementation'] = form.implementation;
            obj['cmg:wsHumanTaskDefName'] = form.wsHumanTaskDefName;
            var extensionElements = bo.extensionElements || moddle.create('bpmn:ExtensionElements');
            var parameters_1 = this.getExtension(bo, 'cmg:Parameters');
            if (!parameters_1) {
                parameters_1 = moddle.create('cmg:Parameters');
                extensionElements.get('values').push(parameters_1);
            }
            parameters_1.parameter = [];
            this.parameters.forEach(function (p) {
                var parameter = moddle.create('cmg:Parameter');
                parameter.key = p.key;
                parameter.value = p.value;
                parameters_1.parameter.push(parameter);
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
    };
    ViewBpmnFileComponent.prototype.getExtension = function (elt, type) {
        if (!elt.extensionElements) {
            return null;
        }
        return elt.extensionElements.values.filter(function (e) {
            return e.$type === type;
        })[0];
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ViewBpmnFileComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ViewBpmnFileComponent.prototype, "sort", void 0);
    ViewBpmnFileComponent = __decorate([
        Component({
            selector: 'view-bpmn-file',
            templateUrl: './viewfile.component.html',
            styleUrls: ['./viewfile.component.scss']
        }),
        __metadata("design:paramtypes", [ActivatedRoute,
            Router,
            Store,
            ScannedActionsSubject,
            MatSnackBar,
            TranslateService,
            MatDialog])
    ], ViewBpmnFileComponent);
    return ViewBpmnFileComponent;
}());
export { ViewBpmnFileComponent };
//# sourceMappingURL=viewfile.component.js.map