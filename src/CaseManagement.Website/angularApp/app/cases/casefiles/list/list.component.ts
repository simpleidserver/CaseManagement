import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { select, Store } from '@ngrx/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { merge, Observable } from 'rxjs';
import { StartFetch } from '../actions/case-files';
import { CaseFile } from '../models/case-file.model';
import * as fromCaseFiles from '../reducers';
import { AddCaseFileDialog } from './add-case-file-dialog';

@Component({
    selector: 'list-case-files',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListCaseFilesComponent implements OnInit {
    displayedColumns: string[] = ['name', 'create_datetime', 'update_datetime'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    searchForm: FormGroup;
    isLoading: boolean;
    length: number;
    isErrorLoadOccured: boolean;
    caseFiles$: Observable<CaseFile[]>;

    constructor(private store: Store<fromCaseFiles.CaseFilesState>, private formBuilder: FormBuilder, private oauthService: OAuthService, private dialog: MatDialog) {
        this.searchForm = this.formBuilder.group({
            text: ''
        });
    }

    ngOnInit() {
        this.caseFiles$ = this.store.pipe(select(fromCaseFiles.selectSearchResults));
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

        let claims : any = this.oauthService.getIdentityClaims();
        let request = new StartFetch(this.sort.active, this.sort.direction, count, startIndex, this.searchForm.get('text').value, claims.sub);
        this.store.dispatch(request);
    }
}
