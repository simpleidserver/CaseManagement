import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import * as fromAppState from '../../../stores/appstate';
import { StartSearch } from '../../../stores/caseplans/actions/caseplan.actions';
import { CasePlan } from '../../../stores/caseplans/models/caseplan.model';
import { SearchCasePlanResult } from '../../../stores/caseplans/models/searchcaseplanresult.model';

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

    constructor(private store: Store<fromAppState.AppState>, private formBuilder: FormBuilder, private activatedRoute: ActivatedRoute) {
        this.searchForm = this.formBuilder.group({
            text: '',
            caseFileId: ''
        });
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectCasePlanLstResult)).subscribe((l: SearchCasePlanResult) => {
            if (!l || !l.content) {
                return;
            }

            this.casePlans$ = l.content;
            this.length = l.totalLength;
        });
        this.activatedRoute.queryParams.subscribe(params => {
            const caseFileId = params['caseFileId'];
            if (caseFileId) {
                this.searchForm.controls['caseFileId'].setValue(caseFileId);
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

        let request = new StartSearch(active, direction, count, startIndex, this.searchForm.get('text').value, this.searchForm.get('caseFileId').value);
        this.store.dispatch(request);
    }
}
