import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { BpmnFile } from '@app/stores/bpmnfiles/models/bpmn-file.model';
import { SearchBpmnFilesResult } from '@app/stores/bpmnfiles/models/search-bpmn-files-result.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';

@Component({
    selector: 'list-bpmn-files',
    templateUrl: './listfiles.component.html',
    styleUrls: ['./listfiles.component.scss']
})
export class ListBpmnFilesComponent implements OnInit {
    displayedColumns: string[] = [ 'name', 'nbInstances', 'version', 'status', 'create_datetime', 'update_datetime' ];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    bpmnFiles$: BpmnFile[] = [];

    constructor(private store: Store<fromAppState.AppState>) {
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectBpmnFilesResult)).subscribe((searchBpmnFilesResult: SearchBpmnFilesResult) => {
            if (!searchBpmnFilesResult) {
                return;
            }

            this.bpmnFiles$ = searchBpmnFilesResult.content;
            this.length = searchBpmnFilesResult.totalLength;
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

        const request = new fromBpmnFileActions.SearchBpmnFiles(active, direction, count, startIndex, true, null);
        this.store.dispatch(request);
    }
}
