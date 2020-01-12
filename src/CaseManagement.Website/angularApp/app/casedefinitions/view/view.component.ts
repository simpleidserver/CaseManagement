import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { CaseDefinitionHistory, CaseElementDefinitionHistory } from '../models/case-definition-history.model';
import { CaseDefinition } from '../models/case-definition.model';
import { CaseInstance } from '../models/case-instance.model';
import { CaseInstancesService } from '../services/caseinstances.service';
import { ActionTypes } from './view-actions';
import * as fromViewCaseDefStates from './view-states';
let CmmnViewer = require('cmmn-js/lib/NavigatedViewer');

@Component({
    selector: 'view-case-files',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCaseDefinitionComponent implements OnInit, OnDestroy {
    isLoading: boolean;
    isCaseInstancesLoading: boolean;
    isErrorLoadOccured: boolean;
    isCaseInstancesErrorLoadOccured: boolean;
    caseDefinition: CaseDefinition = {
        CaseFile: null,
        CreateDateTime: null,
        Description: null,
        Id: null,
        Name: null
    };
    caseDefinitionHistory: CaseDefinitionHistory;
    caseInstances: CaseInstance[];
    displayedColumns: string[] = ['id', 'state', 'create_datetime', 'actions'];
    subscription: any;
    subscriptionCaseInstances: any;
    length: number;
    interval: any;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(private caseDefinitionStore: Store<fromViewCaseDefStates.ViewCaseDefinitionState>, private caseInstancesStore: Store<fromViewCaseDefStates.ViewCaseInstancesState>, private route: ActivatedRoute, private caseInstancesService: CaseInstancesService) {
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
        this.subscriptionCaseInstances = this.caseDefinitionStore.pipe(select('caseInstances')).subscribe((st: fromViewCaseDefStates.ViewCaseInstancesState) => {
            if (!st) {
                return;
            }

            this.isCaseInstancesLoading = st.isLoading;
            this.isCaseInstancesErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                this.caseInstances = st.content.Content;
                this.length = st.content.TotalLength;
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
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    refresh() {
        var id = this.route.snapshot.params['id'];
        let loadCaseDefinition: any = {
            type: ActionTypes.CASEDEFINITIONLOAD,
            id: id
        };
        let loadCaseInstances: any = {
            type: ActionTypes.CASEINSTANCESLOAD,
            id: id,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize
        };
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            loadCaseInstances['startIndex'] = this.paginator.pageIndex * this.paginator.pageSize;
        } else {
            loadCaseInstances['startIndex'] = 0;
        }

        if (this.paginator.pageSize) {
            loadCaseInstances['count'] = this.paginator.pageSize;
        } else {
            loadCaseInstances['count'] = 5;
        }

        this.isCaseInstancesLoading = true;
        this.isCaseInstancesErrorLoadOccured = false;
        this.caseInstancesStore.dispatch(loadCaseInstances);
        this.caseDefinitionStore.dispatch(loadCaseDefinition);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscriptionCaseInstances.unsubscribe();
        clearInterval(this.interval);
    }
}