import { createSelector } from '@ngrx/store';
import * as fromBpmnFiles from './bpmnfiles/reducers/bpmnfile.reducers';
import * as fromBpmnInstances from './bpmninstances/reducers/bpmninstance.reducers';
import * as fromCmmnPlan from './cmmnplans/reducers/cmmnplan.reducers';
import * as fromCmmnFile from './cmmnfiles/reducers/cmmnfile.reducers';
import * as fromCmmnPlanInstance from './cmmninstances/reducers/cmmn-planinstance.reducers';
import * as fromHumanTask from './humantaskdefs/reducers/humantaskdef.reducers';
import * as fromNotification from './notificationdefs/reducers/notificationdef.reducers';

export interface AppState {
    cmmnPlan: fromCmmnPlan.CmmnPlanState;
    cmmnPlanLst: fromCmmnPlan.SearchCmmnPlanState;
    cmmnFile: fromCmmnFile.CmmnFileState;
    cmmnFileLst: fromCmmnFile.CmmnFileLstState;
    cmmnPlanInstance: fromCmmnPlanInstance.CmmnPlanInstanceState;
    cmmnPlanInstanceLst: fromCmmnPlanInstance.CmmnPlanInstanceLstState;
    humanTask: fromHumanTask.HumanTaskDefState;
    humanTasks: fromHumanTask.SearchHumanTaskDefState;
    bpmnFiles: fromBpmnFiles.BpmnFileLstState;
    bpmnFile: fromBpmnFiles.BpmnFileState;
    bpmnInstances: fromBpmnInstances.BpmnInstanceLstState;
    bpmnInstance: fromBpmnInstances.BpmnInstanceState;
    notifications: fromNotification.SearchNotificationDefState,
    notification: fromNotification.NotificationDefState,
    notificationLst: fromNotification.NotificationDefLstState
}

export const selectCmmnPlan = (state: AppState) => state.cmmnPlan;
export const selectCmmnPlanLst = (state: AppState) => state.cmmnPlanLst;
export const selectCmmnFile = (state: AppState) => state.cmmnFile;
export const selectCmmnFileLst = (state: AppState) => state.cmmnFileLst;
export const selectCmmnPlanInstance = (state: AppState) => state.cmmnPlanInstance;
export const selectCmmnPlanInstanceLst = (state: AppState) => state.cmmnPlanInstanceLst;
export const selectHumanTask = (state: AppState) => state.humanTask;
export const selectHumanTasks = (state: AppState) => state.humanTasks;
export const selectBpmnFiles = (state: AppState) => state.bpmnFiles;
export const selectBpmnFile = (state: AppState) => state.bpmnFile;
export const selectBpmnInstances = (state: AppState) => state.bpmnInstances;
export const selectBpmnInstance = (state: AppState) => state.bpmnInstance;
export const selectNotifications = (state: AppState) => state.notifications;
export const selectNotification = (state: AppState) => state.notification;
export const selectNotificationLst = (state: AppState) => state.notificationLst;

export const selectNotificationLstResult = createSelector(
    selectNotificationLst,
    (state: fromNotification.NotificationDefLstState) => {
        if (!state || state.content == null) {
            return [];
        }

        return state.content;
    }
);

export const selectCmmnPlanResult = createSelector(
    selectCmmnPlan,
    (state: fromCmmnPlan.CmmnPlanState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCmmnPlanLstResult = createSelector(
    selectCmmnPlanLst,
    (state: fromCmmnPlan.SearchCmmnPlanState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCmmnFileResult = createSelector(
    selectCmmnFile,
    (state: fromCmmnFile.CmmnFileState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state;
    }
);

export const selectCmmnFileLstResult = createSelector(
    selectCmmnFileLst,
    (state: fromCmmnFile.CmmnFileLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCmmnPlanInstanceResult = createSelector(
    selectCmmnPlanInstance,
    (state: fromCmmnPlanInstance.CmmnPlanInstanceState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
); 

export const selectCmmnPlanInstanceLstResult = createSelector(
    selectCmmnPlanInstanceLst,
    (state: fromCmmnPlanInstance.CmmnPlanInstanceLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectBpmnInstanceResult = createSelector(
    selectBpmnInstance,
    (state: fromBpmnInstances.BpmnInstanceState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectHumanTaskResult = createSelector(
    selectHumanTask,
    (state: fromHumanTask.HumanTaskDefState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectHumanTasksResult = createSelector(
    selectHumanTasks,
    (state: fromHumanTask.SearchHumanTaskDefState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
); 

export const selectBpmnFilesResult = createSelector(
    selectBpmnFiles,
    (state: fromBpmnFiles.BpmnFileLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectBpmnFileResult = createSelector(
    selectBpmnFile,
    (state: fromBpmnFiles.BpmnFileState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state;
    }
);

export const selectBpmnInstancesResult = createSelector(
    selectBpmnInstances,
    (state: fromBpmnInstances.BpmnInstanceLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectNotificationsResult = createSelector(
    selectNotifications,
    (state: fromNotification.SearchNotificationDefState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectNotificationResult = createSelector(
    selectNotification,
    (state: fromNotification.NotificationDefState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const appReducer = {
    cmmnPlan: fromCmmnPlan.cmmnPlanReducer,
    cmmnPlanLst: fromCmmnPlan.cmmnPlanLstReducer,
    cmmnFile: fromCmmnFile.cmmnFileReducer,
    cmmnFileLst: fromCmmnFile.cmmnFileLstReducer,
    cmmnPlanInstance: fromCmmnPlanInstance.cmmnPlanInstanceReducer,
    cmmnPlanInstanceLst: fromCmmnPlanInstance.cmmnPlanInstanceLstReducer,
    humanTask: fromHumanTask.humanTaskDefReducer,
    humanTasks: fromHumanTask.humanTaskDefsReducer,
    bpmnFiles: fromBpmnFiles.bpmnFileLstReducer,
    bpmnFile: fromBpmnFiles.bpmnFileReducer,
    bpmnInstances: fromBpmnInstances.bpmnInstanceLstReducer,
    bpmnInstance: fromBpmnInstances.bpmnInstanceReducer,
    notifications: fromNotification.notificationDefsReducer,
    notification: fromNotification.notificationDefReducer,
    notificationLst: fromNotification.notificationDefLstReducer
};