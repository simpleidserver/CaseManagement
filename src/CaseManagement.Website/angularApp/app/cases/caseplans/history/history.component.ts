import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import * as fromAppState from '../../../stores/appstate';
import { StartSearchHistory } from '../../../stores/caseplans/actions/caseplan.actions';
import { CasePlan } from '../../../stores/caseplans/models/caseplan.model';
import { SearchCasePlanResult } from '../../../stores/caseplans/models/searchcaseplanresult.model';

@Component({
    selector: 'history-case-plan',
    templateUrl: './history.component.html',
    styleUrls: ['./history.component.scss']
})
export class HistoryCasePlanComponent implements OnInit {
    displayedColumns: string[] = ['name', 'version', 'create_datetime', 'actions'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    casePlans$: CasePlan[] = [];

    constructor(private route: ActivatedRoute, private store: Store<fromAppState.AppState>) { }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectCasePlanHistoryLstResult)).subscribe((searchCaseFilesResult: SearchCasePlanResult) => {
            if (!searchCaseFilesResult) {
                return;
            }

            this.casePlans$ = searchCaseFilesResult.content;
            this.length = searchCaseFilesResult.totalLength;
        });
        this.refresh();
    }

    onSubmit() {
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

        let request = new StartSearchHistory(this.route.snapshot.params['id'], active, direction, count, startIndex);
        this.store.dispatch(request);
    }
}
