import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatPaginator, MatSnackBar, MatSort } from '@angular/material';
import { Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { SearchCmmnFilesResult } from '@app/stores/cmmnfiles/models/search-cmmn-files-result.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { AddCmmnFileDialog } from './add-cmmn-file-dialog';

@Component({
    selector: 'list-cmmn-files',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListCmmnFilesComponent implements OnInit {
    displayedColumns: string[] = ['name', 'version', 'status', 'create_datetime', 'update_datetime'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    searchForm: FormGroup;
    length: number;
    cmmnFiles$: CmmnFile[] = [];

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
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.COMPLETE_ADD_CMMNFILE))
            .subscribe((evt : any) => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.CASES_FILE_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.route.navigate(["/cmmns/cmmnfiles/" + evt.id]);
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.ERROR_ADD_CMMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_ADD_CASE_FILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectCmmnFileLstResult)).subscribe((searchCmmnFilesResult: SearchCmmnFilesResult) => {
            if (!searchCmmnFilesResult) {
                return;
            }

            this.cmmnFiles$ = searchCmmnFilesResult.content;
            this.length = searchCmmnFilesResult.totalLength;
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
        const dialogRef = this.dialog.open(AddCmmnFileDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((e : any) => {
            if (!e) {
                return;
            }

            const request = new fromCmmnFileActions.AddCmmnFile(e.name, e.description);
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

        const request = new fromCmmnFileActions.SearchCmmnFiles(active, direction, count, startIndex, this.searchForm.get('text').value, null, true);
        this.store.dispatch(request);
    }
}
