import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { StartSearch } from '../actions/case-files';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseFilesResult } from '../models/search-case-files-result.model';
import * as fromCaseFiles from '../reducers';
import { AddCaseFileDialog } from './add-case-file-dialog';

@Component({
    selector: 'list-case-files',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListCaseFilesComponent implements OnInit {
    displayedColumns: string[] = ['name', 'version', 'status', 'create_datetime', 'update_datetime', 'actions'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    searchForm: FormGroup;
    length: number;
    caseFiles$: CaseFile[] = [];

    constructor(private store: Store<fromCaseFiles.CaseFilesState>, private formBuilder: FormBuilder,  private dialog: MatDialog) {
        this.searchForm = this.formBuilder.group({
            text: ''
        });
    }

    ngOnInit() {
        this.store.pipe(select(fromCaseFiles.selectSearchResults)).subscribe((searchCaseFilesResult : SearchCaseFilesResult) => {
            if (!searchCaseFilesResult) {
                return;
            }

            this.caseFiles$ = searchCaseFilesResult.Content;
            this.length = searchCaseFilesResult.TotalLength;
        });
        this.refresh();
    }

    onSubmit() {
        this.refresh();
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    addCaseFile() {
        let dialogRef = this.dialog.open(AddCaseFileDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe(() => {
            this.refresh();
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

        let active = "create_datetime";
        let direction = "desc";
        if (this.sort.active) {
            active = this.sort.active;
        }

        if (this.sort.direction) {
            direction = this.sort.direction;
        }

        let request = new StartSearch(active, direction, count, startIndex, this.searchForm.get('text').value);
        this.store.dispatch(request);
    }
}
