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
import { CasePlan } from '../models/caseplan.model';
import * as fromCasePlanDefinitions from '../reducers';
import { CasePlanService } from '../services/caseplan.service';
var ViewCaseDefinitionComponent = (function () {
    function ViewCaseDefinitionComponent(casePlanStore, route, casePlanService) {
        this.casePlanStore = casePlanStore;
        this.route = route;
        this.casePlanService = casePlanService;
        this.selectedTimer = "4000";
        this.casePlan$ = new CasePlan();
        this.casePlanInstances$ = new Array();
        this.formInstances$ = new Array();
        this.workerTasks$ = new Array();
        this.displayedColumns = ['id', 'state', 'create_datetime', 'actions'];
        this.formInstanceDisplayedColumns = ['form_id', 'case_instance_id', 'performer', 'update_datetime', 'create_datetime'];
        this.caseActivationDisplayedColumns = ['case_instance_name', 'case_instance_id', 'performer', 'create_datetime'];
    }
    ViewCaseDefinitionComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectGetResult)).subscribe(function (casePlan) {
            if (!casePlan) {
                return;
            }
            _this.casePlan$ = casePlan;
        });
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectSearchInstanceResult)).subscribe(function (searchCasePlanInstanceResult) {
            if (!searchCasePlanInstanceResult) {
                return;
            }
            _this.casePlanInstances$ = searchCasePlanInstanceResult.Content;
            _this.casePlanInstancesLength = searchCasePlanInstanceResult.TotalLength;
        });
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectSearchFormInstancesResult)).subscribe(function (searchFormInstanceResult) {
            if (!searchFormInstanceResult) {
                return;
            }
            _this.formInstances$ = searchFormInstanceResult.Content;
            _this.formInstancesLength = searchFormInstanceResult.TotalLength;
        });
        this.casePlanStore.pipe(select(fromCasePlanDefinitions.selectSearchCaseActivationsResult)).subscribe(function (searchWorkerTaskResult) {
            if (!searchWorkerTaskResult) {
                return;
            }
            _this.caseActivations$ = searchCaseActivationsResult.Content;
            _this.caseActivationsLength = searchCaseActivationsResult.TotalLength;
        });
        this.interval = setInterval(function () {
            _this.refresh();
        }, 4000);
        this.refresh();
    };
    ViewCaseDefinitionComponent.prototype.selectTimer = function (evt) {
        var _this = this;
        clearInterval(this.interval);
        this.interval = setInterval(function () {
            _this.refresh();
        }, evt.value);
    };
    ViewCaseDefinitionComponent.prototype.launchCaseInstance = function () {
        var _this = this;
        this.caseInstancesService.create(this.route.snapshot.params['id']).subscribe(function (caseInstance) {
            _this.caseInstancesService.launch(caseInstance.Id).subscribe(function () {
                _this.refresh();
            });
        });
    };
    ViewCaseDefinitionComponent.prototype.reactivateCaseInstance = function (caseInstance) {
        var _this = this;
        this.caseInstancesService.reactivateCaseInstance(caseInstance.Id).subscribe(function () {
            _this.refresh();
        });
    };
    ViewCaseDefinitionComponent.prototype.suspendCaseInstance = function (caseInstance) {
        var _this = this;
        this.caseInstancesService.suspendCaseInstance(caseInstance.Id).subscribe(function () {
            _this.refresh();
        });
    };
    ViewCaseDefinitionComponent.prototype.resumeCaseInstance = function (caseInstance) {
        var _this = this;
        this.caseInstancesService.resumeCaseInstance(caseInstance.Id).subscribe(function () {
            _this.refresh();
        });
    };
    ViewCaseDefinitionComponent.prototype.closeCaseInstance = function (caseInstance) {
        var _this = this;
        this.caseInstancesService.closeCaseInstance(caseInstance.Id).subscribe(function () {
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
        this.refreshCaseDefinition();
        this.refreshCaseInstances();
        this.refreshFormInstances();
        this.refreshCaseActivations();
    };
    ViewCaseDefinitionComponent.prototype.refreshCaseDefinition = function () {
        var id = this.route.snapshot.params['id'];
        var loadCaseDefinition = new caseDefinitionsActions.StartGet(id);
        this.caseDefinitionStore.dispatch(loadCaseDefinition);
    };
    ViewCaseDefinitionComponent.prototype.refreshCaseInstances = function () {
        var startIndex = 0;
        var count = 5;
        if (this.caseInstancesPaginator.pageIndex && this.caseInstancesPaginator.pageSize) {
            startIndex = this.caseInstancesPaginator.pageIndex * this.caseInstancesPaginator.pageSize;
        }
        if (this.caseInstancesPaginator.pageSize) {
            count = this.caseInstancesPaginator.pageSize;
        }
        var loadCaseInstances = new caseInstancesActions.StartFetch(this.route.snapshot.params['id'], startIndex, count, this.caseInstancesSort.active, this.caseInstancesSort.direction);
        this.caseDefinitionStore.dispatch(loadCaseInstances);
    };
    ViewCaseDefinitionComponent.prototype.refreshFormInstances = function () {
        var startIndex = 0;
        var count = 5;
        if (this.formInstancesPaginator.pageSize) {
            count = this.formInstancesPaginator.pageSize;
        }
        if (this.formInstancesPaginator.pageIndex && this.formInstancesPaginator.pageSize) {
            startIndex = this.formInstancesPaginator.pageIndex * this.formInstancesPaginator.pageSize;
        }
        var loadFormInstances = new caseFormInstancesActions.StartFetch(this.route.snapshot.params['id'], this.formInstancesSort.active, this.formInstancesSort.direction, count, startIndex);
        this.caseDefinitionStore.dispatch(loadFormInstances);
    };
    ViewCaseDefinitionComponent.prototype.refreshCaseActivations = function () {
        var count = 5;
        var startIndex = 0;
        if (this.caseActivationsPaginator.pageSize) {
            count = this.caseActivationsPaginator.pageSize;
        }
        if (this.caseActivationsPaginator.pageIndex && this.caseActivationsPaginator.pageSize) {
            startIndex = this.caseActivationsPaginator.pageIndex * this.caseActivationsPaginator.pageSize;
        }
        var loadCaseActivations = new caseActivationsActions.StartFetch(this.route.snapshot.params['id'], this.caseActivationsSort.active, this.caseActivationsSort.direction, count, startIndex);
        this.caseDefinitionStore.dispatch(loadCaseActivations);
    };
    ViewCaseDefinitionComponent.prototype.ngOnDestroy = function () {
        clearInterval(this.interval);
    };
    __decorate([
        ViewChild('casePlanInstanceSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "caseInstancesSort", void 0);
    __decorate([
        ViewChild('formInstanceSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "formInstancesSort", void 0);
    __decorate([
        ViewChild('workerTaskSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseDefinitionComponent.prototype, "caseActivationsSort", void 0);
    __decorate([
        ViewChild('casePlanInstancePaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "caseInstancesPaginator", void 0);
    __decorate([
        ViewChild('formInstancePaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "formInstancesPaginator", void 0);
    __decorate([
        ViewChild('caseWorkerPaginator'),
        __metadata("design:type", MatPaginator)
    ], ViewCaseDefinitionComponent.prototype, "caseActivationsPaginator", void 0);
    ViewCaseDefinitionComponent = __decorate([
        Component({
            selector: 'view-case-files',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store, ActivatedRoute, CasePlanService])
    ], ViewCaseDefinitionComponent);
    return ViewCaseDefinitionComponent;
}());
export { ViewCaseDefinitionComponent };
//# sourceMappingURL=view.component.js.map