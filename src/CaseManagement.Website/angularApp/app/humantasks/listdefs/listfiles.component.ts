import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatPaginator, MatSort, MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefsActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { SearchHumanTaskDefsResult } from '@app/stores/humantaskdefs/models/searchhumantaskdef.model';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { merge } from 'rxjs';
import { AddHumanTaskDefDialog } from './add-humantaskdef-dialog.component';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'list-humantask-files',
    templateUrl: './listfiles.component.html',
    styleUrls: ['./listfiles.component.scss']
})
export class ListHumanTaskFilesComponent implements OnInit, OnDestroy {
    humanTaskDefsListener: any;
    displayedColumns: string[] = [ 'name', 'version', 'nbInstances', 'create_datetime', 'update_datetime' ];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    humanTaskFiles$: HumanTaskDef[] = [];

    constructor(
        private store: Store<fromAppState.AppState>,
        private dialog: MatDialog,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private snackBar: MatSnackBar) {
    }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefsActions.ActionTypes.COMPLETE_ADD_HUMANTASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.HUMANTASK_CREATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefsActions.ActionTypes.ERROR_ADD_HUMANTASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_ADD_HUMANTASKDEF'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.humanTaskDefsListener = this.store.pipe(select(fromAppState.selectHumanTasksResult)).subscribe((searchHumanTaskFilesResult: SearchHumanTaskDefsResult) => {
            if (!searchHumanTaskFilesResult) {
                return;
            }

            this.humanTaskFiles$ = searchHumanTaskFilesResult.content;
            this.length = searchHumanTaskFilesResult.totalLength;
        });
        this.refresh();
    }

    ngOnDestroy(): void {
        this.humanTaskDefsListener.unsubscribe();
    }

    onSubmit() {
        this.refresh();
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    addHumanTask() {
        const dialogRef = this.dialog.open(AddHumanTaskDefDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((e: any) => {
            if (!e) {
                return;
            }


            const request = new fromHumanTaskDefsActions.AddHumanTaskDefOperation(e.name);
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

        const request = new fromHumanTaskDefsActions.SearchHumanTaskDefOperation(active, direction, count, startIndex);
        this.store.dispatch(request);
    }
}
