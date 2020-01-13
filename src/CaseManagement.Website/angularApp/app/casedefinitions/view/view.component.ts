import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { CaseDefinitionHistory, CaseElementDefinitionHistory } from '../models/case-definition-history.model';
import { CaseDefinition } from '../models/case-definition.model';
import { CaseFormInstance } from '../models/case-form-instance.model';
import { CaseInstance } from '../models/case-instance.model';
import { CaseInstancesService } from '../services/caseinstances.service';
import { ActionTypes } from './view-actions';
import * as fromViewCaseDefStates from './view-states';
import { CaseActivation } from '../models/case-activation.model';
let CmmnViewer = require('cmmn-js/lib/NavigatedViewer');

@Component({
    selector: 'view-case-files',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCaseDefinitionComponent implements OnInit, OnDestroy {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    isCaseInstancesErrorLoadOccured: boolean;
    isCaseFormInstancesErrorLoadOccured: boolean;
    isCaseActivationsErrorLoadOccured: boolean;
    caseDefinition: CaseDefinition = {
        CaseFile: null,
        CreateDateTime: null,
        Description: null,
        Id: null,
        Name: null
    };
    caseDefinitionHistory: CaseDefinitionHistory;
    caseInstances: CaseInstance[];
    caseFormInstances: CaseFormInstance[];
    caseActivations: CaseActivation[];
    displayedColumns: string[] = ['id', 'state', 'create_datetime', 'actions'];
    formInstanceDisplayedColumns: string[] = ['form_id', 'case_instance_id', 'performer', 'update_datetime', 'create_datetime'];
    caseActivationDisplayedColumns: string[] = ['case_instance_name', 'case_instance_id', 'performer', 'create_datetime'];
    subscription: any;
    subscriptionCaseInstances: any;
    subscriptionFormInstances: any;
    subscriptionCaseActivations: any;
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

    constructor(private caseDefinitionStore: Store<fromViewCaseDefStates.ViewCaseDefinitionState>, private caseInstancesStore: Store<fromViewCaseDefStates.ViewCaseInstancesState>, private formInstancesStore: Store<fromViewCaseDefStates.ViewFormInstancesState>, private caseActivationsStore: Store<fromViewCaseDefStates.ViewCaseActivationsState>, private route: ActivatedRoute, private caseInstancesService: CaseInstancesService) {
        this.caseDefinitionHistory = {
            Id: null,
            Elements: [],
            NbInstances: 0
        };
    }

    ngOnInit() {
        let viewer = new CmmnViewer({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        let self = this;
        this.subscription = this.caseDefinitionStore.pipe(select('caseDefinition')).subscribe((st: fromViewCaseDefStates.ViewCaseDefinitionState) => {
            if (!st) {
                return;
            }

            if (this.isLoading == true && !st.isLoading) {
                this.isLoading = st.isLoading;
                this.isErrorLoadOccured = st.isErrorLoadOccured;
                this.caseDefinitionHistory = st.caseDefinitionHistory;
                if (st.caseDefinition) {
                    this.caseDefinition = st.caseDefinition;
                    viewer.importXML(st.caseFile.Payload, function (err: any) {
                        if (err) {
                            return;
                        }

                        var canvas = viewer.get('canvas');
                        self.updateCanvas(viewer, st.caseDefinitionHistory);
                        canvas.zoom('fit-viewport');
                    });
                }
            }
            else if (this.isLoading == false && st.caseDefinitionHistory) {
                self.caseDefinitionHistory = st.caseDefinitionHistory;
                self.updateCanvas(viewer, st.caseDefinitionHistory);
            }
        });
        this.subscriptionCaseInstances = this.caseInstancesStore.pipe(select('caseInstances')).subscribe((st: fromViewCaseDefStates.ViewCaseInstancesState) => {
            if (!st) {
                return;
            }

            this.isCaseInstancesErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                this.caseInstances = st.content.Content;
                this.caseInstancesLength = st.content.TotalLength;
            }
        });
        this.subscriptionFormInstances = this.formInstancesStore.pipe(select('formInstances')).subscribe((st: fromViewCaseDefStates.ViewFormInstancesState) => {
            if (!st) {
                return;
            }

            this.isCaseFormInstancesErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                this.caseFormInstances = st.content.Content;
                this.formInstancesLength = st.content.TotalLength;
            }
        });
        this.subscriptionCaseActivations = this.caseActivationsStore.pipe(select('caseActivations')).subscribe((st: fromViewCaseDefStates.ViewCaseActivationsState) => {
            if (!st) {
                return;
            }

            this.isCaseActivationsErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                this.caseActivations = st.content.Content;
                this.caseActivationsLength = st.content.TotalLength;
            }
        });
        this.interval = setInterval(() => {
            this.refresh();
        }, 5000);
        this.isLoading = true;
        this.isErrorLoadOccured = false;
        this.refresh();
    }

    updateCanvas(viewer: any, caseDefinitionHistory: CaseDefinitionHistory) {
        if (caseDefinitionHistory.Elements.length > 0) {
            var overlays = viewer.get('overlays');
            caseDefinitionHistory.Elements.forEach(function (elt: CaseElementDefinitionHistory) {
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
    }

    createInstance() {
        this.caseInstancesService.create(this.route.snapshot.params['id']).subscribe(() => {
            this.refresh();
        });
    }

    launchCaseInstance(caseInstance: CaseInstance) {
        this.caseInstancesService.launch(caseInstance.Id).subscribe(() => {
            this.refresh();
        });
    }

    ngAfterViewInit() {
        merge(this.caseInstancesSort.sortChange, this.caseInstancesPaginator.page).subscribe(() => this.refreshCaseInstances());
        merge(this.formInstancesSort.sortChange, this.formInstancesPaginator.page).subscribe(() => this.refreshFormInstances());
        merge(this.caseActivationsSort.sortChange, this.caseActivationsPaginator.page).subscribe(() => this.refreshCaseActivations());
    }

    refresh() {
        var id = this.route.snapshot.params['id'];
        let loadCaseDefinition: any = {
            type: ActionTypes.CASEDEFINITIONLOAD,
            id: id
        };
        this.caseDefinitionStore.dispatch(loadCaseDefinition);
        this.refreshCaseInstances();
        this.refreshFormInstances();
        this.refreshCaseActivations();
    }

    refreshCaseInstances() {
        let loadCaseInstances: any = {
            type: ActionTypes.CASEINSTANCESLOAD,
            id: this.route.snapshot.params['id'],
            order: this.caseInstancesSort.active,
            direction: this.caseInstancesSort.direction,
            count: this.caseInstancesPaginator.pageSize
        };
        if (this.caseInstancesPaginator.pageIndex && this.caseInstancesPaginator.pageSize) {
            loadCaseInstances['startIndex'] = this.caseInstancesPaginator.pageIndex * this.caseInstancesPaginator.pageSize;
        } else {
            loadCaseInstances['startIndex'] = 0;
        }

        if (this.caseInstancesPaginator.pageSize) {
            loadCaseInstances['count'] = this.caseInstancesPaginator.pageSize;
        } else {
            loadCaseInstances['count'] = 5;
        }

        this.isCaseInstancesErrorLoadOccured = false;
        this.caseInstancesStore.dispatch(loadCaseInstances);
    }

    refreshFormInstances() {
        let loadFormInstances: any = {
            type: ActionTypes.CASEFORMINSTANCESLOAD,
            id: this.route.snapshot.params['id'],
            order: this.formInstancesSort.active,
            direction: this.formInstancesSort.direction,
            count: this.formInstancesPaginator.pageSize
        };
        if (this.formInstancesPaginator.pageIndex && this.formInstancesPaginator.pageSize) {
            loadFormInstances['startIndex'] = this.formInstancesPaginator.pageIndex * this.formInstancesPaginator.pageSize;
        } else {
            loadFormInstances['startIndex'] = 0;
        }

        if (this.formInstancesPaginator.pageSize) {
            loadFormInstances['count'] = this.formInstancesPaginator.pageSize;
        } else {
            loadFormInstances['count'] = 5;
        }

        this.isCaseFormInstancesErrorLoadOccured = false;
        this.formInstancesStore.dispatch(loadFormInstances);
    }

    refreshCaseActivations() {
        let loadCaseActivations: any = {
            type: ActionTypes.CASEACTIVATIONSLOAD,
            id: this.route.snapshot.params['id'],
            order: this.caseActivationsSort.active,
            direction: this.caseActivationsSort.direction,
            count: this.caseActivationsPaginator.pageSize
        };
        if (this.caseActivationsPaginator.pageIndex && this.caseActivationsPaginator.pageSize) {
            loadCaseActivations['startIndex'] = this.caseActivationsPaginator.pageIndex * this.caseActivationsPaginator.pageSize;
        } else {
            loadCaseActivations['startIndex'] = 0;
        }

        if (this.caseActivationsPaginator.pageSize) {
            loadCaseActivations['count'] = this.caseActivationsPaginator.pageSize;
        } else {
            loadCaseActivations['count'] = 5;
        }

        this.isCaseActivationsErrorLoadOccured = false;
        this.caseInstancesStore.dispatch(loadCaseActivations);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscriptionCaseInstances.unsubscribe();
        this.subscriptionFormInstances.unsubscribe();
        this.subscriptionCaseActivations.unsubscribe();
        clearInterval(this.interval);
    }
}