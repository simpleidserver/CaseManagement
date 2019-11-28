import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { CaseDefinition } from '../models/case-def.model';
import { CaseInstance } from '../models/search-case-instances-result.model';
import { ActionTypes } from './case-def-actions';
import * as fromListCaseDefState from './case-def-states';
import { CaseInstancesService } from '../services/caseinstances.service';
let CmmnViewer = require('cmmn-js');

@Component({
    selector: 'case-def',
    templateUrl: './case-def.component.html',  
  	styleUrls: ['./case-def.component.scss']
})

export class CaseDefComponent implements OnInit, OnDestroy {
    isCaseDefinitionLoading: boolean;
    isCaseDefinitionErrorLoadOccured: boolean;
    isCaseInstancesLoading: boolean;
    isCaseInstancesErrorLoadOccured: boolean;
    caseDefinition: CaseDefinition = new CaseDefinition();
    selectedCasePlanModel: string;
    caseInstances: CaseInstance[] = [];
    displayedColumns: string[] = ['name', 'status', 'create_datetime', 'Actions'];
    subscription: any;
    viewer: any;
    length: number;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(private store: Store<fromListCaseDefState.CaseDefState>, private route: ActivatedRoute, private caseInstanceService: CaseInstancesService) { }

    ngOnInit() {
        let self = this;
        self.isCaseDefinitionLoading = true;		
        self.isCaseDefinitionErrorLoadOccured = false;
        self.viewer = new CmmnViewer({
            container: '#canvas'
        });
        self.store.pipe()
        self.subscription = self.store.pipe(select('caseDef')).subscribe((st: fromListCaseDefState.CaseDefState) => {
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
                self.viewer.importXML(st.caseDefinitionContent.Xml, function (err : any) {
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
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    createCaseInstance(e: any) {
        e.preventDefault();
        let self = this;
        self.caseInstanceService.create(this.caseDefinition.Id, this.selectedCasePlanModel).subscribe(() => {
            self.refresh();
        });
    }

    launchInstance(caseInstance: CaseInstance) {
        let self = this;
        self.caseInstanceService.launch(caseInstance.Id).subscribe(() => {
            self.refresh();
        });
    }

    refresh() {
        var id = this.route.snapshot.params['id'];
        let loadDefRequest: any = {
            type: ActionTypes.CASEDEFLOAD,
            id : id
        };
        let loadInstancesRequest: any = {
            type: ActionTypes.CASEINSTANCESLOAD,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize,
            id: id
        };
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            loadInstancesRequest['startIndex'] = this.paginator.pageIndex * this.paginator.pageSize;
        } else {
            loadInstancesRequest['startIndex'] = 0;
        }

        if (this.paginator.pageSize) {
            loadInstancesRequest['count'] = this.paginator.pageSize;
        } else {
            loadInstancesRequest['count'] = 5;
        }

        this.store.dispatch(loadDefRequest);
        this.store.dispatch(loadInstancesRequest);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}