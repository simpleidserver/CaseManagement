import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { CaseDefinition } from '../models/case-definition.model';
import { ActionTypes } from './list-actions';
import * as fromListCaseDefsState from './list-states';

@Component({
    selector: 'list-case-files',
    templateUrl: './list.component.html',  
    styleUrls: ['./list.component.scss']
})

export class ListCaseDefinitionsComponent implements OnInit, OnDestroy {
    isLoading: boolean;
    caseDefinitions: CaseDefinition[] = [];
    displayedColumns: string[] = [ 'name', 'create_datetime' ];
    isErrorLoadOccured: boolean;
    length: number;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    subscription: any;
	
    constructor(private store: Store<fromListCaseDefsState.ListCaseDefinitionsState>) { }

    ngOnInit() {
		this.isLoading = true;		
        this.isErrorLoadOccured = false;
        this.subscription = this.store.pipe(select('caseDefinitions')).subscribe((st: fromListCaseDefsState.ListCaseDefinitionsState) => {
            if (!st) {
                return;
            }

            this.isLoading = st.isLoading;
            this.isErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                this.caseDefinitions = st.content.Content;
                this.length = st.content.TotalLength;
            }
        });
        this.refresh();
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    refresh() {
        let request: any = {
            type: ActionTypes.CASEDEFINITIONSLOAD,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize
        };
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            request['startIndex'] = this.paginator.pageIndex * this.paginator.pageSize;
        } else {
            request['startIndex'] = 0;
        }

        if (this.paginator.pageSize) {
            request['count'] = this.paginator.pageSize;
        } else {
            request['count'] = 5;
        }

        this.isLoading = true;
        this.store.dispatch(request);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}