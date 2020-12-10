import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnPlanActions from '@app/stores/cmmnplans/actions/cmmn-plans.actions';
import * as fromCmmnInstanceActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
import { CmmnPlan } from '@app/stores/cmmnplans/models/cmmn-plan.model';
import { SearchCmmnPlanResult } from '@app/stores/cmmnplans/models/searchcmmnplanresult.model';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'list-cmmn-plans',
    templateUrl: './plans.component.html',
    styleUrls: ['./plans.component.scss']
})
export class ListCmmnPlansComponent implements OnInit {
    displayedColumns: string[] = ['name', 'version', 'create_datetime', 'update_datetime', 'nb_instances', 'actions'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    cmmnPlans$: CmmnPlan[] = [];

    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService) { }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnInstanceActions.ActionTypes.COMPLETE_LAUNCH_CMMN_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.PLAN_INSTANCE_LAUNCHED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.store.pipe(select(fromAppState.selectCmmnPlanLstResult)).subscribe((searchCmmnPlanResult: SearchCmmnPlanResult) => {
            if (!searchCmmnPlanResult) {
                return;
            }

            this.cmmnPlans$ = searchCmmnPlanResult.content;
            this.length = searchCmmnPlanResult.totalLength;
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

        const id = this.route.parent.snapshot.params['id'];
        const request = new fromCmmnPlanActions.SearchCmmnPlans(active, direction, count, startIndex, id);
        this.store.dispatch(request);
    }

    launch(cmmnPlan: CmmnPlan) {
        const request = new fromCmmnInstanceActions.LaunchCmmnPlanInstance(cmmnPlan.id);
        this.store.dispatch(request);
    }
}
