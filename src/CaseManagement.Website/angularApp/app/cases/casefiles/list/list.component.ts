import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatPaginator, MatSnackBar, MatSort } from '@angular/material';
import { Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCaseFileActions from '@app/stores/casefiles/actions/case-files.actions';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { CaseFile } from '../../../stores/casefiles/models/case-file.model';
import { SearchCaseFilesResult } from '../../../stores/casefiles/models/search-case-files-result.model';
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

    constructor(private store: Store<fromAppState.AppState>,
        private formBuilder: FormBuilder,
        private dialog: MatDialog,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private snackBar: MatSnackBar,
        private route: Router) {
        this.searchForm = this.formBuilder.group({
            text: ''
        });
    }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseFileActions.ActionTypes.COMPLETE_ADD_CASEFILE))
            .subscribe((evt : any) => {
                this.snackBar.open(this.translateService.instant('CASES_FILE_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.route.navigate(["/cases/casefiles/" + evt.id]);
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCaseFileActions.ActionTypes.ERROR_ADD_CASEFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('ERROR_ADD_CASE_FILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectCaseFileLstResult)).subscribe((searchCaseFilesResult : SearchCaseFilesResult) => {
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

    addCaseFile() {
        const dialogRef = this.dialog.open(AddCaseFileDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((e : any) => {
            if (!e) {
                return;
            }

            const request = new fromCaseFileActions.AddCaseFile(e.name, e.description);
            this.store.dispatch(request);
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

        const request = new fromCaseFileActions.SearchCaseFiles(active, direction, count, startIndex, this.searchForm.get('text').value);
        this.store.dispatch(request);
    }
}
