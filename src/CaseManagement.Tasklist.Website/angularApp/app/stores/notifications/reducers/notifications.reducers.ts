import * as fromActions from '../actions/notifications.actions';
import { SearchNotificationResult } from "../models/search-notification.model";

export interface NotificationLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchNotificationResult;
}

export const initialNotificationLstState: NotificationLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function notificationLstReducer(state = initialNotificationLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_NOTIFICATIONS:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}