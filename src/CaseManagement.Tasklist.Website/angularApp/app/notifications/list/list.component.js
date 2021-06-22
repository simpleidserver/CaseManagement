var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild } from '@angular/core';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { Notification } from '@app/stores/notifications/models/notification.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { SearchNotifications } from '@app/stores/notifications/actions/notifications.actions';
import { TranslateService } from '@ngx-translate/core';
var ListNotificationsComponent = (function () {
    function ListNotificationsComponent(store, activatedRoute, router, translate, dialog) {
        this.store = store;
        this.activatedRoute = activatedRoute;
        this.router = router;
        this.translate = translate;
        this.dialog = dialog;
        this.displayedColumns = ['priority', 'presentationName', 'presentationSubject', 'status', 'createdTime'];
        this.notifications$ = [];
    }
    ListNotificationsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.notifications$ = [new Notification(), new Notification(), new Notification()];
        this.store.pipe(select(fromAppState.selectNotificationLstResult)).subscribe(function (l) {
            if (!l || !l.content || l.content.length === 0) {
                return;
            }
            _this.notifications$ = l.content;
            _this.length = l.totalLength;
        });
        this.activatedRoute.queryParamMap.subscribe(function (p) {
            _this.sort.active = p.get('active');
            _this.sort.direction = p.get('direction');
            _this.paginator.pageSize = p.get('pageSize');
            _this.paginator.pageIndex = p.get('pageIndex');
            _this.refresh();
        });
        this.translate.onLangChange.subscribe(function () {
            _this.refresh();
        });
    };
    ListNotificationsComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () {
            _this.refreshUrl();
        });
    };
    ListNotificationsComponent.prototype.onSearchTasks = function () {
        this.refreshUrl();
    };
    ListNotificationsComponent.prototype.refreshUrl = function () {
        var queryParams = {
            pageIndex: this.paginator.pageIndex,
            pageSize: this.paginator.pageSize,
            active: this.sort.active,
            direction: this.sort.direction
        };
        this.router.navigate(['.'], {
            relativeTo: this.activatedRoute,
            queryParams: queryParams
        });
    };
    ListNotificationsComponent.prototype.refresh = function () {
        var startIndex = 0;
        var count = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }
        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }
        var active = this.getOrder();
        var direction = this.getDirection();
        var request = new SearchNotifications(active, direction, count, startIndex);
        this.store.dispatch(request);
    };
    ListNotificationsComponent.prototype.getOrder = function () {
        var active = "createdTime";
        if (this.sort.active) {
            active = this.sort.active;
        }
        return active;
    };
    ListNotificationsComponent.prototype.getDirection = function () {
        var direction = "desc";
        if (this.sort.direction) {
            direction = this.sort.direction;
        }
        return direction;
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListNotificationsComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListNotificationsComponent.prototype, "sort", void 0);
    ListNotificationsComponent = __decorate([
        Component({
            selector: 'list-notifications-component',
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            Router,
            TranslateService,
            MatDialog])
    ], ListNotificationsComponent);
    return ListNotificationsComponent;
}());
export { ListNotificationsComponent };
//# sourceMappingURL=list.component.js.map