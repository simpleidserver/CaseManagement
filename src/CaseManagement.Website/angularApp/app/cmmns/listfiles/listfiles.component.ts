import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { SearchCmmnFilesResult } from '@app/stores/cmmnfiles/models/search-cmmn-files-result.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';

@Component({
    selector: 'list-cmmn-files',
    templateUrl: './listfiles.component.html',
    styleUrls: ['./listfiles.component.scss']
})
export class ListCmmnFilesComponent implements OnInit, OnDestroy {
    cmmnFilesListener: any;
    displayedColumns: string[] = [ 'name', 'version', 'status', 'create_datetime', 'update_datetime' ];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    cmmnFiles$: CmmnFile[] = [];

    constructor(private store: Store<fromAppState.AppState>) {
    }

    ngOnInit() {
        this.cmmnFilesListener = this.store.pipe(select(fromAppState.selectCmmnFileLstResult)).subscribe((searchCmmnFilesResult: SearchCmmnFilesResult) => {
            if (!searchCmmnFilesResult) {
                return;
            }

            this.cmmnFiles$ = searchCmmnFilesResult.content;
            this.length = searchCmmnFilesResult.totalLength;
        });
        this.refresh();
    }

    ngOnDestroy(): void {
        this.cmmnFilesListener.unsubscribe();
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

        const request = new fromCmmnFileActions.SearchCmmnFiles(active, direction, count, startIndex, null, null, true);
        this.store.dispatch(request);
    }
}
