import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnPlanInstancesActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
import { CmmnPlanInstanceResult } from '@app/stores/cmmninstances/models/cmmn-planinstance.model';
import { SearchCasePlanInstanceResult } from '@app/stores/cmmninstances/models/searchcmmnplaninstanceresult.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';

@Component({
    selector: 'view-cmmn-instances-plan',
    templateUrl: './instances.component.html',
    styleUrls: ['./instances.component.scss']
})
export class ViewCmmnPlanInstancesComponent implements OnInit {
    displayedColumns: string[] = ['name', 'state', 'create_datetime', 'update_datetime'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    cmmnPlanInstances$: CmmnPlanInstanceResult[] = [];

    constructor(
        private route: ActivatedRoute,
        private store: Store<fromAppState.AppState>
    ) { }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectCmmnPlanInstanceLstResult)).subscribe((searchCasePlanInstanceResult: SearchCasePlanInstanceResult) => {
            if (!searchCasePlanInstanceResult) {
                return;
            }

            this.cmmnPlanInstances$ = searchCasePlanInstanceResult.content;
            this.length = searchCasePlanInstanceResult.totalLength;
        });
        this.refresh();
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    refresh() {
        let startIndex: number = 0;
        let count: number = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }

        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }

        let active = "create_datetime";
        let direction = "desc";
        if (this.sort.active) {
            active = this.sort.active;
        }

        if (this.sort.direction) {
            direction = this.sort.direction;
        }

        const id = this.route.parent.snapshot.params['id'];
        const request = new fromCmmnPlanInstancesActions.SearchCmmnPlanInstance(active, direction, count, startIndex, id);
        this.store.dispatch(request);
    }
}