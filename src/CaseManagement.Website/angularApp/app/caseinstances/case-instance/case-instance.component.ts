import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { ActionTypes } from './case-instance-actions';
import { CaseInstanceState } from './case-instance-states';
import { CaseInstancePlanItem, CaseInstance } from '../../casedefinitions/models/search-case-instances-result.model';
import { SearchCaseExecutionStepItem } from '../../casedefinitions/models/search-case-execution-steps-result.model';
import { MatPaginator, MatSort } from '@angular/material';
import { merge } from 'rxjs';
let CmmnViewer = require('cmmn-js');

@Component({
    selector: 'case-instance',
    templateUrl: './case-instance.component.html',  
  	styleUrls: ['./case-instance.component.scss']
})
export class CaseInstanceComponent implements OnInit, OnDestroy {
    viewer: any;
    isCaseInstanceLoading: boolean;
    isCaseInstanceErrorLoadOccured: boolean;
    isCaseExecutionStepsLoaded: boolean;
    isCaseExecutionStepsErrorLoadOccured: boolean;
    subscription: any;
    caseInstance: CaseInstance = new CaseInstance();
    executionSteps: SearchCaseExecutionStepItem[];
    displayedColumns: string[] = ['name', 'start_datetime', 'end_datetime'];
    length: number;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;


    constructor(private store: Store<CaseInstanceState>, private route: ActivatedRoute) { }

    ngOnInit() {
        let self = this;
        self.viewer = new CmmnViewer({
            container: "#canvas"
        });
        self.subscription = self.store.pipe(select("caseInstance")).subscribe((st: CaseInstanceState) => {
            if (!st) {
                return;
            }

            self.isCaseInstanceLoading = st.isCaseInstanceLoading;
            self.isCaseInstanceErrorLoadOccured = st.isCaseInstanceErrorLoadOccured;
            self.isCaseExecutionStepsLoaded = st.isCaseExecutionStepsLoading;
            self.isCaseExecutionStepsErrorLoadOccured = st.isCaseExecutionStepsErrorLoadOccured;
            if (st.caseDefinition && st.caseInstance) {
                self.caseInstance = st.caseInstance;
                self.viewer.importXML(st.caseDefinition.Xml, function (err: any) {
                    if (err) {
                        return;
                    }

                    self.viewer.get('canvas').zoom('fit-viewport');
                    let canvas = self.viewer.get('canvas');
                    st.caseInstance.PlanItems.forEach(function (pi: CaseInstancePlanItem) {
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
    }
    
    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    init() {
        var id = this.route.snapshot.params['id'];
        this.isCaseInstanceLoading = true;
        this.isCaseInstanceErrorLoadOccured = false;
        let loadCaseInstanceRequest: any = {
            type: ActionTypes.CASEINSTANCELOAD,
            id: id
        };

        this.store.dispatch(loadCaseInstanceRequest);
        this.refresh();
    }

    refresh() {
        var id = this.route.snapshot.params['id'];
        this.isCaseExecutionStepsLoaded = true;
        this.isCaseInstanceErrorLoadOccured = false;
        let loadCaseExecutionStepsRequest: any = {
            id: id,
            type: ActionTypes.CASEEXECUTIONSSTEPSLOAD,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize
        };
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            loadCaseExecutionStepsRequest['startIndex'] = this.paginator.pageIndex * this.paginator.pageSize;
        } else {
            loadCaseExecutionStepsRequest['startIndex'] = 0;
        }

        if (this.paginator.pageSize) {
            loadCaseExecutionStepsRequest['count'] = this.paginator.pageSize;
        } else {
            loadCaseExecutionStepsRequest['count'] = 5;
        }

        this.store.dispatch(loadCaseExecutionStepsRequest);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}