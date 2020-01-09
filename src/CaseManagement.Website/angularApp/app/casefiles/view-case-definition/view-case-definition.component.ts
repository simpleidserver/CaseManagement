import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { CaseDefinition } from '../models/case-definition.model';
import { CaseInstance } from '../models/case-instance.model';
import * as fromCaseDefinitionStates from './case-definition-states';
import * as fromCaseInstancesStates from './case-instances-states';
import { ActionTypes } from './view-case-definition-actions';
import { CaseInstancesService } from '../services/caseinstances.service';

@Component({
    selector: 'view-case-definition',
    templateUrl: './view-case-definition.component.html',  
    styleUrls: ['./view-case-definition.component.scss']
})
export class ViewCaseDefinitionComponent implements OnInit, OnDestroy {
    interval: any;
    displayedColumns: string[] = ['Id', 'State', 'CreateDateTime', 'Actions'];
    caseDefinition: CaseDefinition = {
        Id: null,
        Name: null,
        Description: null,
        CreateDateTime: null,
        CaseFile: null
    };
    caseInstances: CaseInstance[] = [];
    length: number;
    isCaseDefinitionLoading: boolean;
    isCaseDefinitionErrorLoadOccured: boolean;
    isCaseInstancesLoading: boolean;
    isCaseInstancesErrorLoadOccured: boolean;
    caseDefinitionStoreSubscription: any;
    caseInstancesSubscription: any;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(private caseDefinitionStore: Store<fromCaseDefinitionStates.CaseDefinitionState>, private caseInstancesStore: Store<fromCaseInstancesStates.CaseInstancesState>, private route: ActivatedRoute, private caseInstancesService: CaseInstancesService) { }

    ngOnInit() {
        this.isCaseDefinitionLoading = true;
        this.isCaseInstancesLoading = true;
        this.isCaseDefinitionErrorLoadOccured = false;
        this.isCaseInstancesErrorLoadOccured = false;
        this.caseDefinitionStoreSubscription = this.caseDefinitionStore.pipe(select('caseDefinition')).subscribe((st: fromCaseDefinitionStates.CaseDefinitionState) => {
            if (!st) {
                return;
            }

            this.isCaseDefinitionLoading = st.isLoading;
            this.isCaseDefinitionErrorLoadOccured = st.isErrorLoadOccured;
            if (st.caseDefinition) {
                this.caseDefinition = st.caseDefinition;
            }
        });
        this.caseInstancesSubscription = this.caseInstancesStore.pipe(select('caseInstances')).subscribe((st: fromCaseInstancesStates.CaseInstancesState) => {
            if (!st) {
                return;
            }

            this.isCaseInstancesLoading = st.isLoading;
            this.isCaseInstancesErrorLoadOccured = st.isErrorLoadOccured;
            if (st.caseInstances) {
                this.caseInstances = st.caseInstances.Content;
                this.length = st.caseInstances.TotalLength;
            }
        });

        this.init();
        this.interval = setInterval(() => {
            this.refresh();
        }, 5000);
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    init() {
        let id = this.route.snapshot.params['defid'];
        let request: any = {
            type: ActionTypes.LOADCASEDEFINITION,
            caseDefinitionId: id            
        };
        this.isCaseDefinitionLoading = true;
        this.caseDefinitionStore.dispatch(request);
        this.refresh();
    }

    refresh() {
        let id = this.route.snapshot.params['defid'];
        let request: any = {
            type: ActionTypes.LOADCASEINSTANCES,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize,
            caseDefinitionId: id
        };
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            request['startIndex'] = this.paginator.pageIndex * this.paginator.pageSize;
        } else {
            request['startIndex'] = 0;
        }

        if (this.paginator.pageSize) {
            request['count'] = this.paginator.pageSize;
        } else {
            request['count'] = 5;
        }

        this.isCaseInstancesLoading = true;
        this.caseInstancesStore.dispatch(request);
    }

    createCaseInstance() {
        let defid = this.route.snapshot.params['defid'];
        this.caseInstancesService.create(defid).subscribe(() => {
            this.refresh();
        });
    }

    launchCaseInstance(caseInstance: CaseInstance) {
        this.caseInstancesService.launch(caseInstance.Id).subscribe(() => {
            this.refresh();
        });
    }

    ngOnDestroy() {
        this.caseDefinitionStoreSubscription.unsubscribe();
        this.caseInstancesSubscription.unsubscribe();
        clearInterval(this.interval);
    }
}