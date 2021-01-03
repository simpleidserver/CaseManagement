import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { SearchCases } from '@app/stores/cases/actions/cases.actions';
import { CaseInstance } from '@app/stores/cases/models/caseinstance.model';
import { SearchCaseInstanceResult } from '@app/stores/cases/models/search-caseinstance.model';
import { select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';

@Component({
    selector: 'list-cases-component',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListCasesComponent implements OnInit {
    displayedColumns: string[] = [ 'name', 'state', 'createDateTime' ];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    caseInstances$: CaseInstance[] = [];

    constructor(
        private store: Store<fromAppState.AppState>,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private translate: TranslateService) {

    }

    ngOnInit(): void {
        this.caseInstances$ = [ new CaseInstance(), new CaseInstance(), new CaseInstance() ];
        this.store.pipe(select(fromAppState.selectCaseLstResult)).subscribe((l: SearchCaseInstanceResult) => {
            if (!l || !l.content || l.content.length === 0) {
                return;
            }

            this.caseInstances$ = l.content;
            this.length = l.totalLength;
        });
        this.activatedRoute.queryParamMap.subscribe((p: any) => {
            this.sort.active = p.get('active');
            this.sort.direction = p.get('direction');
            this.paginator.pageSize = p.get('pageSize');
            this.paginator.pageIndex = p.get('pageIndex');
            this.refresh();
        });
        this.translate.onLangChange.subscribe(() => {
            this.refresh();
        });
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => {
            this.refreshUrl();
        });
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

        let active = this.getOrder();
        let direction = this.getDirection();
        let request = new SearchCases(active, direction, count, startIndex);
        this.store.dispatch(request);
    }

    refreshUrl() {
        const queryParams: any = {
            pageIndex: this.paginator.pageIndex,
            pageSize: this.paginator.pageSize,
            active: this.sort.active,
            direction: this.sort.direction
        };
        this.router.navigate(['.'], {
            relativeTo: this.activatedRoute,
            queryParams: queryParams
        });
    }

    private getOrder() {
        let active = "createdTime";
        if (this.sort.active) {
            active = this.sort.active;
        }

        return active;
    }

    private getDirection() {
        let direction = "desc";
        if (this.sort.direction) {
            direction = this.sort.direction;
        }

        return direction;
    }
}