import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSelectChange, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import * as caseFilesActions from '../../casefiles/actions/case-files';
import * as caseActivationsActions from '../actions/case-activations';
import * as caseDefinitionsActions from '../actions/case-plans';
import * as caseFormInstancesActions from '../actions/case-form-instances';
import * as caseInstancesActions from '../actions/case-instances';
import { CaseActivation } from '../models/case-activation.model';
import { CasePlan } from '../models/case-plan.model';
import { CaseFormInstance } from '../models/case-form-instance.model';
import { CaseInstance } from '../models/case-instance.model';
import { SearchCaseActivationsResult } from '../models/search-case-activations-result.model';
import { SearchCaseFormInstancesResult } from '../models/search-case-form-instances-result.model';
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';
import * as fromCaseDefinitions from '../reducers';
import { CaseInstancesService } from '../services/caseinstances.service';

@Component({
    selector: 'view-case-files',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCaseDefinitionComponent implements OnInit, OnDestroy {
    selectedTimer: string = "4000";
    caseDefinition$: CasePlan = new CasePlan();
    caseInstances$: CaseInstance[] = new Array<CaseInstance>();
    caseFormInstances$:  CaseFormInstance[] = new Array<CaseFormInstance>();
    caseActivations$: CaseActivation[] = new Array<CaseActivation>();
    displayedColumns: string[] = ['id', 'state', 'create_datetime', 'actions'];
    formInstanceDisplayedColumns: string[] = ['form_id', 'case_instance_id', 'performer', 'update_datetime', 'create_datetime'];
    caseActivationDisplayedColumns: string[] = ['case_instance_name', 'case_instance_id', 'performer', 'create_datetime'];
    caseInstancesLength: number;
    formInstancesLength: number;
    caseActivationsLength: number;
    interval: any;
    @ViewChild('caseInstancesSort') caseInstancesSort: MatSort;
    @ViewChild('formInstancesSort') formInstancesSort: MatSort;
    @ViewChild('caseActivationsSort') caseActivationsSort: MatSort;
    @ViewChild('caseInstancesPaginator') caseInstancesPaginator: MatPaginator;
    @ViewChild('formInstancesPaginator') formInstancesPaginator: MatPaginator;
    @ViewChild('caseActivationsPaginator') caseActivationsPaginator: MatPaginator;

    constructor(private caseDefinitionStore: Store<fromCaseDefinitions.CaseDefinitionsState>, private route: ActivatedRoute, private caseInstancesService: CaseInstancesService) {  }

    ngOnInit() {
        this.caseDefinitionStore.pipe(select(fromCaseDefinitions.selectGetResult)).subscribe((caseDefinition: CasePlan) => {
            this.caseDefinition$ = caseDefinition;
            if (this.caseDefinition$.CaseFile) {
                let loadCaseFile = new caseFilesActions.StartGet(this.caseDefinition$.CaseFile);
                this.caseDefinitionStore.dispatch(loadCaseFile);
            }
        });
        this.caseDefinitionStore.pipe(select(fromCaseDefinitions.selectSearchInstancesResult)).subscribe((searchCaseInstancesResult: SearchCaseInstancesResult) => {
            this.caseInstances$ = searchCaseInstancesResult.Content;
            this.caseInstancesLength = searchCaseInstancesResult.TotalLength;
        });
        this.caseDefinitionStore.pipe(select(fromCaseDefinitions.selectSearchFormInstancesResult)).subscribe((searchCaseFormInstancesResult : SearchCaseFormInstancesResult) => {
            this.caseFormInstances$ = searchCaseFormInstancesResult.Content;
            this.formInstancesLength = searchCaseFormInstancesResult.TotalLength;
        });
        this.caseDefinitionStore.pipe(select(fromCaseDefinitions.selectSearchCaseActivationsResult)).subscribe((searchCaseActivationsResult: SearchCaseActivationsResult) => {
            this.caseActivations$ = searchCaseActivationsResult.Content;
            this.caseActivationsLength = searchCaseActivationsResult.TotalLength;
        });
        this.interval = setInterval(() => {
            this.refresh();
        }, 4000);
        this.refresh();
    }

    selectTimer(evt: MatSelectChange) {
        clearInterval(this.interval);
        this.interval = setInterval(() => {
            this.refresh();
        }, evt.value);
    }

    launchCaseInstance() {
        this.caseInstancesService.create(this.route.snapshot.params['id']).subscribe((caseInstance: CaseInstance) => {
            this.caseInstancesService.launch(caseInstance.Id).subscribe(() => {
                this.refresh();
            });
        });
    }

    reactivateCaseInstance(caseInstance: CaseInstance) {
        this.caseInstancesService.reactivateCaseInstance(caseInstance.Id).subscribe(() => {
            this.refresh();
        });
    }

    suspendCaseInstance(caseInstance: CaseInstance) {
        this.caseInstancesService.suspendCaseInstance(caseInstance.Id).subscribe(() => {
            this.refresh();
        });
    }

    resumeCaseInstance(caseInstance: CaseInstance) {
        this.caseInstancesService.resumeCaseInstance(caseInstance.Id).subscribe(() => {
            this.refresh();
        });
    }

    closeCaseInstance(caseInstance: CaseInstance) {
        this.caseInstancesService.closeCaseInstance(caseInstance.Id).subscribe(() => {
            this.refresh();
        });
    }

    ngAfterViewInit() {
        merge(this.caseInstancesSort.sortChange, this.caseInstancesPaginator.page).subscribe(() => this.refreshCaseInstances());
        merge(this.formInstancesSort.sortChange, this.formInstancesPaginator.page).subscribe(() => this.refreshFormInstances());
        merge(this.caseActivationsSort.sortChange, this.caseActivationsPaginator.page).subscribe(() => this.refreshCaseActivations());
    }

    refresh() {
        this.refreshCaseDefinition();
        this.refreshCaseInstances();
        this.refreshFormInstances();
        this.refreshCaseActivations();
    }

    refreshCaseDefinition() {
        var id = this.route.snapshot.params['id'];
        let loadCaseDefinition = new caseDefinitionsActions.StartGet(id);
        this.caseDefinitionStore.dispatch(loadCaseDefinition);
    }

    refreshCaseInstances() {
        let startIndex = 0;
        let count = 5;
        if (this.caseInstancesPaginator.pageIndex && this.caseInstancesPaginator.pageSize) {
            startIndex = this.caseInstancesPaginator.pageIndex * this.caseInstancesPaginator.pageSize;
        }

        if (this.caseInstancesPaginator.pageSize) {
            count = this.caseInstancesPaginator.pageSize;
        }

        let loadCaseInstances = new caseInstancesActions.StartFetch(this.route.snapshot.params['id'], startIndex, count, this.caseInstancesSort.active, this.caseInstancesSort.direction);
        this.caseDefinitionStore.dispatch(loadCaseInstances);
    }

    refreshFormInstances() {
        let startIndex = 0;
        let count = 5;
        if (this.formInstancesPaginator.pageSize) {
            count = this.formInstancesPaginator.pageSize;
        }

        if (this.formInstancesPaginator.pageIndex && this.formInstancesPaginator.pageSize) {
            startIndex = this.formInstancesPaginator.pageIndex * this.formInstancesPaginator.pageSize;
        }

        let loadFormInstances = new caseFormInstancesActions.StartFetch(this.route.snapshot.params['id'], this.formInstancesSort.active, this.formInstancesSort.direction, count, startIndex);
        this.caseDefinitionStore.dispatch(loadFormInstances);
    }

    refreshCaseActivations() {
        let count = 5;
        let startIndex = 0;
        if (this.caseActivationsPaginator.pageSize) {
            count = this.caseActivationsPaginator.pageSize;
        }

        if (this.caseActivationsPaginator.pageIndex && this.caseActivationsPaginator.pageSize) {
            startIndex = this.caseActivationsPaginator.pageIndex * this.caseActivationsPaginator.pageSize;
        }

        let loadCaseActivations = new caseActivationsActions.StartFetch(this.route.snapshot.params['id'], this.caseActivationsSort.active, this.caseActivationsSort.direction, count, startIndex);
        this.caseDefinitionStore.dispatch(loadCaseActivations);
    }

    ngOnDestroy() {
        clearInterval(this.interval);
    }
}