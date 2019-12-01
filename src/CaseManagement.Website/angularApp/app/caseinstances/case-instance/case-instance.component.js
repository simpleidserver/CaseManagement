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
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { ActionTypes } from './case-instance-actions';
import { CaseInstance } from '../../casedefinitions/models/search-case-instances-result.model';
import { MatPaginator, MatSort } from '@angular/material';
import { merge } from 'rxjs';
var CmmnViewer = require('cmmn-js');
var CaseInstanceComponent = (function () {
    function CaseInstanceComponent(store, route) {
        this.store = store;
        this.route = route;
        this.caseInstance = new CaseInstance();
        this.displayedColumns = ['name', 'start_datetime', 'end_datetime'];
    }
    CaseInstanceComponent.prototype.ngOnInit = function () {
        var self = this;
        self.viewer = new CmmnViewer({
            container: "#canvas"
        });
        self.subscription = self.store.pipe(select("caseInstance")).subscribe(function (st) {
            if (!st) {
                return;
            }
            self.isCaseInstanceLoading = st.isCaseInstanceLoading;
            self.isCaseInstanceErrorLoadOccured = st.isCaseInstanceErrorLoadOccured;
            self.isCaseExecutionStepsLoaded = st.isCaseExecutionStepsLoading;
            self.isCaseExecutionStepsErrorLoadOccured = st.isCaseExecutionStepsErrorLoadOccured;
            if (st.caseDefinition && st.caseInstance) {
                self.caseInstance = st.caseInstance;
                self.viewer.importXML(st.caseDefinition.Xml, function (err) {
                    if (err) {
                        return;
                    }
                    self.viewer.get('canvas').zoom('fit-viewport');
                    var canvas = self.viewer.get('canvas');
                    st.caseInstance.PlanItems.forEach(function (pi) {
                        canvas.addMarker(pi.Id, pi.Status);
                    });
                });
            }
            if (st.executionStepsResult) {
                self.length = st.executionStepsResult.TotalLength;
                self.executionSteps = st.executionStepsResult.Content;
            }
        });
        self.init();
    };
    CaseInstanceComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    CaseInstanceComponent.prototype.init = function () {
        var id = this.route.snapshot.params['id'];
        this.isCaseInstanceLoading = true;
        this.isCaseInstanceErrorLoadOccured = false;
        var loadCaseInstanceRequest = {
            type: ActionTypes.CASEINSTANCELOAD,
            id: id
        };
        this.store.dispatch(loadCaseInstanceRequest);
        this.refresh();
    };
    CaseInstanceComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        this.isCaseExecutionStepsLoaded = true;
        this.isCaseInstanceErrorLoadOccured = false;
        var loadCaseExecutionStepsRequest = {
            id: id,
            type: ActionTypes.CASEEXECUTIONSSTEPSLOAD,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize
        };
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            loadCaseExecutionStepsRequest['startIndex'] = this.paginator.pageIndex * this.paginator.pageSize;
        }
        else {
            loadCaseExecutionStepsRequest['startIndex'] = 0;
        }
        if (this.paginator.pageSize) {
            loadCaseExecutionStepsRequest['count'] = this.paginator.pageSize;
        }
        else {
            loadCaseExecutionStepsRequest['count'] = 5;
        }
        this.store.dispatch(loadCaseExecutionStepsRequest);
    };
    CaseInstanceComponent.prototype.ngOnDestroy = function () {
        this.subscription.unsubscribe();
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], CaseInstanceComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], CaseInstanceComponent.prototype, "sort", void 0);
    CaseInstanceComponent = __decorate([
        Component({
            selector: 'case-instance',
            templateUrl: './case-instance.component.html',
            styleUrls: ['./case-instance.component.scss']
        }),
        __metadata("design:paramtypes", [Store, ActivatedRoute])
    ], CaseInstanceComponent);
    return CaseInstanceComponent;
}());
export { CaseInstanceComponent };
//# sourceMappingURL=case-instance.component.js.map