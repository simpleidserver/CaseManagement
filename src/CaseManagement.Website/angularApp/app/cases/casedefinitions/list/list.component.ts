import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator, MatSort } from '@angular/material';
import { select, Store } from '@ngrx/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { merge, Observable } from 'rxjs';
import { StartFetch } from '../actions/case-definitions';
import { CaseDefinition } from '../models/case-definition.model';
import * as fromCaseFiles from '../reducers';

@Component({
    selector: 'list-case-files',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListCaseDefinitionsComponent implements OnInit {
    displayedColumns: string[] = ['name', 'create_datetime', 'case_file'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    searchForm: FormGroup;
    isLoading: boolean;
    length: number;
    isErrorLoadOccured: boolean;
    caseDefinitions$: Observable<CaseDefinition[]>;

    constructor(private store: Store<fromCaseFiles.CaseDefinitionsState>, private formBuilder: FormBuilder, private oauthService: OAuthService) {
        this.searchForm = this.formBuilder.group({
            text: ''
        });
    }

    ngOnInit() {
        this.caseDefinitions$ = this.store.pipe(select(fromCaseFiles.selectSearchResults));
        this.store.pipe(select(fromCaseFiles.selectLengthResults)).subscribe((l : number) => {
            this.length = l;
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

        let claims : any = this.oauthService.getIdentityClaims();
        let request = new StartFetch(this.sort.active, this.sort.direction, count, startIndex, this.searchForm.get('text').value, claims.sub);
        this.store.dispatch(request);
    }
}
