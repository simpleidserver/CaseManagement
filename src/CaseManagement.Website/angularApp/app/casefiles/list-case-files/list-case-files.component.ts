import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { CaseFile } from '../models/case-file.model';
import { ActionTypes } from './list-case-files-actions';
import * as fromListCaseDefsState from './list-case-files-states';

@Component({
    selector: 'list-case-files',
    templateUrl: './list-case-files.component.html',  
    styleUrls: ['./list-case-files.component.scss']
})

export class ListCaseFilesComponent implements OnInit, OnDestroy {
    isLoading: boolean;
    caseFiles: CaseFile[] = [];
    displayedColumns: string[] = [ 'Id', 'Name', 'Description', 'CreateDateTime', 'Actions' ];
    isErrorLoadOccured: boolean;
    length: number;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    subscription: any;
	
    constructor(private store: Store<fromListCaseDefsState.ListCaseFilesState>) { }

    ngOnInit() {
		this.isLoading = true;		
        this.isErrorLoadOccured = false;		
        this.subscription = this.store.pipe(select('caseFiles')).subscribe((st: fromListCaseDefsState.ListCaseFilesState) => {
            if (!st) {
                return;
            }

            this.isLoading = st.isLoading;
            this.isErrorLoadOccured = st.isErrorLoadOccured;
            if (st.content) {
                this.caseFiles = st.content.Content;
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
            type: ActionTypes.CASEFILESLOAD,
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