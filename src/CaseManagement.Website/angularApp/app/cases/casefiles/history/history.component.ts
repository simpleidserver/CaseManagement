import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCaseFileActions from '@app/stores/casefiles/actions/case-files.actions';
import { CaseFile } from '@app/stores/casefiles/models/case-file.model';
import { SearchCaseFilesResult } from '@app/stores/casefiles/models/search-case-files-result.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';

@Component({
    selector: 'history-case-file',
    templateUrl: './history.component.html',
    styleUrls: ['./history.component.scss']
})
export class HistoryCaseFileComponent implements OnInit {
    displayedColumns: string[] = ['name', 'version', 'status', 'create_datetime', 'update_datetime', 'actions'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    caseFiles$: CaseFile[] = [];

    constructor(private route: ActivatedRoute, private store: Store<fromAppState.AppState>) { }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectCaseFileHistoryLstResult)).subscribe((searchCaseFilesResult: SearchCaseFilesResult) => {
            if (!searchCaseFilesResult) {
                return;
            }

            this.caseFiles$ = searchCaseFilesResult.content;
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

        const request = new fromCaseFileActions.SearchCaseFilesHistory(this.route.snapshot.params['id'], active, direction, count, startIndex);
        this.store.dispatch(request);
    }
}
