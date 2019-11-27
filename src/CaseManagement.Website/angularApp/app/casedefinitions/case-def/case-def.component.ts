import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { SearchCaseInstanceItem } from '../models/search-case-instances-result.model';
import { ActionTypes } from './case-def-actions';
import * as fromListCaseDefState from './case-def-states';
import { merge } from 'rxjs';
let CmmnViewer = require('cmmn-js');

@Component({
    selector: 'case-def',
    templateUrl: './case-def.component.html',  
  	styleUrls: ['./case-def.component.scss']
})

export class CaseDefComponent implements OnInit, OnDestroy {
    // isLoading: boolean;
    // isErrorLoadOccured: boolean;
    caseInstances: SearchCaseInstanceItem[] = [];
    displayedColumns: string[] = ['Id', 'Name', 'Status', 'CreateDateTime', 'Actions'];
    subscription: any;
    viewer: any;
    length: number;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(private store: Store<fromListCaseDefState.CaseDefState>, private route: ActivatedRoute) { }

    ngOnInit() {
        let self = this;
		// this.isLoading = true;		
        // this.isErrorLoadOccured = false;
        this.viewer = new CmmnViewer({
            container: '#canvas'
        });
        this.subscription = this.store.pipe(select('caseDef')).subscribe((st: fromListCaseDefState.CaseDefState) => {
            if (!st) {
                return;
            }

            // this.isLoading = st.;
            // this.isErrorLoadOccured = st.isErrorLoadOccured;
            if (st.caseDefinitionContent) {
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
        this.refresh();
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
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