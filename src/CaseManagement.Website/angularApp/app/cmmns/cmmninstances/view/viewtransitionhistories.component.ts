import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { CmmnPlanInstanceResult, TransitionHistoryResult, CmmnPlanItemInstanceResult } from '@app/stores/cmmninstances/models/cmmn-planinstance.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'view-transition-histories',
    templateUrl: './viewtransitionhistories.component.html',
    styleUrls: ['./viewtransitionhistories.component.scss']
})
export class ViewTransitionHistoriesComponent implements OnInit {
    displayedColumns: string[] = ['transition', 'executionDateTime', 'message'];
    length: number;
    transitionHistories$: MatTableDataSource<TransitionHistoryResult> = new MatTableDataSource<TransitionHistoryResult>();
    @ViewChild(MatSort) sort: MatSort;
    cmmnInstance: CmmnPlanInstanceResult = null;

    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectCmmnPlanInstanceResult)).subscribe((e: CmmnPlanInstanceResult) => {
            if (!e) {
                return;
            }

            this.cmmnInstance = e;
            this.refresh();
        });
        this.route.params.subscribe(() => {
            this.refresh();
        });
    }

    ngAfterViewInit() {
        this.transitionHistories$.sort = this.sort;
    }

    refresh() {
        if (!this.cmmnInstance) {
            return;
        }

        const id = this.route.snapshot.params['instid'];
        const filteredExecutionPath = this.cmmnInstance.children.filter(function (ep: CmmnPlanItemInstanceResult) {
            return ep.id === id;
        });
        if (filteredExecutionPath.length === 1) {
            this.transitionHistories$.data = filteredExecutionPath[0].transitionHistories;
        }
    }
}
