import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { Notification } from '@app/stores/notifications/models/notification.model';
import { SearchNotificationResult } from '@app/stores/notifications/models/search-notification.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { SearchNotifications } from '@app/stores/notifications/actions/notifications.actions';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'list-notifications-component',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListNotificationsComponent implements OnInit {
    displayedColumns: string[] = ['priority', 'presentationName', 'presentationSubject', 'status', 'createdTime'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    baseTranslationKey: string = "NOTIFICATIONS.LIST";
    length: number;
    notifications$: Notification[] = [];

    constructor(private store: Store<fromAppState.AppState>,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private translate: TranslateService,
        public dialog: MatDialog) {
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectNotificationLstResult)).subscribe((l: SearchNotificationResult) => {
            if (!l || !l.content) {
                return;
            }

            this.notifications$ = l.content;
            this.length = l.totalLength;
        });
        this.activatedRoute.queryParamMap.subscribe((p: any) => {
            this.sort.active = p.get('active');
            this.sort.direction = p.get('direction');
            this.paginator.pageSize = p.get('pageSize');
            this.paginator.pageIndex = p.get('pageIndex');
            this.refresh()
        });
        this.translate.onLangChange.subscribe(() => {
            this.refresh();
        });
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => {
            this.refreshUrl();
        });
    }

    onSearchTasks() {
        this.refreshUrl();
    }

    refreshUrl() {
        const queryParams: any = {
            pageIndex: this.paginator.pageIndex,
            pageSize: this.paginator.pageSize,
            active: this.sort.active,
            direction: this.sort.direction
        };
        this.router.navigate(['.'], {
            relativeTo: this.activatedRoute,
            queryParams: queryParams
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

        let active = this.getOrder();
        let direction = this.getDirection();
        let request = new SearchNotifications(active, direction, count, startIndex);
        this.store.dispatch(request);
    }

    private getOrder() {
        let active = "createdTime";
        if (this.sort.active) {
            active = this.sort.active;
        }

        return active;
    }

    private getDirection() {
        let direction = "desc";
        if (this.sort.direction) {
            direction = this.sort.direction;
        }

        return direction;
    }
}