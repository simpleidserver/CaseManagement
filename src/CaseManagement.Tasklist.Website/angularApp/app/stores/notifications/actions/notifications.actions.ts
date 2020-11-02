import { Action } from '@ngrx/store';
import { SearchNotificationResult } from '../models/search-notification.model';

export enum ActionTypes {
    SEARCH_NOTIFICATIONS = "[Notifications] SEARCH_NOTIFICATIONS",
    ERROR_SEARCH_NOTIFICATIONS = "[Notifications] ERROR_SEARCH_NOTIFICATIONS",
    COMPLETE_SEARCH_NOTIFICATIONS = "[Notifications] COMPLETE_SEARCH_NOTIFICATIONS"
}

export class SearchNotifications implements Action {
    readonly type = ActionTypes.SEARCH_NOTIFICATIONS
    constructor(public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearchNotifications implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_NOTIFICATIONS;
    constructor(public content: SearchNotificationResult) { }
}

export type ActionsUnion = SearchNotifications |
    CompleteSearchNotifications;