import { createSelector } from '@ngrx/store';
import * as fromBpmnFiles from './bpmnfiles/reducers/bpmnfile.reducers';
import * as fromBpmnInstances from './bpmninstances/reducers/bpmninstance.reducers';
import * as fromCasePlanInstance from './caseplaninstances/reducers/caseplaninstance.reducers';
import * as fromCmmnPlan from './cmmnplans/reducers/cmmnplan.reducers';
import * as fromCmmnFile from './cmmnfiles/reducers/cmmnfile.reducers';
import * as fromHumanTask from './humantaskdefs/reducers/humantaskdef.reducers';

export interface AppState {
    cmmnPlan: fromCmmnPlan.CmmnPlanState;
    cmmnPlanLst: fromCmmnPlan.SearchCmmnPlanState;
    cmmnFile: fromCmmnFile.CmmnFileState;
    cmmnFileLst: fromCmmnFile.CmmnFileLstState;
    casePlanInstance: fromCasePlanInstance.CasePlanInstanceState;
    casePlanInstanceLst: fromCasePlanInstance.CasePlanInstanceLstState;
    humanTask: fromHumanTask.HumanTaskDefState;
    humanTasks: fromHumanTask.SearchHumanTaskDefState;
    bpmnFiles: fromBpmnFiles.BpmnFileLstState;
    bpmnFile: fromBpmnFiles.BpmnFileState;
    bpmnInstances: fromBpmnInstances.BpmnInstanceLstState;
    bpmnInstance: fromBpmnInstances.BpmnInstanceState
}

export const selectCmmnPlan = (state: AppState) => state.cmmnPlan;
export const selectCmmnPlanLst = (state: AppState) => state.cmmnPlanLst;
export const selectCmmnFile = (state: AppState) => state.cmmnFile;
export const selectCmmnFileLst = (state: AppState) => state.cmmnFileLst;
export const selectCasePlanInstance = (state: AppState) => state.casePlanInstance;
export const selectCasePlanInstanceLst = (state: AppState) => state.casePlanInstanceLst;
export const selectHumanTask = (state: AppState) => state.humanTask;
export const selectHumanTasks = (state: AppState) => state.humanTasks;
export const selectBpmnFiles = (state: AppState) => state.bpmnFiles;
export const selectBpmnFile = (state: AppState) => state.bpmnFile;
export const selectBpmnInstances = (state: AppState) => state.bpmnInstances;
export const selectBpmnInstance = (state: AppState) => state.bpmnInstance;

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

        return state.content;
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

export const selectCasePlanInstanceResult = createSelector(
    selectCasePlanInstance,
    (state: fromCasePlanInstance.CasePlanInstanceState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const selectCasePlanInstanceLstResult = createSelector(
    selectCasePlanInstanceLst,
    (state: fromCasePlanInstance.CasePlanInstanceLstState) => {
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

        return state.content;
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

export const appReducer = {
    cmmnPlan: fromCmmnPlan.cmmnPlanReducer,
    cmmnPlanLst: fromCmmnPlan.cmmnPlanLstReducer,
    cmmnFile: fromCmmnFile.cmmnFileReducer,
    cmmnFileLst: fromCmmnFile.cmmnFileLstReducer,
    casePlanInstance: fromCasePlanInstance.casePlanInstanceReducer,
    casePlanInstanceLst: fromCasePlanInstance.casePlanInstanceLstReducer,
    humanTask: fromHumanTask.humanTaskDefReducer,
    humanTasks: fromHumanTask.humanTaskDefsReducer,
    bpmnFiles: fromBpmnFiles.bpmnFileLstReducer,
    bpmnFile: fromBpmnFiles.bpmnFileReducer,
    bpmnInstances: fromBpmnInstances.bpmnInstanceLstReducer,
    bpmnInstance: fromBpmnInstances.bpmnInstanceReducer
};