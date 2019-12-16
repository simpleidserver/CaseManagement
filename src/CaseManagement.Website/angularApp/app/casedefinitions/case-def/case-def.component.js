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
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { CaseDefinition } from '../models/case-def.model';
import { ActionTypes } from './case-def-actions';
import { CaseInstancesService } from '../services/caseinstances.service';
var CmmnViewer = require('cmmn-js');
var CaseDefComponent = (function () {
    function CaseDefComponent(store, route, caseInstanceService) {
        this.store = store;
        this.route = route;
        this.caseInstanceService = caseInstanceService;
        this.caseDefinition = new CaseDefinition();
        this.caseInstances = [];
        this.displayedColumns = ['name', 'status', 'create_datetime', 'Actions'];
    }
    CaseDefComponent.prototype.ngOnInit = function () {
        var self = this;
        self.isCaseDefinitionLoading = true;
        self.isCaseDefinitionErrorLoadOccured = false;
        self.viewer = new CmmnViewer({
            container: '#canvas'
        });
        self.store.pipe();
        self.subscription = self.store.pipe(select('caseDef')).subscribe(function (st) {
            if (!st) {
                return;
            }
            self.isCaseDefinitionLoading = st.isCaseDefinitionLoading;
            self.isCaseDefinitionErrorLoadOccured = st.isCaseDefinitionErrorLoadOccured;
            self.isCaseInstancesLoading = st.isCaseInstancesLoading;
            self.isCaseInstancesErrorLoadOccured = st.isCaseInstancesErrorLoadOccured;
            if (st.caseDefinitionContent) {
                self.caseDefinition = st.caseDefinitionContent;
                if (self.caseDefinition.CasePlanModels) {
                    self.selectedCasePlanModel = self.caseDefinition.CasePlanModels[0].Id;
                }
                self.viewer.importXML(st.caseDefinitionContent.Xml, function (err) {
                    if (!err) {
                        self.viewer.get('canvas').zoom('fit-viewport');
                    }
                });
            }
            if (st.caseInstancesContent) {
                self.length = st.caseInstancesContent.TotalLength;
                self.caseInstances = st.caseInstancesContent.Content;
            }
        });
        self.refresh();
        self.interval = setInterval(function () {
            self.refresh();
        }, 1000);
    };
    CaseDefComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    CaseDefComponent.prototype.createCaseInstance = function (e) {
        e.preventDefault();
        var self = this;
        self.caseInstanceService.create(this.caseDefinition.Id, this.selectedCasePlanModel).subscribe(function () {
            self.refresh();
        });
    };
    CaseDefComponent.prototype.launchInstance = function (caseInstance) {
        var self = this;
        self.caseInstanceService.launch(caseInstance.Id).subscribe(function () {
            self.refresh();
        });
    };
    CaseDefComponent.prototype.stopInstance = function (caseInstance) {
        var self = this;
        self.caseInstanceService.stop(caseInstance.Id).subscribe(function () {
            self.refresh();
        });
    };
    CaseDefComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var loadDefRequest = {
            type: ActionTypes.CASEDEFLOAD,
            id: id
        };
        var loadInstancesRequest = {
            type: ActionTypes.CASEINSTANCESLOAD,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize,
            id: id
        };
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            loadInstancesRequest['startIndex'] = this.paginator.pageIndex * this.paginator.pageSize;
        }
        else {
            loadInstancesRequest['startIndex'] = 0;
        }
        if (this.paginator.pageSize) {
            loadInstancesRequest['count'] = this.paginator.pageSize;
        }
        else {
            loadInstancesRequest['count'] = 5;
        }
        this.store.dispatch(loadDefRequest);
        this.store.dispatch(loadInstancesRequest);
    };
    CaseDefComponent.prototype.ngOnDestroy = function () {
        this.subscription.unsubscribe();
        clearInterval(this.interval);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], CaseDefComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], CaseDefComponent.prototype, "sort", void 0);
    CaseDefComponent = __decorate([
        Component({
            selector: 'case-def',
            templateUrl: './case-def.component.html',
            styleUrls: ['./case-def.component.scss']
        }),
        __metadata("design:paramtypes", [Store, ActivatedRoute, CaseInstancesService])
    ], CaseDefComponent);
    return CaseDefComponent;
}());
export { CaseDefComponent };
//# sourceMappingURL=case-def.component.js.map