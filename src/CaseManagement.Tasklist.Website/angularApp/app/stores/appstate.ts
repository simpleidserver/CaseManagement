import { createSelector } from '@ngrx/store';
import * as fromCase from './cases/reducers/cases.reducers';
import * as fromNotification from './notifications/reducers/notifications.reducers';
import * as fromTask from './tasks/reducers/tasks.reducers';

export interface AppState {
    taskLst: fromTask.TaskLstState;
    task: fromTask.TaskState;
    caseLst: fromCase.CaseLstState;
    case: fromCase.CaseState;
    notificationLst: fromNotification.NotificationLstState;
}

export const selectTaskLst = (state: AppState) => state.taskLst;
export const selectTask = (state: AppState) => state.task;
export const selectNotificationLst = (state: AppState) => state.notificationLst;
export const selectCaseLst = (state: AppState) => state.caseLst;
export const selectCase = (state: AppState) => state.case;

export const selectTaskLstResult = createSelector(
    selectTaskLst,
    (state: fromTask.TaskLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCaseLstResult = createSelector(
    selectCaseLst,
    (state: fromCase.CaseLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCaseResult = createSelector(
    selectCase,
    (state: fromCase.CaseState) => {
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
    caseLst: fromCase.caseLstReducer,
    case: fromCase.caseReducer,
    notificationLst: fromNotification.notificationLstReducer
};