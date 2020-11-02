import { createSelector } from '@ngrx/store';
import * as fromNotification from './notifications/reducers/notifications.reducers';
import * as fromTask from './tasks/reducers/tasks.reducers';

export interface AppState {
    taskLst: fromTask.TaskLstState;
    task: fromTask.TaskState;
    notificationLst: fromNotification.NotificationLstState;
}

export const selectTaskLst = (state: AppState) => state.taskLst;
export const selectTask = (state: AppState) => state.task;
export const selectNotificationLst = (state: AppState) => state.notificationLst;

export const selectTaskLstResult = createSelector(
    selectTaskLst,
    (state: fromTask.TaskLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectNotificationLstResult = createSelector(
    selectNotificationLst,
    (state: fromNotification.NotificationLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const appReducer = {
    taskLst: fromTask.taskLstReducer,
    task: fromTask.taskReducer,
    notificationLst: fromNotification.notificationLstReducer
};