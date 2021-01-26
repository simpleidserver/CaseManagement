import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatDialog, MatSnackBar } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromNotificationDefsActions from '@app/stores/notificationdefs/actions/notificationdef.actions';
import { NotificationDefinition } from '@app/stores/notificationdefs/models/notificationdef.model';
import { SearchNotificationDefsResult } from '@app/stores/notificationdefs/models/searchnotificationdef.model';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { AddNotificationDefDialog } from './add-notificationdef-dialog.component';
import { merge } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'list-notification-definitions',
    templateUrl: './listdefs.component.html',
    styleUrls: ['./listdefs.component.scss']
})
export class ListNotificationDefinitionsComponent implements OnInit, OnDestroy {
    notificationsListener: any;
    displayedColumns: string[] = [ 'name', 'version', 'nbInstances', 'create_datetime', 'update_datetime' ];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    notificationDefs$: NotificationDefinition[] = [];

    constructor(
        private store: Store<fromAppState.AppState>,
        private dialog: MatDialog,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private snackBar: MatSnackBar) {
    }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefsActions.ActionTypes.COMPLETE_ADD_NOTIFICATIONDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.NOTIFICATION_CREATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefsActions.ActionTypes.ERROR_ADD_NOTIFICATIONDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('NOTIFICATION.MESSAGES.ERROR_ADD_NOTIFICATIONDEF'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.notificationsListener = this.store.pipe(select(fromAppState.selectNotificationsResult)).subscribe((searchNotificationDefsResult: SearchNotificationDefsResult) => {
            if (!searchNotificationDefsResult) {
                return;
            }

            this.notificationDefs$ = searchNotificationDefsResult.content;
            this.length = searchNotificationDefsResult.totalLength;
        });
        this.refresh();
    }

    addNotification() {
        const dialogRef = this.dialog.open(AddNotificationDefDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((e: any) => {
            if (!e) {
                return;
            }


            const request = new fromNotificationDefsActions.AddNotificationDefOperation(e.name);
            this.store.dispatch(request);
        });
    }

    ngOnDestroy(): void {
        this.notificationsListener.unsubscribe();
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

        const request = new fromNotificationDefsActions.SearchNotificationDefOperation(active, direction, count, startIndex);
        this.store.dispatch(request);
    }
}
