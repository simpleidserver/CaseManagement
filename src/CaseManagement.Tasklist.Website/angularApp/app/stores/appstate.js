import { createSelector } from '@ngrx/store';
import * as fromNotification from './notifications/reducers/notifications.reducers';
import * as fromTask from './tasks/reducers/tasks.reducers';
export var selectTaskLst = function (state) { return state.taskLst; };
export var selectTask = function (state) { return state.task; };
export var selectNotificationLst = function (state) { return state.notificationLst; };
export var selectTaskLstResult = createSelector(selectTaskLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var selectNotificationLstResult = createSelector(selectNotificationLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var appReducer = {
    taskLst: fromTask.taskLstReducer,
    task: fromTask.taskReducer,
    notificationLst: fromNotification.notificationLstReducer
};
//# sourceMappingURL=appstate.js.map