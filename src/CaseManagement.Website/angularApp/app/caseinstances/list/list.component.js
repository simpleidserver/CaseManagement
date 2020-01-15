var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { CaseInstancesService } from '../services/caseinstances.service';
import { ActionTypes } from './view-actions';
var CmmnViewer = require('cmmn-js/lib/NavigatedViewer');
var ViewCaseDefinitionComponent = (function () {
    function ViewCaseDefinitionComponent(caseDefinitionStore, caseInstancesStore, formInstancesStore, caseActivationsStore, route, caseInstancesService) {
        this.caseDefinitionStore = caseDefinitionStore;
        this.caseInstancesStore = caseInstancesStore;
        this.formInstancesStore = formInstancesStore;
        this.caseActivationsStore = caseActivationsStore;
        this.route = route;
        this.caseInstancesService = caseInstancesService;
        this.caseDefinition = {
            CaseFile: null,
            CreateDateTime: null,
            Description: null,
            Id: null,
            Name: null
        };
        this.displayedColumns = ['id', 'state', 'create_datetime', 'actions'];
        this.formInstanceDisplayedColumns = ['form_id', 'case_instance_id', 'performer', 'update_datetime', 'create_datetime'];
        this.caseActivationDisplayedColumns = ['case_instance_name', 'case_instance_id', 'performer', 'create_datetime'];
        this.caseDefinitionHistory = {
            Id: null,
            Elements: [],
            NbInstances: 0
        };
    }
    ViewCaseDefinitionComponent.prototype.ngOnInit = function () {
        var _this = this;
        var viewer = new CmmnViewer({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        var self = this;
        this.subscription = this.caseDefinitionStore.pipe(select('caseDefinition')).subscribe(function (st) {
            if (!st) {
                return;
            }
            if (_this.isLoading == true && !st.isLoading) {
                _this.isLoading = st.isLoading;
                _this.isErrorLoadOccured = st.isErrorLoadOccured;
                _this.caseDefinitionHistory = st.caseDefinitionHistory;
                if (st.caseDefinition) {
                    _this.caseDefinition = st.caseDefinition;
                    viewer.importXML(st.caseFile.Payload, function (err) {
                        if (err) {
                            return;
                        }
                        var canvas = viewer.get('canvas');
                        self.updateCanvas(viewer, st.caseDefinitionHistory);
                        canvas.zoom('fit-viewport');
                    });
                }
            }
            else if (_this.isLoading == false && st.caseDefinitionHistory) {
                self.caseDefinitionHistory = st.caseDefinitionHistory;
                self.updateCanvas(viewer, st.caseDefinitionHistory);
            }
        });
        this.subscriptionCaseInstances = this.caseInstancesStore.pipe(select('caseInstances')).subscribe(function (st) {
            if (!st) {
                return;
            }
            _this.isCaseInstancesErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                _this.caseInstances = st.content.Content;
                _this.caseInstancesLength = st.content.TotalLength;
            }
        });
        this.subscriptionFormInstances = this.formInstancesStore.pipe(select('formInstances')).subscribe(function (st) {
            if (!st) {
                return;
            }
            _this.isCaseFormInstancesErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                _this.caseFormInstances = st.content.Content;
                _this.formInstancesLength = st.content.TotalLength;
            }
        });
        this.subscriptionCaseActivations = this.caseActivationsStore.pipe(select('caseActivations')).subscribe(function (st) {
            if (!st) {
                return;
            }
            _this.isCaseActivationsErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                _this.caseActivations = st.content.Content;
                _this.caseActivationsLength = st.content.TotalLength;
            }
        });
        this.interval = setInterval(function () {
            _this.refresh();
        }, 5000);
        this.isLoading = true;
        this.isErrorLoadOccured = false;
        this.refresh();
    };
    ViewCaseDefinitionComponent.prototype.updateCanvas = function (viewer, caseDefinitionHistory) {
        if (caseDefinitionHistory.Elements.length > 0) {
            var overlays = viewer.get('overlays');
            caseDefinitionHistory.Elements.forEach(function (elt) {
                overlays.remove(elt.Element);
                overlays.add(elt.Element, "note", {
                    position: {
                        top: -5,
                        right: -5
                    },
                    html: '<div class="nb-instances">' + elt.NbInstances + '</div>'
                });
            });
        }
    };
    ViewCaseDefinitionComponent.prototype.createInstance = function () {
        var _this = this;
        this.caseInstancesService.create(this.route.snapshot.params['id']).subscribe(function () {
            _this.refresh();
        });
    };
    ViewCaseDefinitionComponent.prototype.launchCaseInstance = function (caseInstance) {
        var _this = this;
        this.caseInstancesService.launch(caseInstance.Id).subscribe(function () {
            _this.refresh();
        });
    };
    ViewCaseDefinitionComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.caseInstancesSort.sortChange, this.caseInstancesPaginator.page).subscribe(function () { return _this.refreshCaseInstances(); });
        merge(this.formInstancesSort.sortChange, this.formInstancesPaginator.page).subscribe(function () { return _this.refreshFormInstances(); });
        merge(this.caseActivationsSort.sortChange, this.caseActivationsPaginator.page).subscribe(function () { return _this.refreshCaseActivations(); });
    };
    ViewCaseDefinitionComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var loadCaseDefinition = {
            type: ActionTypes.CASEDEFINITIONLOAD,
            id: id
        };
        this.caseDefinitionStore.dispatch(loadCaseDefinition);
        this.refreshCaseInstances();
        this.refreshFormInstances();
        this.refreshCaseActivations();
    };
    ViewCaseDefinitionComponent.prototype.refreshCaseInstances = function () {
        var loadCaseInstances = {
            type: ActionTypes.CASEINSTANCESLOAD,
            id: this.route.snapshot.params['id'],
            order: this.caseInstancesSort.active,
            direction: this.caseInstancesSort.direction,
            count: this.caseInstancesPaginator.pageSize
        };
        if (this.caseInstancesPaginator.pageIndex && this.caseInstancesPaginator.pageSize) {
            loadCaseInstances['startIndex'] = this.caseInstancesPaginator.pageIndex * this.caseInstancesPaginator.pageSize;
        }
        else {
            loadCaseInstances['startIndex'] = 0;
        }
        if (this.caseInstancesPaginator.pageSize) {
            loadCaseInstances['count'] = this.caseInstancesPaginator.pageSize;
        }
        else {
            loadCaseInstances['count'] = 5;
        }
        this.isCaseInstancesErrorLoadOccured = false;
        this.caseInstancesStore.dispatch(loadCaseInstances);
    };
    ViewCaseDefinitionComponent.prototype.refreshFormInstances = function () {
        var loadFormInstances = {
            type: ActionTypes.CASEFORMINSTANCESLOAD,
            id: this.route.snapshot.params['id'],
            order: this.formInstancesSort.active,
            direction: this.formInstancesSort.direction,
            count: this.formInstancesPaginator.pageSize
        };
        if (this.formInstancesPaginator.pageIndex && this.formInstancesPaginator.pageSize) {
            loadFormInstances['startIndex'] = this.formInstancesPaginator.pageIndex * this.formInstancesPaginator.pageSize;
        }
        else {
            loadFormInstances['startIndex'] = 0;
        }
        if (this.formInstancesPaginator.pageSize) {
            loadFormInstances['count'] = this.formInstancesPaginator.pageSize;
        }
        else {
            loadFormInstances['count'] = 5;
        }
        this.isCaseFormInstancesErrorLoadOccured = false;
        this.formInstancesStore.dispatch(loadFormInstances);
    };
    ViewCaseDefinitionComponent.prototype.refreshCaseActivations = function () {
        var loadCaseActivations = {
            type: ActionTypes.CASEACTIVATIONSLOAD,
            id: this.route.snapshot.params['id'],
            order: this.caseActivationsSort.active,
            direction: this.caseActivationsSort.direction,
            count: this.caseActivationsPaginator.pageSize
        };
        if (this.caseActivationsPaginator.pageIndex && this.caseActivationsPaginator.pageSize) {
            loadCaseActivations['startIndex'] = this.caseActivationsPaginator.pageIndex * this.caseActivationsPaginator.pageSize;
        }
        else {
            loadCaseActivations['startIndex'] = 0;
        }
        if (this.caseActivationsPaginator.pageSize) {
            loadCaseActivations['count'] = this.caseActivationsPaginator.pageSize;
        }
        else {
            loadCaseActivations['count'] = 5;
        }
        this.isCaseActivationsErrorLoadOccured = false;
        this.caseInstancesStore.dispatch(loadCaseActivations);
    };
    ViewCaseDefinitionComponent.prototype.ngOnDestroy = function () {
        this.subscription.unsubscribe();
        this.subscriptionCaseInstances.unsubscribe();
        this.subscriptionFormInstances.unsubscribe();
        this.subscriptionCaseActivations.unsubscribe();
        clearInterval(this.interval);
    };
    __decorate([
        ViewChild('caseInstancesSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "caseInstancesSort", void 0);
    __decorate([
        ViewChild('formInstancesSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "formInstancesSort", void 0);
    __decorate([
        ViewChild('caseActivationsSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "caseActivationsSort", void 0);
    __decorate([
        ViewChild('caseInstancesPaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "caseInstancesPaginator", void 0);
    __decorate([
        ViewChild('formInstancesPaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "formInstancesPaginator", void 0);
    __decorate([
        ViewChild('caseActivationsPaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "caseActivationsPaginator", void 0);
    ViewCaseDefinitionComponent = __decorate([
        Component({
            selector: 'view-case-files',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store, Store, Store, Store, ActivatedRoute, CaseInstancesService])
    ], ViewCaseDefinitionComponent);
    return ViewCaseDefinitionComponent;
}());
export { ViewCaseDefinitionComponent };
//# sourceMappingURL=view.component.js.map