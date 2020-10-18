import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatPaginator, MatSnackBar, MatSort } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { SearchHumanTaskDefsResult } from '@app/stores/humantaskdefs/models/searchhumantaskdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { merge } from 'rxjs';
import { filter } from 'rxjs/operators';
import { AddHumanTaskDefDialog } from './add-humantaskdef-dialog.component';

@Component({
    selector: 'view-list-humantaskdef-component',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ListHumanTaskDef implements OnInit {
    displayedColumns: string[] = ['name', 'priority', 'create_datetime', 'update_datetime', 'actions'];
    humanTaskDefs$: HumanTaskDef[] = [];
    length: number = 0;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    baseTranslationKey: string = 'HUMANTASK.DEF.LIST';

    constructor(
        private actions$: ScannedActionsSubject,
        private store: Store<fromAppState.AppState>,
        private dialog: MatDialog,
        private snackBar: MatSnackBar,
        private translateService: TranslateService) {
    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_HUMANTASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.HUMANTASK_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_HUMANTASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_ADD_HUMANTASK'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectHumanTasksResult)).subscribe((searchHumanTaskDefsResult: SearchHumanTaskDefsResult) => {
            if (!searchHumanTaskDefsResult) {
                return;
            }

            this.humanTaskDefs$ = searchHumanTaskDefsResult.content;
            this.length = searchHumanTaskDefsResult.totalLength;
        });
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


            const request = new fromHumanTaskDefActions.AddHumanTaskDefOperation(e.name);
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

        const request = new fromHumanTaskDefActions.SearchHumanTaskDefOperation(active, direction, count, startIndex);
        this.store.dispatch(request);
    }
}
