import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { SearchCasePlanInstances } from '@app/stores/caseplaninstances/actions/caseplaninstance.actions';
import { CasePlanInstanceResult } from '@app/stores/caseplaninstances/models/caseplaninstance.model';
import { SearchCasePlanInstanceResult } from '@app/stores/caseplaninstances/models/searchcaseplaninstanceresult.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';

@Component({
    selector: 'list-case-instances',
    templateUrl: './list.component.html',  
    styleUrls: ['./list.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ListCasePlanInstancesComponent implements OnInit {
    displayedColumns: string[] = ['id', 'state', 'create_datetime', 'actions'];
    length: number;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    casePlanInstances$: CasePlanInstanceResult[] = [];
    searchForm: FormGroup;

    constructor(private store: Store<fromAppState.AppState>, private formBuilder: FormBuilder, private activatedRoute: ActivatedRoute) {
        this.searchForm = this.formBuilder.group({
            casePlanId: ''
        });
    }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectCasePlanInstanceLstResult)).subscribe((searchCasePlanInstanceResult: SearchCasePlanInstanceResult) => {
            if (!searchCasePlanInstanceResult) {
                return;
            }

            this.length = searchCasePlanInstanceResult.totalLength;
            this.casePlanInstances$ = searchCasePlanInstanceResult.content;
        });

        this.activatedRoute.queryParams.subscribe(params => {
            const casePlanId = params['casePlanId'];
            if (casePlanId) {
                this.searchForm.controls['casePlanId'].setValue(casePlanId);
            }

            this.refresh();
        });
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

        const request = new SearchCasePlanInstances(startIndex, count, active, direction, this.searchForm.get('casePlanId').value);
        this.store.dispatch(request);
    }
}