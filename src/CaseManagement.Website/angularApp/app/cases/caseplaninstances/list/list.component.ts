import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { select, Store } from '@ngrx/store';
import { CasePlanInstance } from '../models/caseplaninstance.model';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';
import * as fromCasePlanInstance from '../reducers';
import { StartSearch } from '../actions/caseplaninstance';
import { merge } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

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
    casePlanInstances$: CasePlanInstance[] = [];
    searchForm: FormGroup;

    constructor(private store: Store<fromCasePlanInstance.CasePlanInstanceState>, private formBuilder: FormBuilder, private activatedRoute: ActivatedRoute) {
        this.searchForm = this.formBuilder.group({
            casePlanId: ''
        });
    }

    ngOnInit(): void {
        this.store.pipe(select(fromCasePlanInstance.selectSearchResult)).subscribe((searchCasePlanInstanceResult: SearchCasePlanInstanceResult) => {
            if (!searchCasePlanInstanceResult) {
                return;
            }

            this.length = searchCasePlanInstanceResult.TotalLength;
            this.casePlanInstances$ = searchCasePlanInstanceResult.Content;
        });

        this.activatedRoute.queryParams.subscribe(params => {
            var casePlanId = params['casePlanId'];
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

        let request = new StartSearch(startIndex, count, active, direction, this.searchForm.get('casePlanId').value);
        this.store.dispatch(request);
    }
}