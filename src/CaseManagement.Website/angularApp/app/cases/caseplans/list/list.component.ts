import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator, MatSort } from '@angular/material';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { StartSearch } from '../actions/case-plans';
import { CasePlan } from '../models/case-plan.model';
import { SearchCasePlansResult } from '../models/search-case-plans-result.model';
import * as fromCaseFiles from '../reducers';

@Component({
    selector: 'list-case-plans',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListCasePlansComponent implements OnInit {
    displayedColumns: string[] = ['name', 'version', 'create_datetime', 'actions'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    searchForm: FormGroup;
    length: number;
    casePlans$: CasePlan[] = [];

    constructor(private store: Store<fromCaseFiles.CaseDefinitionsState>, private formBuilder: FormBuilder) {
        this.searchForm = this.formBuilder.group({
            text: ''
        });
    }

    ngOnInit() {
        this.store.pipe(select(fromCaseFiles.selectSearchResults)).subscribe((l: SearchCasePlansResult) => {
            if (!l || !l.Content) {
                return;
            }

            this.casePlans$ = l.Content;
            this.length = l.TotalLength;
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

        let request = new StartSearch(this.sort.active, this.sort.direction, count, startIndex, this.searchForm.get('text').value);
        this.store.dispatch(request);
    }
}
