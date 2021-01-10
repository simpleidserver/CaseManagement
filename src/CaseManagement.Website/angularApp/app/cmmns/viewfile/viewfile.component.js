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
import { FormControl, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import * as fromCmmnPlanActions from '@app/stores/cmmnplans/actions/cmmn-plans.actions';
import * as fromCmmnPlanInstanceActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { MatSnackBar, MatDialog, MatSort, MatPaginator } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { ViewXmlDialog } from './view-xml-dialog';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
var CmmnViewer = require('cmmn-js/lib/Modeler'), propertiesPanelModule = require('cmmn-js-properties-panel'), propertiesProviderModule = require('cmmn-js-properties-panel/lib/provider/cmmn');
var caseMgtCmmnModdle = require('@app/moddlextensions/casemanagement-cmmn');
var ViewCmmnFileComponent = (function () {
    function ViewCmmnFileComponent(store, route, formBuilder, snackBar, actions$, translateService, router, dialog) {
        this.store = store;
        this.route = route;
        this.formBuilder = formBuilder;
        this.snackBar = snackBar;
        this.actions$ = actions$;
        this.translateService = translateService;
        this.router = router;
        this.dialog = dialog;
        this.mappingTypesToEvents = [
            { types: ['cmmn:HumanTask'], evts: ['create', 'start', 'enable', 'reenable', 'disable', 'manualStart', 'reactivate', 'fault', 'suspend', 'parentSuspend', 'parentResume', 'resume', 'complete', 'terminate', 'exit'] },
            { types: ['cmmn:UserEventListener'], evts: ['create', 'resume', 'suspend', 'terminate', 'occur', 'parentTerminate'] }
        ];
        this.standardEvts = [];
        this.displayedColumns = ['status', 'name', 'create_datetime', 'update_datetime'];
        this.buildingForm = true;
        this.selectedElt = null;
        this.isEltSelected = false;
        this.inputParameters = [];
        this.humanTaskDefs = [];
        this.parameters = [];
        this.cmmnFiles$ = [];
        this.cmmnPlans$ = [];
        this.cmmnPlanInstances$ = [];
        this.cmmnFile = new CmmnFile();
        this.updatePropertiesForm = new FormGroup({
            id: new FormControl(''),
            name: new FormControl(''),
            implementation: new FormControl(''),
            formId: new FormControl(''),
            standardEvent: new FormControl(''),
            repetitionRuleCondition: new FormControl(''),
            manualActivationRuleCondition: new FormControl('')
        });
        this.addParameterForm = new FormGroup({
            key: new FormControl(''),
            value: new FormControl('')
        });
        this.versionFormControl = new FormControl('');
        this.saveForm = this.formBuilder.group({
            name: new FormControl({ value: '' }),
            description: new FormControl({ value: '' })
        });
    }
    ViewCmmnFileComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
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
        var evtBus = this.viewer.get('eventBus');
        evtBus.on('element.click', function (evt) {
            self.updateProperties(evt.element);
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnPlanInstanceActions.ActionTypes.COMPLETE_LAUNCH_CMMN_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.CMMNPLANINSTANCE_LAUNCHED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnPlanInstanceActions.ActionTypes.ERROR_LAUNCH_CMMN_PLANINSTANCE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_LAUNCH_CMMNPLANINSTANCE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.CMMNFILE_UPDATED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.ERROR_UPDATE_CMMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_UPDATE_CMMNFILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.COMPLETE_PUBLISH_CMMNFILE; }))
            .subscribe(function (act) {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.CMMNFILE_PUBLISHED'), _this.translateService.instant('undo'), {
                duration: 2000
            });
            _this.router.navigate(['/cmmns/' + act.id]);
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromCmmnFileActions.ActionTypes.ERROR_PUBLISH_CMMNFILE; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('CMMN.MESSAGES.ERROR_PUBLISH_CMMNFILE'), _this.translateService.instant('undo'), {
                duration: 2000
            });
        });
        this.updatePropertiesForm.valueChanges.subscribe(function () {
            if (self.buildingForm) {
                return;
            }
            self.saveProperties(_this.updatePropertiesForm.value);
        });
        this.cmmnPlanInstanceLstListener = this.store.pipe(select(fromAppState.selectCmmnPlanInstanceLstResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.cmmnPlanInstances$ = e.content;
        });
        this.cmmnFileLstListener = this.store.pipe(select(fromAppState.selectCmmnFileLstResult)).subscribe(function (e) {
            if (!e) {
                return;
            }
            _this.displayCanvas();
            _this.cmmnFiles$ = e.content;
        });
        this.cmmnFileListener = this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe(function (cmmnFile) {
            if (!cmmnFile || !cmmnFile.content) {
                return;
            }
            _this.cmmnFile = cmmnFile.content;
            _this.humanTaskDefs = cmmnFile.humanTaskDefs;
            _this.displayCanvas();
            _this.saveForm.controls['name'].setValue(cmmnFile.content.name);
            _this.saveForm.controls['description'].setValue(cmmnFile.content.description);
            var request = new fromCmmnFileActions.SearchCmmnFiles("create_datetime", "desc", 10000, 0, null, cmmnFile.content.fileId, false);
            _this.store.dispatch(request);
        });
        this.store.pipe(select(fromAppState.selectCmmnPlanLstResult)).subscribe(function (searchResult) {
            if (!searchResult) {
                return;
            }
            _this.cmmnPlans$ = searchResult.content;
        });
        this.paramsSub = this.route.params.subscribe(function () {
            var id = _this.route.snapshot.params['id'];
            _this.versionFormControl.setValue(id);
            _this.refresh();
        });
    };
    ViewCmmnFileComponent.prototype.ngOnDestroy = function () {
        this.paramsSub.unsubscribe();
        this.cmmnPlanInstanceLstListener.unsubscribe();
        this.cmmnFileLstListener.unsubscribe();
        this.cmmnFileListener.unsubscribe();
    };
    ViewCmmnFileComponent.prototype.displayCanvas = function () {
        if (!this.cmmnFile || !this.cmmnPlans$) {
            return;
        }
        var self = this;
        this.viewer.importXML(this.cmmnFile.payload, function () {
            var elementRegistry = self.viewer.get('elementRegistry');
            var overlays = self.viewer.get('overlays');
            var elts = elementRegistry.getAll();
            var stageElts = elts.filter(function (e) {
                return e.type === "cmmn:Stage";
            });
            var launchTemplate = "<div class='launch-stage' data-id='{id}'><svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='black' width='24px' height='24px'><path d='M0 0h24v24H0z' fill='none'/><path d='M19 19H5V5h7V3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.1 0 2-.9 2-2v-7h-2v7zM14 3v2h3.59l-9.83 9.83 1.41 1.41L19 6.41V10h2V3h-7z'/></svg></div>";
            stageElts.forEach(function (se) {
                var filtered = self.cmmnPlans$.filter(function (cmmnPlan) {
                    return cmmnPlan.casePlanId === se.id;
                });
                if (filtered.length !== 1) {
                    return;
                }
                var launch = launchTemplate.replace('{id}', filtered[0].id);
                launch = $(launch);
                overlays.add(se.id, {
                    position: {
                        top: 0,
                        right: 24
                    },
                    html: launch
                });
                launch.click(function () {
                    var id = $(this).data('id');
                    var request = new fromCmmnPlanInstanceActions.LaunchCmmnPlanInstance(id);
                    self.store.dispatch(request);
                });
            });
        });
    };
    ViewCmmnFileComponent.prototype.updateFileVersion = function () {
        var self = this;
        var filtered = this.cmmnFiles$.filter(function (b) {
            return b.id === self.versionFormControl.value;
        })[0];
        this.router.navigate(['/cmmns/' + filtered.id]);
    };
    ViewCmmnFileComponent.prototype.onSave = function (form) {
        var self = this;
        var id = this.route.snapshot.params['id'];
        this.viewer.saveXML({}, function (e, x) {
            if (e) {
                return;
            }
            var act = new fromCmmnFileActions.UpdateCmmnFile(id, form.name, form.description, x);
            self.store.dispatch(act);
        });
    };
    ViewCmmnFileComponent.prototype.onPublish = function (evt) {
        evt.preventDefault();
        var id = this.route.snapshot.params['id'];
        var act = new fromCmmnFileActions.PublishCmmnFile(id);
        this.store.dispatch(act);
    };
    ViewCmmnFileComponent.prototype.viewXML = function () {
        var self = this;
        var id = this.route.snapshot.params['id'];
        var xml = this.cmmnFile.payload;
        var d = this.dialog.open(ViewXmlDialog, {
            data: { xml: xml },
            width: '900px'
        });
        d.afterClosed().subscribe(function (r) {
            if (!r) {
                return;
            }
            var act = new fromCmmnFileActions.UpdateCmmnFile(id, self.saveForm.value.name, self.saveForm.value.description, r.xml);
            self.store.dispatch(act);
        });
    };
    ViewCmmnFileComponent.prototype.refresh = function () {
        this.id = this.route.snapshot.params['id'];
        var request = new fromCmmnFileActions.GetCmmnFile(this.id);
        this.store.dispatch(request);
        var searchPlans = new fromCmmnPlanActions.SearchCmmnPlans("create_datetime", "desc", 2000, 0, this.id, false);
        this.store.dispatch(searchPlans);
        this.refreshCmmnInstances();
    };
    ViewCmmnFileComponent.prototype.refreshCmmnInstances = function () {
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
        var searchBpmn = new fromCmmnPlanInstanceActions.SearchCmmnPlanInstance(active, direction, count, startIndex, null, id);
        this.store.dispatch(searchBpmn);
    };
    ViewCmmnFileComponent.prototype.saveProperties = function (form) {
        if (!this.selectedElt || !this.selectedElt.businessObject) {
            return;
        }
        var moddle = this.viewer.get('moddle');
        var modeling = this.viewer.get('modeling');
        var obj = {
            id: form.id
        };
        if (this.selectedElt.businessObject && this.selectedElt.businessObject.$type === 'cmmn:PlanItem') {
            var manualActivationRuleCondition = this.updatePropertiesForm.get('manualActivationRuleCondition').value;
            var repetitionRuleCondition = this.updatePropertiesForm.get('repetitionRuleCondition').value;
            var itemControl = this.selectedElt.businessObject.itemControl;
            var boObj = {};
            if (itemControl.manualActivationRule) {
                if (manualActivationRuleCondition) {
                    var expression = itemControl.manualActivationRule.condition || moddle.create('cmmn:Expression');
                    expression.body = manualActivationRuleCondition;
                    itemControl.manualActivationRule.condition = expression;
                }
                else {
                    itemControl.manualActivationRule.condition = null;
                }
            }
            if (itemControl.repetitionRule) {
                if (repetitionRuleCondition) {
                    var expression = itemControl.repetitionRule.condition || moddle.create('cmmn:Expression');
                    expression.body = repetitionRuleCondition;
                    itemControl.repetitionRule.condition = expression;
                }
                else {
                    itemControl.repetitionRule.condition = null;
                }
            }
            boObj['itemControl'] = itemControl;
            modeling.updateProperties(this.selectedElt.businessObject, boObj);
        }
        if (this.selectedElt.businessObject.definitionRef) {
            var defRef = this.selectedElt.businessObject.definitionRef;
            if (defRef.$type === 'cmmn:HumanTask') {
                obj['cmg:implementation'] = form.implementation;
                obj['cmg:formId'] = form.formId;
                var extensionElements = defRef.extensionElements || moddle.create('cmmn:ExtensionElements');
                var parameters_1 = this.getExtension(defRef, 'cmg:Parameters');
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
                obj['cmmn:extensionElements'] = extensionElements;
            }
            modeling.updateProperties(defRef, obj);
        }
        if (this.selectedElt.type === 'cmmndi:CMMNEdge' && this.selectedElt.businessObject.cmmnElementRef) {
            var cmmnEltRef = this.selectedElt.businessObject.cmmnElementRef;
            var standardEvt = this.updatePropertiesForm.get('standardEvent').value;
            obj['standardEvent'] = standardEvt;
            modeling.updateProperties(cmmnEltRef, obj);
        }
        modeling.updateProperties(this.selectedElt, {
            name: form.name
        });
    };
    ViewCmmnFileComponent.prototype.updateProperties = function (elt) {
        this.buildingForm = true;
        this.updatePropertiesForm.reset();
        this.selectedElt = elt;
        this.isEltSelected = true;
        if (!elt || !elt.businessObject) {
            return;
        }
        var self = this;
        if (elt.businessObject && elt.businessObject.$type === 'cmmn:PlanItem') {
            if (elt.businessObject.itemControl) {
                if (elt.businessObject.itemControl.manualActivationRule && elt.businessObject.itemControl.manualActivationRule.condition) {
                    this.updatePropertiesForm.get('manualActivationRuleCondition').setValue(elt.businessObject.itemControl.manualActivationRule.condition.body);
                }
                if (elt.businessObject.itemControl.repetitionRule && elt.businessObject.itemControl.repetitionRule.condition) {
                    this.updatePropertiesForm.get('repetitionRuleCondition').setValue(elt.businessObject.itemControl.repetitionRule.condition.body);
                }
            }
        }
        if (elt.businessObject.definitionRef) {
            var defRef = elt.businessObject.definitionRef;
            this.updatePropertiesForm.get('id').setValue(defRef.id);
            this.updatePropertiesForm.get('name').setValue(elt.businessObject.name);
            if (defRef.$type === 'cmmn:HumanTask') {
                var parameters = this.getExtension(defRef, 'cmg:Parameters');
                this.updatePropertiesForm.get('implementation').setValue(defRef.get('cmg:implementation'));
                this.updatePropertiesForm.get('formId').setValue(defRef.get('cmg:formId'));
                this.selectHumanTask(defRef.get('cmg:formId'));
                self.parameters = [];
                if (parameters && parameters.parameter) {
                    parameters.parameter.forEach(function (p) {
                        self.parameters.push({ key: p.key, value: p.value });
                    });
                }
            }
        }
        if (elt.type === 'cmmndi:CMMNEdge' && elt.businessObject.cmmnElementRef) {
            var cmmnEltRef_1 = elt.businessObject.cmmnElementRef;
            this.updatePropertiesForm.get('id').setValue(cmmnEltRef_1.id);
            this.updatePropertiesForm.get('name').setValue(cmmnEltRef_1.name);
            this.updatePropertiesForm.get('standardEvent').setValue(cmmnEltRef_1.standardEvent);
            var filtered = this.mappingTypesToEvents.filter(function (v) {
                return v.types.includes(cmmnEltRef_1.sourceRef.definitionRef.$type);
            });
            if (filtered.length === 1) {
                this.standardEvts = filtered[0].evts;
            }
        }
        this.buildingForm = false;
    };
    ViewCmmnFileComponent.prototype.getExtension = function (elt, type) {
        if (!elt.extensionElements) {
            return null;
        }
        return elt.extensionElements.values.filter(function (e) {
            return e.$type === type;
        })[0];
    };
    ViewCmmnFileComponent.prototype.removeParameter = function (elt) {
        if (!this.selectedElt) {
            return;
        }
        var defRef = this.selectedElt.businessObject.definitionRef;
        if (defRef.$type === 'cmmn:HumanTask') {
            var index = this.parameters.indexOf(elt);
            this.parameters.splice(index, 1);
            this.saveProperties(this.updatePropertiesForm.value);
        }
    };
    ViewCmmnFileComponent.prototype.addParameter = function (form) {
        if (!this.selectedElt) {
            return;
        }
        var defRef = this.selectedElt.businessObject.definitionRef;
        if (defRef.$type === 'cmmn:HumanTask') {
            this.parameters.push(form);
            this.addParameterForm.reset();
            this.saveProperties(this.updatePropertiesForm.value);
        }
    };
    ViewCmmnFileComponent.prototype.onHumanTaskChanged = function (evt) {
        var value = evt.value;
        this.selectHumanTask(value);
    };
    ViewCmmnFileComponent.prototype.selectHumanTask = function (name) {
        var filteredHumanTaskDefs = this.humanTaskDefs.filter(function (ht) {
            return ht.name === name;
        });
        if (filteredHumanTaskDefs.length !== 1) {
            this.inputParameters = [];
            return;
        }
        this.inputParameters = HumanTaskDef.getInputOperationParameters(filteredHumanTaskDefs[0]);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ViewCmmnFileComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ViewCmmnFileComponent.prototype, "sort", void 0);
    ViewCmmnFileComponent = __decorate([
        Component({
            selector: 'view-cmmnfile',
            templateUrl: './viewfile.component.html',
            styleUrls: ['./viewfile.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            FormBuilder,
            MatSnackBar,
            ScannedActionsSubject,
            TranslateService,
            Router,
            MatDialog])
    ], ViewCmmnFileComponent);
    return ViewCmmnFileComponent;
}());
export { ViewCmmnFileComponent };
//# sourceMappingURL=viewfile.component.js.map